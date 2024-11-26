using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ComputerGraphics3
{
    internal class furniture
    {
        protected float[] _vertices;
        protected uint[] _indices;

        protected int _vao, _vbo, _ebo;
        protected Shader _shader;
        protected Texture _texture;
        protected Matrix4 _modelMatrix;

        protected readonly Vector3 _size; // Tamaño del objeto para colisiones
        protected Vector3 _position; // Posición del objeto
 
        

        public furniture(Shader shader, Texture texture, Vector3 size, Vector3 position)
        {
            _shader = shader;
            _texture = texture;
            _size = size;
            _position = position;

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
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);

            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(texCoordLocation);

            GL.BindVertexArray(0);
        }

        public virtual void Render(Camera camera)
        {
            _shader.Use();
            _shader.SetMatrix4("model", _modelMatrix);
            _shader.SetMatrix4("view", camera.GetViewMatrix());
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            _texture.Use(TextureUnit.Texture0);

            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

       
    }
}
