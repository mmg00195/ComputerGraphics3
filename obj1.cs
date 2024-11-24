using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics3
{
    class obj1
    {
        private readonly float[] _vertices;
        private readonly uint[] _indices;

        private int _vao, _vbo, _ebo;
        private Shader _shader;
        private Color color;
        private Randomizer rando;
        private bool visibility;

        public Matrix4 ModelMatrix { get; private set; }

        public obj1(Shader shader)
        {
            float[] vertices = {
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

            uint[] indices = {
                0, 1, 2, 2, 3, 0, // Back face
                4, 5, 6, 6, 7, 4, // Front face
                0, 1, 5, 5, 4, 0, // Bottom face
                2, 3, 7, 7, 6, 2, // Top face
                0, 3, 7, 7, 4, 0, // Left face
                1, 2, 6, 6, 5, 1  // Right face
            };
            _vertices = vertices;
            _indices = indices;
            _shader = shader;
            rando = new Randomizer();
            color = rando.RandomColor();
            visibility = true;

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
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
                BufferUsageHint.StaticDraw);

            var positionLocation = _shader.GetAttribLocation("aPosition");
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float),
                3 * sizeof(float));
            GL.EnableVertexAttribArray(texCoordLocation);

            GL.BindVertexArray(0);
        }

        public void Render(Camera camera)
        {
            _shader.Use();
            _shader.SetColor("objectColor",color);

            //_texture.Use(TextureUnit.Texture0);

            _shader.SetMatrix4("model", ModelMatrix);
            _shader.SetMatrix4("view", camera.GetViewMatrix());
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.BindVertexArray(_vao);
            if(visibility)
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

        public void ToggleColor()
        {
            this.color = rando.RandomColor();
        }

        public void ToggleVisibility()
        {
            visibility = !visibility;
        }
    }
}