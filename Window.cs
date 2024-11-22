using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;


namespace ComputerGraphics3
{
    public class Window : GameWindow
    {
        private int _elementBufferObject;
        private int _vertexBufferObject;
        private int _vertexArrayObject;

        private Shader shader;
        private Texture texture;

        private Camera cam;
        private bool _firstMove = true;
        private Vector2 _lastPos;

        private Randomizer rando;
        private Room room;

        private double _time;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            rando = new Randomizer();

            DisplayHelp();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            shader = new Shader("C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.vert", "C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.frag");
            shader.Use();

            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            texture = Texture.LoadFromFile("C:/Users/manue/source/repos/ComputerGraphics3/Resources/container.png");
            texture.Use(TextureUnit.Texture0);
            //shader.SetInt("texture0", 0);
            
            cam = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            CursorState = CursorState.Grabbed;

            room = new Room(shader, texture);
            room.Scale(new Vector3(10.0f, 10.0f, 10.0f));
            cam = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            // We need to update the aspect ratio once the window has been resized.
            cam.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                cam.Position += cam.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                cam.Position -= cam.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                cam.Position -= cam.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                cam.Position += cam.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                cam.Position += cam.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                cam.Position -= cam.Up * cameraSpeed * (float)e.Time; // Down
            }
            if (input.IsKeyDown(Keys.H))
            {
                DisplayHelp();
            }
            if (input.IsKeyDown(Keys.B))
            {
                GL.ClearColor(rando.RandomColor());
            }

            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                cam.Yaw += deltaX * sensitivity;
                cam.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            _time += 4.0 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindVertexArray(_vertexArrayObject);

            texture.Use(TextureUnit.Texture0);
            shader.Use();

            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", cam.GetViewMatrix());
            shader.SetMatrix4("projection", cam.GetProjectionMatrix());

            //GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

            room.Render(cam);

            SwapBuffers();
        }

        private void DisplayHelp()
        {
            Console.WriteLine("\n");
            Console.WriteLine(" (H) - help menu");
            Console.WriteLine(" (ESC) - stop aplication");
            Console.WriteLine(" (B) - toggle grid colour");
            //Console.WriteLine(" (V) - toggle grid visibilit");
            Console.WriteLine(" (W,A,S,D, shift, space) - camera movement");
        }
    }
}
