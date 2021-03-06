using System;
using SharpEngine.Utils;

namespace SharpEngine.Managers;

/// <summary>
/// Gestion des informations Debug
/// </summary>
public static class DebugManager
{
    private static int _frameRate;
    private static int _frameCounter;
    private static TimeSpan _elapsedTime = TimeSpan.Zero;

    public static int GetFps() => _frameRate;
    public static long GetGcMemory() => GC.GetTotalMemory(false);
    public static string GetMonogameVersion() => System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Microsoft.Xna.Framework.Game).Assembly.Location).FileVersion;
    public static string GetSharpEngineVersion() => "0.10.0";

    internal static void Update(GameTime gameTime)
    {
        _elapsedTime += gameTime.ElapsedGameTime;

        if (_elapsedTime <= TimeSpan.FromSeconds(1)) return;
        
        _elapsedTime -= TimeSpan.FromSeconds(1);
        _frameRate = _frameCounter;
        _frameCounter = 0;
    }

    internal static void Draw() => _frameCounter++;
}
