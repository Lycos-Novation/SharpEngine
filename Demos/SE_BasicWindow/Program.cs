using SharpEngine;
using SharpEngine.Utils;

namespace SE_BasicWindow;

internal static class Program
{
    private static void Main()
    {
        var win = new Window(new Vec2(900, 600), Color.CornflowerBlue);

        win.AddScene(new MyScene());
        win.Run();
    }
}