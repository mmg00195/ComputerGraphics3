using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;


namespace ComputerGraphics3
{
    public class Window : GameWindow
    {

        private Shader shader;
        private Shader shader2;
        private Texture texture;
        private Texture table_texture;

        private Camera cam;
        private bool _firstMove = true;
        private Vector2 _lastPos;

        private Randomizer rando;
        private Room room;

        private obj1 obj1;
        private List<obj1> polygon;
        int polnum;

        private List<furniture> roomObjects;



        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            rando = new Randomizer();
            polygon = new List<obj1>();
            polnum = 0;
            roomObjects = new List<furniture>();
            DisplayHelp();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            shader = new Shader("C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.vert", "C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.frag");
            shader.Use();

            
            texture = Texture.LoadFromFile("C:/Users/manue/source/repos/ComputerGraphics3/Resources/container.png");
            texture.Use(TextureUnit.Texture0);
            shader.SetInt("texture0", 0);
            
            cam = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            CursorState = CursorState.Grabbed;

            room = new Room(shader, texture);
            room.Scale(new Vector3(10.0f, 10.0f, 10.0f));

            shader2 = new Shader("C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.vert", "C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader_solid.frag");
            shader2.Use();
            shader2.SetInt("objectColor",0);

            polygon.Add(new obj1(shader2, rando.RandomInt(1, 4)));

            table_texture = Texture.LoadFromFile("C:/Users/manue/source/repos/ComputerGraphics3/Resources/beige-wooden-texture.jpg");
            table_texture.Use(TextureUnit.Texture0);
            shader.SetInt("texture0", 0);
            table table = new table(shader, table_texture, new Vector3(1.5f, 1.0f, 1.2f), new Vector3(0.0f,-2.1f,-3.7f));
            roomObjects.Add(table);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            // We need to update the aspect ratio once the window has been resized.
            if (cam != null)
            {
                cam.AspectRatio = Size.X / (float)Size.Y;
            }
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;
            var mouseinput = MouseState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;
            if (!mouseinput.IsButtonDown(MouseButton.Left))
            {

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
            }

            if (input.IsKeyDown(Keys.H))
            {
                DisplayHelp();
            }
            if (input.IsKeyPressed(Keys.B))
            {
                GL.ClearColor(rando.RandomColor());
            }
            if (input.IsKeyPressed(Keys.C))
            {
                polygon[polnum].ToggleColor();
            }
            if (input.IsKeyPressed(Keys.V))
            {
                polygon[polnum].ToggleVisibility();
            }
            if (input.IsKeyPressed(Keys.X))
            {
                polygon[polnum].ToggleWireframeMode();
            }
            if (input.IsKeyPressed(Keys.G))
            {
                polygon[polnum].ToggleGravity();
            }
            if (input.IsKeyPressed(Keys.D1))
            {
                polygon.Add(new obj1(shader2,1));
                polnum = polygon.Count - 1;
                polygon[polnum].Translate(new Vector3(rando.RandomInt(-5,5), rando.RandomInt(-5, 5), rando.RandomInt(-5, 5)));
            }
            if (input.IsKeyPressed(Keys.D2))
            {
                polygon.Add(new obj1(shader2, 2));
                polnum = polygon.Count - 1;
                polygon[polnum].Translate(new Vector3(rando.RandomInt(-5, 5), rando.RandomInt(-5, 5), rando.RandomInt(-5, 5)));
            }
            if (input.IsKeyPressed(Keys.D3))
            {
                polygon.Add(new obj1(shader2, 3));
                polnum = polygon.Count - 1;
                polygon[polnum].Translate(new Vector3(rando.RandomInt(-5, 5), rando.RandomInt(-5, 5), rando.RandomInt(-5, 5)));
            }
            if (input.IsKeyPressed(Keys.LeftAlt))
            {
                if (polnum == polygon.Count - 1)
                    polnum = 0;
                else
                    polnum += 1;
            }

            if (input.IsKeyDown(Keys.M))
            {
                foreach (obj1 pol in polygon)
                    pol.DiscoMode();
            }

            if (input.IsKeyDown(Keys.Backspace))
            {
                polygon.Clear();
                polygon.Add(new obj1(shader2, rando.RandomInt(1,4)));
                polnum = polygon.Count - 1;
            }
            float deltaTime = (float)e.Time;
            polygon[polnum].Update(deltaTime);
            
            
            
            //mouse
            var mouse = MouseState;
            // Calculate the offset of the mouse position
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            if (_firstMove) // This bool variable is initially set to true.
            {
                _firstMove = false;
            }
            else
            {
                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                cam.Yaw += deltaX * sensitivity;
                cam.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }

            
            if (mouseinput.IsButtonDown(MouseButton.Left))
            {
                if (input.IsKeyDown(Keys.W))
                    polygon[polnum].Translate(-Vector3.UnitZ * cameraSpeed * (float)e.Time);
                if (input.IsKeyDown(Keys.S))
                    polygon[polnum].Translate(Vector3.UnitZ * cameraSpeed * (float)e.Time);
                if (input.IsKeyDown(Keys.A))
                    polygon[polnum].Translate(-Vector3.UnitX * cameraSpeed * (float)e.Time);
                if (input.IsKeyDown(Keys.D))
                    polygon[polnum].Translate(Vector3.UnitX * cameraSpeed * (float)e.Time);
                if (input.IsKeyDown(Keys.Space))
                    polygon[polnum].Translate(Vector3.UnitY * cameraSpeed * (float)e.Time);
                if (input.IsKeyDown(Keys.LeftShift))
                    polygon[polnum].Translate(-Vector3.UnitY * cameraSpeed * (float)e.Time);

                //polygon[polnum].Rotate(deltaY, Vector3.UnitX);
                //polygon[polnum].Rotate(deltaX, Vector3.UnitY);

            }
        }

        

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            cam.Fov -= e.OffsetY;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            texture.Use(TextureUnit.Texture0);
            shader.Use();
            shader2.Use();

            /*var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", cam.GetViewMatrix());
            shader.SetMatrix4("projection", cam.GetProjectionMatrix());
            */
            //GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            room.Render(cam);
            foreach (obj1 pol in polygon)
            {
                pol.Render(cam);
            }

            foreach (furniture furn in roomObjects)
            {
                furn.Render(cam);
            }
            SwapBuffers();
        }

        private void DisplayHelp()
        {
            Console.WriteLine("\n");
            Console.WriteLine("__________App Tools_________");
            Console.WriteLine(" (H) - help menu");
            Console.WriteLine(" (ESC) - stop application");
            Console.WriteLine(" (B) - toggle landscape colour");
            Console.WriteLine(" (W,A,S,D, shift, space) - camera movement");
            Console.WriteLine("\n");
            Console.WriteLine("__________Object Tools_________");
            Console.WriteLine(" (LefttClick) - selected pol movement");
            Console.WriteLine(" (LeftAlt) - select form");
            Console.WriteLine(" (1,2,3) - add new form");
            Console.WriteLine(" (C) - toggle object colour");
            Console.WriteLine(" (V) - toggle object visibility");
            Console.WriteLine(" (X) - toggle object Wireframe/Surface Mode");
            Console.WriteLine(" (G) - toggle object Gravity");
            Console.WriteLine(" (Backspace) - remove all objects");
            Console.WriteLine(" (Hold M) - Disco Mode");
            Console.WriteLine("\n");
            Console.WriteLine("__________Surprises_________");
            Console.WriteLine("() - Manu Surprise");
            Console.WriteLine("() - Vasco Surprise");
            Console.WriteLine("() - Celia Surprise");


        }
    }
}
