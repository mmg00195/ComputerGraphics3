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
        private Vector3 gravityDirection;
        private const int GRAVITY_OFFSET = 3000;
        private int sectorCount = 36;
        private int stackCount = 18;

        private readonly Vector3 roomMin = new Vector3(-5.0f, -5.0f, -5.0f); // inferior limit
        private readonly Vector3 roomMax = new Vector3(5.0f, 5.0f, 5.0f);   // superior limit
        private readonly Vector3 objSize = new Vector3(1.0f, 1.0f, 1.0f);  // obj size

        public Matrix4 ModelMatrix { get; private set; }

        public obj1(Shader shader, int polygonType)
        {
            float[] vertices = new float[]{};
            switch (polygonType)
            {
                case 1:
                    vertices = new float[]
                    {
                        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, // Front face
                        0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                        0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                        0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                        -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
                        -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

                        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, // Back face
                        0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                        0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                        0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                        -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
                        -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

                        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f, // Left face
                        -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                        -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
                        -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
                        -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

                        0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, // Right face
                        0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                        0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                        0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
                        0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
                        0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

                        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f, // Bottom face
                        0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
                        0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                        0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                        -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
                        -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,

                        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, // Top face
                        0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
                        0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                        0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                        -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
                        -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
                    };

                    break;
                case 2:
                    // Piramid
                    vertices = new float[]
                    {
                        // Base
                        -0.5f, 0.0f, -0.5f,  0.0f, -1.0f,  0.0f, // V0
                        0.5f, 0.0f, -0.5f,  0.0f, -1.0f,  0.0f, // V1
                        0.5f, 0.0f,  0.5f,  0.0f, -1.0f,  0.0f,
                        0.5f, 0.0f,  0.5f,  0.0f, -1.0f,  0.0f,// V2
                        -0.5f, 0.0f,  0.5f,  0.0f, -1.0f,  0.0f,
                        -0.5f, 0.0f, -0.5f,  0.0f, -1.0f,  0.0f, // V3

                        // Cara lateral 1
                        -0.5f, 0.0f, -0.5f, -0.707f,  0.5f, -0.707f, // V0
                        0.5f, 0.0f, -0.5f, -0.707f,  0.5f, -0.707f, // V1
                        0.0f, 1.0f,  0.0f, -0.707f,  0.5f, -0.707f, // V4

                        // Cara lateral 2
                        0.5f, 0.0f, -0.5f,  0.707f,  0.5f, -0.707f, // V1
                        0.5f, 0.0f,  0.5f,  0.707f,  0.5f, -0.707f, // V2
                        0.0f, 1.0f,  0.0f,  0.707f,  0.5f, -0.707f, // V4

                        // Cara lateral 3
                        0.5f, 0.0f,  0.5f,  0.707f,  0.5f,  0.707f, // V2
                        -0.5f, 0.0f,  0.5f,  0.707f,  0.5f,  0.707f, // V3
                        0.0f, 1.0f,  0.0f,  0.707f,  0.5f,  0.707f, // V4

                        // Cara lateral 4
                        -0.5f, 0.0f,  0.5f, -0.707f,  0.5f,  0.707f, // V3
                        -0.5f, 0.0f, -0.5f, -0.707f,  0.5f,  0.707f, // V0
                        0.0f, 1.0f,  0.0f, -0.707f,  0.5f,  0.707f  // V4
                    };

                    break;
                case 3:
                    vertices = GenerateSphere(sectorCount, stackCount);
                    break;
            }

            _vertices = vertices;
            _shader = shader;
            rando = new Randomizer();
            //color = rando.RandomColor();
            lightColor = SetLightColor();
            visibility = true;
            wireframe = false;
            gravity = false;
            gravityDirection = Vector3.Zero;

            ModelMatrix = Matrix4.Identity;

            SetupBuffers();
        }

        private float[] GenerateSphere(int sectorCount, int stackCount)
        {
            float radius = 0.5f;
            List<float> vertices = new List<float>();

            float x, y, nx, ny, nz;
            float sectorStep = 2 * MathF.PI / sectorCount;
            float stackStep = MathF.PI / stackCount;

            // Generar vértices y normales para cada triángulo
            for (int i = 0; i < stackCount; ++i)
            {
                float stackAngle1 = MathF.PI / 2 - i * stackStep;         // Ángulo para la primera fila
                float stackAngle2 = MathF.PI / 2 - (i + 1) * stackStep;   // Ángulo para la segunda fila

                float xy1 = radius * MathF.Cos(stackAngle1); // Radio en el plano XY para la fila 1
                float z1 = radius * MathF.Sin(stackAngle1);  // Z para la fila 1

                float xy2 = radius * MathF.Cos(stackAngle2); // Radio en el plano XY para la fila 2
                float z2 = radius * MathF.Sin(stackAngle2);  // Z para la fila 2

                for (int j = 0; j < sectorCount; ++j)
                {
                    float sectorAngle1 = j * sectorStep;           // Ángulo para el sector 1
                    float sectorAngle2 = (j + 1) * sectorStep;     // Ángulo para el sector 2

                    // Vértices del primer triángulo
                    x = xy1 * MathF.Cos(sectorAngle1);
                    y = xy1 * MathF.Sin(sectorAngle1);
                    nx = x / radius;
                    ny = y / radius;
                    nz = z1 / radius;
                    vertices.AddRange(new float[] { x, y, z1, nx, ny, nz });

                    x = xy2 * MathF.Cos(sectorAngle1);
                    y = xy2 * MathF.Sin(sectorAngle1);
                    nx = x / radius;
                    ny = y / radius;
                    nz = z2 / radius;
                    vertices.AddRange(new float[] { x, y, z2, nx, ny, nz });

                    x = xy2 * MathF.Cos(sectorAngle2);
                    y = xy2 * MathF.Sin(sectorAngle2);
                    nx = x / radius;
                    ny = y / radius;
                    nz = z2 / radius;
                    vertices.AddRange(new float[] { x, y, z2, nx, ny, nz });

                    // Vértices del segundo triángulo
                    x = xy1 * MathF.Cos(sectorAngle1);
                    y = xy1 * MathF.Sin(sectorAngle1);
                    nx = x / radius;
                    ny = y / radius;
                    nz = z1 / radius;
                    vertices.AddRange(new float[] { x, y, z1, nx, ny, nz });

                    x = xy2 * MathF.Cos(sectorAngle2);
                    y = xy2 * MathF.Sin(sectorAngle2);
                    nx = x / radius;
                    ny = y / radius;
                    nz = z2 / radius;
                    vertices.AddRange(new float[] { x, y, z2, nx, ny, nz });

                    x = xy1 * MathF.Cos(sectorAngle2);
                    y = xy1 * MathF.Sin(sectorAngle2);
                    nx = x / radius;
                    ny = y / radius;
                    nz = z1 / radius;
                    vertices.AddRange(new float[] { x, y, z1, nx, ny, nz });
                }
            }

            return vertices.ToArray();
        }


        private void SetupBuffers()
        {
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();

            GL.BindVertexArray(_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
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

            for (int i = 0; i < 2; i++)
            {
                _shader.SetVector3($"lights[{i}].position", room._pointLightPos[i]);
                _shader.SetVector3($"lights[{i}].ambient", ambientColor);
                _shader.SetVector3($"lights[{i}].diffuse", diffuseColor);
                _shader.SetVector3($"lights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
            }
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
                GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 6);
            }
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.BindVertexArray(0);

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
            gravityDirection = new Vector3(0, -GRAVITY_OFFSET, 0);
        }

        public void GlobalGravity()
        {
            // Gravedad en el eje -Y
            gravityDirection = new Vector3(0, -GRAVITY_OFFSET, 0); // Hacia abajo en Y
            gravity = true;
        }
        public void GlobalGravityInv()
        {
            // Gravedad en el eje Y
            gravityDirection = new Vector3(0, GRAVITY_OFFSET, 0); // Hacia arriba en Y
            gravity = true;
        }
        public void GlobalGravityLeft()
        {
            // Gravedad en el eje Y
            gravityDirection = new Vector3(-GRAVITY_OFFSET, 0, 0); // Hacia eje -X
            gravity = true;
        }
        public void GlobalGravityRight()
        {
            // Gravedad en el eje Y
            gravityDirection = new Vector3(GRAVITY_OFFSET, 0, 0); // Hacia eje X
            gravity = true;
        }

        public void ResetGravity()
        {
            ToggleGravity();
        }
        public void Update(float deltaTime)
        {
            if (gravity)
            {
                Translate((Vector3.Zero + gravityDirection * deltaTime) * deltaTime);
            }

            var position = ModelMatrix.ExtractTranslation();
            var lastposition = position;
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

            // Colision table
            if (position.Z + objSize.Z / 2 < roomMin.Z + 3.6 && position.Y + objSize.Y / 2 < roomMin.Y + 4.38) {
                // Bloquear primero en el eje Z si intenta cruzar el límite
                if (lastposition.Z > roomMin.Z + 3.6)
                {
                    position.Z = roomMin.Z + 3.6f - objSize.Z / 2;
                }
                // Si no cruza en Z, verifica y bloquea en Y
                else
                {
                    position.Y = roomMin.Y + 4.38f - objSize.Y / 2;
                }
            }

            // Actualizar la posición del cubo en el modelo
            ModelMatrix = Matrix4.CreateTranslation(position);
        }
        public void DiscoMode()
        {
            ToggleColor();
        }
    }
}