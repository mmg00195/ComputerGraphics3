using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics3;

internal class furniture
{
    protected uint[] _indices;
    protected Matrix4 _modelMatrix;
    protected Vector3 _position; // Obj pos
    protected Shader _shader;

    protected Vector3 _size; // obj size
    protected List<Texture> _texture;

    protected int _vao, _vbo, _ebo;
    protected float[] _vertices;
    protected int currentText;

    public furniture(Shader shader, int i, List<Texture> texture, Vector3 size, Vector3 position)
    {
        _shader = shader;
        _texture = texture;
        _size = size;
        _position = position;
        currentText = i;

        _modelMatrix = Matrix4.CreateTranslation(position);

        _vertices = new float[] { };
        _indices = new uint[] { };

        SetupBuffers();
    }

    protected void SetupBuffers()
    {
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
            BufferUsageHint.StaticDraw);

        var positionLocation = _shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
        GL.EnableVertexAttribArray(positionLocation);

        // Configura atributos de normales
        var normalLocation = _shader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float),
            3 * sizeof(float));

        var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float),
            6 * sizeof(float));
        GL.EnableVertexAttribArray(texCoordLocation);

        GL.BindVertexArray(0);
    }

    public virtual void ToggleTexture()
    {
        currentText = (currentText + 1) % _texture.Count;
    }

    public virtual void Render(Camera camera, Room room)
    {
        _shader.Use();
        _shader.SetMatrix4("model", _modelMatrix);
        _shader.SetMatrix4("view", camera.GetViewMatrix());
        _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

        _shader.SetVector3("viewPos", camera.Position);

        _texture[currentText].Use(TextureUnit.Texture0);

        _shader.SetInt("material.diffuse", 0);
        _shader.SetInt("material.specular", 0);
        _shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _shader.SetFloat("material.shininess", 32.0f);

        room.ConfigureLighting(_shader, camera, 1);

        GL.BindVertexArray(_vao);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }
}