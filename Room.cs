using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics3
{
    class Room
    {

        private readonly float[] _vertices = {
            // Positions          // Texture coordinates
            -0.5f, -0.5f, -0.5f,  0.0f, 0.0f, // Back face
             0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, 0.0f, // Front face
             0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f, 1.0f
        };

        private readonly uint[] _indices = {
            0, 1, 2, 2, 3, 0, // Back face
            4, 5, 6, 6, 7, 4, // Front face
            0, 1, 5, 5, 4, 0, // Bottom face
            2, 3, 7, 7, 6, 2, // Top face
            0, 3, 7, 7, 4, 0, // Left face
            1, 2, 6, 6, 5, 1  // Right face
        };

        private int _vao, _vbo, _ebo;
        private Shader _shader;
        private Texture _texture;

        public Matrix4 ModelMatrix { get; private set; }

        public Room(Shader shader, Texture texture)
        {
            _shader = shader;
            _texture = texture;

            ModelMatrix = Matrix4.Identity;

            SetupBuffers();
        }

        private void SetupBuffers()
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            var positionLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(texCoordLocation);

            GL.BindVertexArray(0);
        }

        public void Render(Camera camera)
        {
            _shader.Use();
            _texture.Use(TextureUnit.Texture0);

            _shader.SetMatrix4("model", ModelMatrix);
            _shader.SetMatrix4("view", camera.GetViewMatrix());
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Translate(Vector3 translation)
        {
            ModelMatrix *= Matrix4.CreateTranslation(translation);
        }

        public void Rotate(float angle, Vector3 axis)
        {
            ModelMatrix *= Matrix4.CreateFromAxisAngle(axis, MathHelper.DegreesToRadians(angle));
        }

        public void Scale(Vector3 scale)
        {
            ModelMatrix *= Matrix4.CreateScale(scale);
        }
    }
}
