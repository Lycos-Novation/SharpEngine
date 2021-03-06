using Microsoft.Xna.Framework;
using SharpEngine.Managers;
using SharpEngine.Utils;
using Color = SharpEngine.Utils.Color;
using GameTime = SharpEngine.Utils.GameTime;

namespace SharpEngine.Components;

/// <summary>
/// Composant ajoutant l'affichage d'un sprite
/// </summary>
public class SpriteComponent: Component
{
    public string Sprite;
    public bool Displayed;
    public Vec2 Offset;

    /// <summary>
    /// Initialise le Composant.
    /// </summary>
    /// <param name="sprite">Nom de la texture</param>
    /// <param name="displayed">Est affiché</param>
    /// <param name="offset">Décalage de la position du Sprite (Vec2(0))</param>
    public SpriteComponent(string sprite, bool displayed = true, Vec2 offset = null)
    {
        Sprite = sprite;
        Displayed = displayed;
        Offset = offset ?? new Vec2(0);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        if (Entity.GetComponent<TransformComponent>() is not { } tc || !Displayed || Sprite.Length <= 0) return;
        
        var texture = GetWindow().TextureManager.GetTexture(Sprite);
        GetSpriteBatch().Draw(texture, (tc.Position + Offset - CameraManager.Position).ToMg(), null, Color.White.ToMg(), MathHelper.ToRadians(tc.Rotation), new Vector2(texture.Width, texture.Height) / 2, tc.Scale.ToMg(), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 1);
    }

    public override string ToString() => $"SpriteComponent(sprite={Sprite}, displayed={Displayed}, offset={Offset})";
}
