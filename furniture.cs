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

        protected Vector3 _size; // Tamaño del objeto para colisiones
        protected Vector3 _position; // Posición del objeto

        protected Hitbox hitbox;

        public furniture(Shader shader, Texture texture, Vector3 size, Vector3 position)
        {
            _shader = shader;
            _texture = texture;
            _size = size;
            _position = position;

            hitbox = new Hitbox(position, size);

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
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));


            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(texCoordLocation);

            GL.BindVertexArray(0);
        }

        public virtual void Render(Camera camera, Room room)
        {
            _shader.Use();
            _shader.SetMatrix4("model", _modelMatrix);
            _shader.SetMatrix4("view", camera.GetViewMatrix());
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            _shader.SetVector3("viewPos", camera.Position);

            _texture.Use(TextureUnit.Texture0);

            _shader.SetInt("material.diffuse", 0);
            _shader.SetInt("material.specular", 0);
            _shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _shader.SetFloat("material.shininess", 32.0f);

            room.ConfigureLighting(_shader, camera);

            GL.BindVertexArray(_vao);

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public virtual Hitbox getHitbox()
        {
            return hitbox;
        }

       /* public virtual void UpdateHitbox()
        {
            Vector3 currentPos = _modelMatrix.ExtractTranslation();
            Vector3 currentScale = _modelMatrix.ExtractScale();
            Vector3 scaledSize = Vector3.Multiply(_size, currentScale);

            hitbox.UpdatePosition(scaledSize, currentPos);
        }*/

        public virtual void RenderHitbox(Camera camera, Shader hitboxShader)
        {
            Matrix4 hitboxMatrix = Matrix4.CreateScale(hitbox.Size) * Matrix4.CreateTranslation(hitbox.Position);
            //hitbox.Render(hitboxMatrix, camera, hitboxShader);
        }

        public virtual bool CheckCollision(Hitbox other)
        {
            if (hitbox.CheckCollision(other))
            {
                return true; // Hay una colisión
            }

            return false; // No hay colisión
        }
    }
}
