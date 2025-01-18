using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ComputerGraphics3;

public class Window : GameWindow
{
    private readonly List<Object> polygon;

    private readonly Randomizer rando;

    private readonly List<furniture> roomObjects;
    private readonly List<Texture> table_texture;
    private bool _firstMove = true;
    private Vector2 _lastPos;

    private Axes axes;

    private Camera cam;
    private int currentText;

    private Object Object;
    private Shader objShader;
    private int polnum;
    private Room room;

    private Shader roomShader;
    private Shader shader;
    private Shader shader2;
    private Shader tableShader;
    private Texture texture;


    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        rando = new Randomizer();
        polygon = new List<Object>();
        polnum = -1;
        roomObjects = new List<furniture>();
        table_texture = new List<Texture>();
        currentText = 0;
        DisplayHelp();
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        GL.Enable(EnableCap.DepthTest);

        roomShader = new Shader("C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.vert",
            "C:/Users/manue/source/repos/ComputerGraphics3/Shaders/lighting.frag");
        objShader = new Shader("C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.vert",
            "C:/Users/manue/source/repos/ComputerGraphics3/Shaders/lighting.frag");
        tableShader = new Shader("C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.vert",
            "C:/Users/manue/source/repos/ComputerGraphics3/Shaders/lighting.frag");

        shader = new Shader("C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.vert",
            "C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.frag");

        shader.Use();
        roomShader.Use();
        tableShader.Use();
        objShader.Use();

        texture = Texture.LoadFromFile("C:/Users/manue/source/repos/ComputerGraphics3/Resources/container.png");
        texture.Use(TextureUnit.Texture0);

        cam = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

        CursorState = CursorState.Grabbed;

        room = new Room(shader, roomShader, texture);
        room.Scale(new Vector3(10.0f, 10.0f, 10.0f));

        shader2 = new Shader("C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.vert",
            "C:/Users/manue/source/repos/ComputerGraphics3/Shaders/shader.frag");
        shader2.Use();
        axes = new Axes(shader2);

        //polygon.Add(new Object(shader2, rando.RandomInt(1, 4)));

