using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ComputerGraphics3
{
    public static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const uint SWP_NOSIZE = 0x0001;    // No cambiar el tamaño
        private const uint SWP_NOZORDER = 0x0004;  // No cambiar el orden Z

        private static void MoveDebugConsoleToCorner()
        {
            IntPtr consoleHandle = GetConsoleWindow();
            if (consoleHandle != IntPtr.Zero)
            {
                // Cambia la posición de la consola. Ejemplo: esquina superior izquierda
                SetWindowPos(consoleHandle, IntPtr.Zero, -700, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
            }
        }
        private static void Main()
        {

            MoveDebugConsoleToCorner();

            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new Vector2i(800, 600),
                Location = new Vector2i(700, 200),
                Title = "Element Erasmus Graphics",

                // This is needed to run on macos???
                Flags = ContextFlags.ForwardCompatible,
            };

            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {

                window.Run();
            }
        }
    }
}
    
