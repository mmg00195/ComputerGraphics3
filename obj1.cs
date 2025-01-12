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
        //private Color color;
        private Vector3 lightColor;
        private Randomizer rando;
        private bool visibility;
        private bool wireframe;
        private bool gravity;
        private const int GRAVITY_OFFSET = 3000;
        private int sectorCount = 36;
        private int stackCount = 18;

        private readonly Vector3 roomMin = new Vector3(-5.0f, -5.0f, -5.0f); // inferior limit
        private readonly Vector3 roomMax = new Vector3(5.0f, 5.0f, 5.0f);   // superior limit
        private readonly Vector3 objSize = new Vector3(1.0f, 1.0f, 1.0f);  // obj size

        protected Hitbox hitbox;

        public Matrix4 ModelMatrix { get; private set; }

        public obj1(Shader shader, int polygonType)
        {
            float[] vertices = new float[]{};
            uint[] indices = new uint[]{};
            switch (polygonType)
            {
                case 1:
                    vertices = new float[]
                    {
                        // Positions          // Normals
                        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, 1.0f, // Back face
                         0.5f, -0.5f, -0.5f,  0.0f,  0.0f, 1.0f,
                         0.5f,  0.5f, -0.5f,  0.0f,  0.0f, 1.0f,
                        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, 1.0f,

                        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  -1.0f, // Front face
                         0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  -1.0f,
                         0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  -1.0f,
                        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  -1.0f,

                        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f, // Left face
                        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
                        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

                         0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f, // Right face
                         0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                         0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
                         0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

                        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Bottom face
                         0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
                         0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,

                        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Top face
                         0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
                         0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f
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
                    break;
                case 2:
                    // Piramid
                    vertices = new float[]
                    {
                        // Positions         // Normals         // Texture coordinates
                        -0.5f, 0.0f, -0.5f,  0.0f, -1.0f,  0.0f, // Base - V0
                        0.5f, 0.0f, -0.5f,  0.0f, -1.0f,  0.0f, // Base - V1
                        0.5f, 0.0f,  0.5f,  0.0f, -1.0f,  0.0f, // Base - V2
                        -0.5f, 0.0f,  0.5f,  0.0f, -1.0f,  0.0f, // Base - V3
                        0.0f, 1.0f,  0.0f,  0.0f,  1.0f,  0.0f  // Ápice - V4
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
                    break;
                case 3:
                    (vertices, indices) = GenerateSphere(sectorCount, stackCount);
                    break;
            }

            _vertices = vertices;
            _indices = indices;
            _shader = shader;
            rando = new Randomizer();
            //color = rando.RandomColor();
            lightColor = SetLightColor();
            visibility = true;
            wireframe = false;
            gravity = false;

            ModelMatrix = Matrix4.Identity;
            hitbox = new Hitbox(Vector3.Zero, objSize);

            SetupBuffers();
        }

        private (float[] vertices, uint[] indices)  GenerateSphere(int sectorCount, int stackCount)
        {
            float radius = 0.5f;
            List<float> verticesSp = new List<float>();
            List<uint> indicesSp = new List<uint>();

            float x, y, z, nx, ny, nz, xy;
            float sectorStep = 2 * MathF.PI / sectorCount;
            float stackStep = MathF.PI / stackCount;
            float sectorAngle, stackAngle;

            // Generar vértices
            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = MathF.PI / 2 - i * stackStep;
                xy = radius * MathF.Cos(stackAngle);
                z = radius * MathF.Sin(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    // Coordenadas de vértices
                    x = xy * MathF.Cos(sectorAngle);
                    y = xy * MathF.Sin(sectorAngle);
                    nx = x / radius;
                    ny = y / radius;
                    nz = z / radius;

                    verticesSp.Add(x);
                    verticesSp.Add(y);
                    verticesSp.Add(z);

                    // Norm
                    verticesSp.Add(nx);
                    verticesSp.Add(ny);
                    verticesSp.Add(nz);
                }
            }

            // Generar índices
            for (int i = 0; i < stackCount; ++i)
            {
                int k1 = i * (sectorCount + 1);
                int k2 = k1 + sectorCount + 1;

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indicesSp.Add((uint)k1);
                        indicesSp.Add((uint)k2);
                        indicesSp.Add((uint)(k1 + 1));
                    }

                    if (i != (stackCount - 1))
                    {
                        indicesSp.Add((uint)(k1 + 1));
                        indicesSp.Add((uint)k2);
                        indicesSp.Add((uint)(k2 + 1));
                    }
                }
            }


            return (verticesSp.ToArray(), indicesSp.ToArray());

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
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(positionLocation);

            var normalLocation = _shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

            GL.BindVertexArray(0);
        }

        public void Render(Camera camera, Room room)
        {
            _shader.Use();
            //_shader.SetColor("objectColor",color);

            //_texture.Use(TextureUnit.Texture0);

            _shader.SetMatrix4("model", ModelMatrix);
            _shader.SetMatrix4("view", camera.GetViewMatrix());
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            _shader.SetVector3("viewPos", camera.Position);

            _shader.SetInt("material.diffuse", 0);
            _shader.SetInt("material.specular", 0);
            _shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _shader.SetFloat("material.shininess", 32.0f);
            
            // The ambient light is less intensive than the diffuse light in order to make it less dominant
            Vector3 ambientColor = lightColor * new Vector3(0.2f);
            Vector3 diffuseColor = lightColor * new Vector3(0.5f);

            _shader.SetVector3("lights[0].position", room._pointLightPos[0]);
            _shader.SetVector3("lights[0].ambient", ambientColor);
            _shader.SetVector3("lights[0].diffuse", diffuseColor);
            _shader.SetVector3("lights[0].specular", new Vector3(1.0f, 1.0f, 1.0f));


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
            UpdateHitbox();
        }

        public void Rotate(float angle, Vector3 axis)//?????????
        {
            ModelMatrix *= Matrix4.CreateFromAxisAngle(axis, MathHelper.DegreesToRadians(angle));
        }

        public void Scale(Vector3 scale)
        {
            ModelMatrix *= Matrix4.CreateScale(scale);
        }

        public Vector3 SetLightColor()
        {
            Color color = rando.RandomColor();
            lightColor = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
            return lightColor;
        }
        public void ToggleColor()
        {
            Color color = rando.RandomColor();
            lightColor = new Vector3(color.R / 255f, color.G / 255f, color.B / 255f);
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
            // Colision furniture
            /*if (position.Z - objSize.Z / 2 < roomMax.Z - 5 && position.Y - objSize.Y / 2 < roomMin.Y - 5)
            {
                position.Z = Math.Clamp(position.Z,5 + roomMin.Z + objSize.Z / 2, roomMax.Z - objSize.Z / 2);
                position.Y = Math.Clamp(position.Y,5 + roomMin.Y + objSize.Y / 2, roomMax.Y - objSize.Y / 2);
            }*/
            // Actualizar la posición del cubo en el modelo
            ModelMatrix = Matrix4.CreateTranslation(position);
            UpdateHitbox();
            
        }
        public void DiscoMode()
        {
            ToggleColor();
        }

        public Hitbox getHitbox()
        {
            return hitbox;
        }

        public void UpdateHitbox()
        {
            Vector3 currentPos = ModelMatrix.ExtractTranslation();
            Vector3 currentScale = ModelMatrix.ExtractScale();
            Vector3 scaledSize = Vector3.Multiply(objSize, currentScale);
            
            hitbox.UpdatePosition(scaledSize, currentPos);
        }
        public void RenderHitbox(Camera camera, Shader hitboxShader)
        {
            hitboxShader.Use();
            Matrix4 hitboxMatrix =
                Matrix4.CreateScale(hitbox.Size) * Matrix4.CreateTranslation(hitbox.Position);
            hitboxShader.SetMatrix4("model", hitboxMatrix);
            hitboxShader.SetMatrix4("view", camera.GetViewMatrix());
            hitboxShader.SetMatrix4("projection", camera.GetProjectionMatrix());

            // Renderiza como un cubo o líneas
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 24);

        }
    }
}