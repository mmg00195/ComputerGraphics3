using OpenTK.Mathematics;
using BufferTarget = OpenTK.Graphics.OpenGL4.BufferTarget;
using BufferUsageHint = OpenTK.Graphics.OpenGL4.BufferUsageHint;
using GL = OpenTK.Graphics.OpenGL4.GL;
using PrimitiveType = OpenTK.Graphics.OpenGL4.PrimitiveType;
using TextureUnit = OpenTK.Graphics.OpenGL4.TextureUnit;
using VertexAttribPointerType = OpenTK.Graphics.OpenGL4.VertexAttribPointerType;

namespace ComputerGraphics3;

internal class Room
{
    private readonly Shader _lampShader;
    private readonly Shader _lightingShader;

    public readonly Vector3[] _pointLightPos =
    {
        new(0.0f, 4.0f, 0.0f),
        new(10.2f, 10.0f, 20.0f)
    };

    private readonly Texture _texture;

    private readonly float[] _vertices =
    {
        // Positions         //Normals    // Texture coordinates
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, // Back face
        0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
        0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
        0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,

        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, // Front face
        0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,

        -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f, // Left face
        -0.5f, 0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        -0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

        0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, // Right face
        0.5f, 0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
        0.5f, -0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

        -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f, // Bottom face
        0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
        0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
        0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
        -0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
        -0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,

        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, // Top face
        0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
        0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
        -0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
        -0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f
    };

    private int _vaoLamp;
    private int _vaoModel;



    private int _vbo;

    //public BoundingBox boundingbox { get; private set; }
    public Room(Shader lampShader, Shader lightingShader, Texture texture)
    {
        _lampShader = lampShader;
        _lightingShader = lightingShader;
        _texture = texture;

        ModelMatrix = Matrix4.Identity;
        //boundingbox = new BoundingBox(Vector3.Zero, new Vector3(1.0f, 1.0f, 1.0f));
        SetupBuffers();
    }


    public Matrix4 ModelMatrix { get; private set; }

    private void SetupBuffers()
    {
        _vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);

        _vaoModel = GL.GenVertexArray();
        GL.BindVertexArray(_vaoModel);

        // Configura atributos de posición
        var positionLocation = _lightingShader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        // Configura atributos de normales
        var normalLocation = _lightingShader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float),
            3 * sizeof(float));

        // Configura atributos de textura
        var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float),
            6 * sizeof(float));

        _vaoLamp = GL.GenVertexArray();
        GL.BindVertexArray(_vaoLamp);

        // The lamp shader should have its stride updated aswell, however we dont actually
        // use the texture coords for the lamp, so we dont need to add any extra attributes.
        positionLocation = _lampShader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);


        GL.BindVertexArray(0);
    }

    public void ConfigureLighting(Shader shader, Camera camera, int tamLights)
    {
        shader.Use();

        for (var i = 0; i < tamLights; i++)
        {
            shader.SetVector3($"lights[{i}].position", _pointLightPos[i]);
            shader.SetVector3($"lights[{i}].ambient", new Vector3(0.2f, 0.2f, 0.2f));
            shader.SetVector3($"lights[{i}].diffuse", new Vector3(0.5f, 0.5f, 0.5f));
            shader.SetVector3($"lights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
        }
    }

    public void Render(Camera camera)
    {
        GL.BindVertexArray(_vaoModel);
        _texture.Use(TextureUnit.Texture0);
        _lightingShader.Use();

        _lightingShader.SetMatrix4("model", ModelMatrix);
        _lightingShader.SetMatrix4("view", camera.GetViewMatrix());
        _lightingShader.SetMatrix4("projection", camera.GetProjectionMatrix());

        _lightingShader.SetVector3("viewPos", camera.Position);

        // Here we specify to the shaders what textures they should refer to when we want to get the positions.
        _lightingShader.SetInt("material.diffuse", 0);
        _lightingShader.SetInt("material.specular", 0);
        _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        _lightingShader.SetFloat("material.shininess", 32.0f);

        ConfigureLighting(_lightingShader, camera, 2);

        GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 8);

        GL.BindVertexArray(_vaoLamp);

        _lampShader.Use();

        _lampShader.SetMatrix4("view", camera.GetViewMatrix());
        _lampShader.SetMatrix4("projection", camera.GetProjectionMatrix());

        var lampMatrix = Matrix4.Identity;
        lampMatrix *= Matrix4.CreateScale(5.0f);
        lampMatrix *= Matrix4.CreateTranslation(_pointLightPos[1]);
        _lampShader.SetMatrix4("model", lampMatrix);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);


        var lampMatrix2 = Matrix4.Identity;
        lampMatrix2 *= Matrix4.CreateScale(0.7f);
        lampMatrix2 *= Matrix4.CreateTranslation(_pointLightPos[0]);
        _lampShader.SetMatrix4("model", lampMatrix2);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
    }

    public void Translate(Vector3 translation)
    {
        ModelMatrix *= Matrix4.CreateTranslation(translation);
        //UpdateBoundingBox();
    }

    public void Rotate(float angle, Vector3 axis)
    {
        ModelMatrix *= Matrix4.CreateFromAxisAngle(axis, MathHelper.DegreesToRadians(angle));
        //UpdateBoundingBox();
    }

    public void Scale(Vector3 scale)
    {
        ModelMatrix *= Matrix4.CreateScale(scale);
        _lightingShader.SetMatrix4("model", ModelMatrix);
        //UpdateBoundingBox();
    }
}