﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace SharpEngine
{
    /// <summary>
    /// Classe interne du jeu
    /// </summary>
    public class InternalGame : Game
    {
        public readonly GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        private readonly Window window;

        public InternalGame(Window win) : base()
        {
            window = win;

            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = window.mouseVisible;
        }

        internal void SetFullscreen(FullScreenType fullScreenType)
        {
            switch(fullScreenType) {
                case FullScreenType.BORDERLESS_FULLSCREEN:
                    if (graphics.IsFullScreen)
                        graphics.ToggleFullScreen();
                    int displayWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    int displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    graphics.PreferredBackBufferWidth = displayWidth;
                    graphics.PreferredBackBufferHeight = displayHeight;

                    Window.IsBorderless = true;
                    graphics.ApplyChanges();
                    break;
                case FullScreenType.HARDWARE_FULLSCREEN:
                    SetFullscreen(FullScreenType.NO_FULLSCREEN);
                    graphics.ToggleFullScreen();
                    graphics.ApplyChanges();
                    break;
                case FullScreenType.NO_FULLSCREEN:
                    if (graphics.IsFullScreen)
                        graphics.ToggleFullScreen();
                    graphics.PreferredBackBufferWidth = (int)window.screenSize.x;
                    graphics.PreferredBackBufferHeight = (int)window.screenSize.y;

                    Window.IsBorderless = false;
                    graphics.ApplyChanges();
                    break;
            }
        }

        internal void TakeScreenshot(string fileName)
        {
            int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int h = GraphicsDevice.PresentationParameters.BackBufferHeight;

            Draw(new Microsoft.Xna.Framework.GameTime());

            int[] backBuffer = new int[w * h];
            GraphicsDevice.GetBackBufferData(backBuffer);
 
            Texture2D texture = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
            texture.SetData(backBuffer);

            Stream stream = File.OpenWrite(fileName + ".jpg");

            texture.SaveAsJpeg(stream, w, h);
            stream.Dispose();
            texture.Dispose();
        }

        protected void TextInputHandler(object sender, TextInputEventArgs args)
        {
            if (window.currentScene != -1)
                window.scenes[window.currentScene].TextInput(sender, (Inputs.Key)args.Key, args.Character);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = (int)window.screenSize.x;
            graphics.PreferredBackBufferHeight = (int)window.screenSize.y;
            graphics.ApplyChanges();

            SetFullscreen(window.fullscreen);

            Window.TextInput += TextInputHandler;

            window.textureManager.Load();
            window.fontManager.Load();

            foreach (Scene scene in window.scenes)
                scene.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D blank = new Texture2D(GraphicsDevice, 1, 1);
            blank.SetData(new[] { Color.WHITE.ToMG() });
            window.textureManager.AddTexture("blank", blank);

            foreach (Scene scene in window.scenes)
                scene.LoadContent();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            foreach (Scene scene in window.scenes)
                scene.UnloadContent();

            window.textureManager.Unload();
            MusicManager.Unload();
            SoundManager.Unload();

            base.UnloadContent();
        }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            DebugManager.Update(GameTime.FromMonogameGameTime(gameTime));
            CameraManager.Update(window.screenSize);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (window.currentScene != -1)
                window.scenes[window.currentScene].Update(GameTime.FromMonogameGameTime(gameTime));

            InputManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            DebugManager.Draw();

            GraphicsDevice.Clear(window.backgroundColor.ToMG());

            spriteBatch.Begin();
            if (window.currentScene != -1)
                window.scenes[window.currentScene].Draw(GameTime.FromMonogameGameTime(gameTime));
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
