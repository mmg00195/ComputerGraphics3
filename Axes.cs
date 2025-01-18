using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ComputerGraphics3;

internal class Axes
{
    private readonly float[] vertices;
    protected uint[] _indices;
    protected Shader _shader;

    protected int _vao, _vbo;
    protected float[] _vertices;
    private bool myvisibility;

    public Axes(Shader shader)
    {
        vertices = new[]
        {
            //Position  //Color
            0f, 0f, 0f, 1f, 0f, 0f,
            5f, 0f, 0f, 1f, 0f, 0f,

            0f, 0f, 0f, 0f, 1f, 0f,
            0f, 5f, 0f, 0f, 1f, 0f,

            0f, 0f, 0f, 0f, 0f, 1f,
            0f, 0f, 5f, 0f, 0f, 1f
        };
        _shader = shader;
        myvisibility = true;
        SetupBuffers();
    }

    protected void SetupBuffers()
    {
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices,
            BufferUsageHint.StaticDraw);

        var positionLocation = _shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
        GL.EnableVertexAttribArray(positionLocation);

        var colorLocation = _shader.GetAttribLocation("aColor");
        GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float),
            3 * sizeof(float));
        GL.EnableVertexAttribArray(colorLocation);

        GL.BindVertexArray(0);
    }

    public void Render(Camera camera)
    {
        _shader.Use();
        _shader.SetMatrix4("model", Matrix4.Identity);
        _shader.SetMatrix4("view", camera.GetViewMatrix());
        _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

        // Renderiza un cubo o líneas que representen la hitbox
        GL.BindVertexArray(_vao);
        if (myvisibility)
            GL.DrawArrays(PrimitiveType.Lines, 0, 6);
    }

    public void ToggleVisibility()
    {
        myvisibility = !myvisibility;
    }
}