        table_texture.Add(
            Texture.LoadFromFile("C:/Users/manue/source/repos/ComputerGraphics3/Resources/beige-wooden-texture.jpg"));
        table_texture.Add(
            Texture.LoadFromFile("C:/Users/manue/source/repos/ComputerGraphics3/Resources/dark_wooden_texture.jpg"));
        table_texture.Add(
            Texture.LoadFromFile("C:/Users/manue/source/repos/ComputerGraphics3/Resources/natural_wooden_texture.jpg"));
        table_texture.Add(
            Texture.LoadFromFile("C:/Users/manue/source/repos/ComputerGraphics3/Resources/metal_texture.jpg"));
        currentText = 3;
        table_texture[0].Use(TextureUnit.Texture0);
        tableShader.SetInt("material.diffuse", 0);
        var table = new table(tableShader, currentText, table_texture, new Vector3(2.3f, 1.0f, 1.2f),
            new Vector3(0.0f, -2.1f, -3.7f));
        roomObjects.Add(table);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, Size.X, Size.Y);
        // We need to update the aspect ratio once the window has been resized.
        if (cam != null) cam.AspectRatio = Size.X / (float)Size.Y;
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);

        if (!IsFocused) // Check to see if the window is focused
            return;

        var input = KeyboardState;
        var mouseinput = MouseState;
        var deltaTime = (float)e.Time;

        if (input.IsKeyDown(Keys.Escape)) Close();

        const float cameraSpeed = 1.5f;
        const float sensitivity = 0.1f;
        if (!mouseinput.IsButtonDown(MouseButton.Left))
        {
            if (input.IsKeyDown(Keys.W)) cam.Position += cam.Front * cameraSpeed * (float)e.Time; // Forward

            if (input.IsKeyDown(Keys.S)) cam.Position -= cam.Front * cameraSpeed * (float)e.Time; // Backwards

            if (input.IsKeyDown(Keys.A)) cam.Position -= cam.Right * cameraSpeed * (float)e.Time; // Left

            if (input.IsKeyDown(Keys.D)) cam.Position += cam.Right * cameraSpeed * (float)e.Time; // Right

            if (input.IsKeyDown(Keys.Space)) cam.Position += cam.Up * cameraSpeed * (float)e.Time; // Up

            if (input.IsKeyDown(Keys.LeftShift)) cam.Position -= cam.Up * cameraSpeed * (float)e.Time; // Down
        }

        if (input.IsKeyDown(Keys.H)) DisplayHelp();
        if (input.IsKeyPressed(Keys.B)) GL.ClearColor(rando.RandomColor());

        if (input.IsKeyPressed(Keys.Z)) axes.ToggleVisibility();
        if (input.IsKeyPressed(Keys.D1))
        {
            polygon.Add(new Object(objShader, 1));
            polnum = polygon.Count - 1;
            polygon[polnum]
                .Translate(new Vector3(rando.RandomInt(-5, 5), rando.RandomInt(-5, 5), rando.RandomInt(-5, 5)));
        }

        if (input.IsKeyPressed(Keys.D2))
        {
            polygon.Add(new Object(objShader, 2));
            polnum = polygon.Count - 1;
            polygon[polnum]
                .Translate(new Vector3(rando.RandomInt(-5, 5), rando.RandomInt(-5, 5), rando.RandomInt(-5, 5)));
        }

        if (input.IsKeyPressed(Keys.D3))
        {
            polygon.Add(new Object(objShader, 3));
            polnum = polygon.Count - 1;
            polygon[polnum]
                .Translate(new Vector3(rando.RandomInt(-5, 5), rando.RandomInt(-5, 5), rando.RandomInt(-5, 5)));
        }

        if (input.IsKeyPressed(Keys.T))
            foreach (var furn in roomObjects)
                furn.ToggleTexture();

        //Camera teleport to X position
        if (input.IsKeyPressed(Keys.D8))
        {
            cam.MoveTo(new Vector3(-10.0f, 10.0f, 10.0f));
        }
        if (input.IsKeyPressed(Keys.D9))
        {
            cam.MoveTo(new Vector3(-10.0f, 10.0f, -10.0f));
        }
        if (input.IsKeyPressed(Keys.D0))
        {
            cam.MoveTo(new Vector3(-5.0f, 5.0f, 5.0f));
        }


        if (polnum >= 0)
        {
            if (input.IsKeyPressed(Keys.C)) polygon[polnum].ToggleColor();

            if (input.IsKeyPressed(Keys.V)) polygon[polnum].ToggleVisibility();

            if (input.IsKeyPressed(Keys.X)) polygon[polnum].ToggleWireframeMode();

            if (input.IsKeyPressed(Keys.G)) polygon[polnum].ToggleGravity();

            if (input.IsKeyDown(Keys.Down))
                foreach (var pol in polygon)
                {
                    pol.GlobalGravity();
                    pol.Update(deltaTime);
                }

            if (input.IsKeyDown(Keys.Up))
                foreach (var pol in polygon)
                {
                    pol.GlobalGravityInv();
                    pol.Update(deltaTime);
                }

            if (input.IsKeyDown(Keys.Left))
                foreach (var pol in polygon)
                {
                    pol.GlobalGravityLeft();
                    pol.Update(deltaTime);
                }

            if (input.IsKeyDown(Keys.Right))
                foreach (var pol in polygon)
                {
                    pol.GlobalGravityRight();
                    pol.Update(deltaTime);
                }

            if (input.IsKeyReleased(Keys.Up) || input.IsKeyReleased(Keys.Down) || input.IsKeyReleased(Keys.Left) ||
                input.IsKeyReleased(Keys.Right))
                foreach (var pol in polygon)
                    pol.ResetGravity();

            if (input.IsKeyPressed(Keys.LeftAlt))
            {
                if (polnum == polygon.Count - 1)
                    polnum = 0;
                else
                    polnum += 1;
            }

            if (input.IsKeyDown(Keys.M))
                foreach (var pol in polygon)
                    pol.DiscoMode();

            if (input.IsKeyDown(Keys.Backspace))
            {
                polygon.Clear();
                polnum = -1;
            }
            else
            {
                polygon[polnum].Update(deltaTime);
            }
        }


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


        if (mouseinput.IsButtonDown(MouseButton.Left) && polnum >= 0)
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
        objShader.Use();
        tableShader.Use();
        roomShader.Use();
        shader.Use();
        shader2.Use();

        room.Render(cam);
        axes.Render(cam);
        foreach (var pol in polygon) pol.Render(cam, room);

        foreach (var furn in roomObjects) furn.Render(cam, room);

        SwapBuffers();
    }

    private void DisplayHelp()
    {
        Console.WriteLine("\n");
        Console.WriteLine("__________App Tools_________");
        Console.WriteLine(" (H) - help menu");
        Console.WriteLine(" (ESC) - stop application");
        Console.WriteLine(" (B) - toggle landscape colour");
        Console.WriteLine(" (Z) - toggle axes visibility");
        Console.WriteLine(" (W,A,S,D, shift, space) - camera movement");
        Console.WriteLine(" (mouse wheel) - camera fov");
        Console.WriteLine(" (8,9,0) - camera position");
        Console.WriteLine("\n");
        Console.WriteLine("__________Object Tools_________");
        Console.WriteLine(" (LeftClick) - selected pol movement");
        Console.WriteLine(" (LeftAlt) - select pol");
        Console.WriteLine(" (1,2,3) - add new pol(1-cube, 2-triangle, 3-sphere)");
        Console.WriteLine(" (C) - toggle object colour");
        Console.WriteLine(" (V) - toggle object visibility");
        Console.WriteLine(" (X) - toggle object Wireframe/Surface Mode");
        Console.WriteLine(" (G) - toggle object Gravity");
        Console.WriteLine(" (Backspace) - remove all objects");
        Console.WriteLine("\n");
        Console.WriteLine("__________Surprises_________");
        Console.WriteLine("(Hold M) -  Vasco Surprise");
        Console.WriteLine("(Arrows) -  Manuel Surprise");
        Console.WriteLine("(T)      -  Celia Surprise");
    }
}