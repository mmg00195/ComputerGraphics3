using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Vector3 = OpenTK.Mathematics.Vector3;

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
        private bool wireframe;
        private bool gravity;
        private const int GRAVITY_OFFSET = 3000;

        private readonly Vector3 roomMin = new Vector3(-5.0f, -5.0f, -5.0f); // Límite inferior
        private readonly Vector3 roomMax = new Vector3(5.0f, 5.0f, 5.0f);   // Límite superior
        private readonly Vector3 objSize = new Vector3(1.0f, 1.0f, 1.0f);  // Tamaño del cubo


        public Matrix4 ModelMatrix { get; private set; }

        public obj1(Shader shader, int polygonType)
        {
            float[] vertices = new float[]{};
            uint[] indices = new uint[]{};
            if (polygonType == 1)
            {
                //Cube
                vertices = new float[]
                {
                    // Positions          // Texture coordinates
                    -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, // Back face
                    0.5f, -0.5f, -0.5f, 1.0f, 0.0f,
                    0.5f, 0.5f, -0.5f, 1.0f, 1.0f,
                    -0.5f, 0.5f, -0.5f, 0.0f, 1.0f,
                    -0.5f, -0.5f, 0.5f, 0.0f, 0.0f, // Front face
                    0.5f, -0.5f, 0.5f, 1.0f, 0.0f,
                    0.5f, 0.5f, 0.5f, 1.0f, 1.0f,
                    -0.5f, 0.5f, 0.5f, 0.0f, 1.0f
                };

                indices = new uint[]
                {
                    0, 1, 2, 2, 3, 0, // Back face
                    4, 5, 6, 6, 7, 4, // Front face
                    0, 1, 5, 5, 4, 0, // Bottom face
                    2, 3, 7, 7, 6, 2, // Top face
                    0, 3, 7, 7, 4, 0, // Left face
                    1, 2, 6, 6, 5, 1 // Right face
                };
            }
            else
            {
                if (polygonType == 2) {
                    // Piramid
                    vertices = new float[]
                    {
                    // Positions         // Texture coordinates
                    -0.5f, 0.0f, -0.5f, 0.0f, 0.0f, // Base - V0
                    0.5f, 0.0f, -0.5f, 1.0f, 0.0f, // Base - V1
                    0.5f, 0.0f, 0.5f, 1.0f, 1.0f, // Base - V2
                    -0.5f, 0.0f, 0.5f, 0.0f, 1.0f, // Base - V3
                    0.0f, 1.0f, 0.0f, 0.5f, 0.5f // Ápice - V4
                    };

                    // Índices para formar los triángulos
                    indices = new uint[]
                    {
                    // Base cuadrada (dos triángulos)
                    0, 1, 2,
                    2, 3, 0,

                    // Caras laterales (cada triángulo conecta el ápice con dos vértices de la base)
                    0, 1, 4,
                    1, 2, 4,
                    2, 3, 4,
                    3, 0, 4
                    };
                }
            } 

            _vertices = vertices;
            _indices = indices;
            _shader = shader;
            rando = new Randomizer();
            color = rando.RandomColor();
            visibility = true;
            wireframe = false;
            gravity = false;

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

            if (wireframe)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            else
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            if (visibility)
            {
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
            }
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        public void Translate(Vector3 translation)
        {
            ModelMatrix *= Matrix4.CreateTranslation(translation);
            
        }

        public void Rotate(float angle, Vector3 axis)//?????????
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

        public void ToggleWireframeMode()
        {
            wireframe = !wireframe;
        }

        public void ToggleGravity()
        {
            gravity = !gravity;
        }
        public void Update(float deltaTime)
        {
            if (gravity)
            {
                Translate((Vector3.Zero + new Vector3(0, -GRAVITY_OFFSET, 0) * deltaTime) * deltaTime);
            }

            var position = ModelMatrix.ExtractTranslation();

            // Colisión en el eje X
            if (position.X - objSize.X / 2 < roomMin.X || position.X + objSize.X / 2 > roomMax.X)
            {
                position.X = Math.Clamp(position.X, roomMin.X + objSize.X / 2, roomMax.X - objSize.X / 2);
            }

            // Colisión en el eje Y
            if (position.Y - objSize.Y / 2 < roomMin.Y || position.Y + objSize.Y / 2 > roomMax.Y)
            {
                position.Y = Math.Clamp(position.Y, roomMin.Y + objSize.Y / 2, roomMax.Y - objSize.Y / 2);
            }

            // Colisión en el eje Z
            if (position.Z - objSize.Z / 2 < roomMin.Z || position.Z + objSize.Z / 2 > roomMax.Z)
            {
                position.Z = Math.Clamp(position.Z, roomMin.Z + objSize.Z / 2, roomMax.Z - objSize.Z / 2);
            }

            // Actualizar la posición del cubo en el modelo
            ModelMatrix = Matrix4.CreateTranslation(position);
           

        }

    }
}