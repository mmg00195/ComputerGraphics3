using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ComputerGraphics3
{
    internal class table : furniture
    {
        private Hitbox subHitboxes;
        private Vector3 hitboxTablePos;
        private Vector3 hitboxTableSize;
        

        public table(Shader shader, Texture texture, Vector3 size, Vector3 position) :
            base(shader, texture, size, position)
        {

            // Escalamos por size.X, size.Y, size.Z
            float[] vertices =
            {
                //Table
                // Back face
                -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 0.0f, 1.0f,
                // Front face
                -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 0.0f,
                2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                // Left face
                -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                -2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                // Right face
                2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                // Bottom face
                -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                // Top face
                -2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,

                //Leg left
                // Back face
                -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                -1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                -1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, 1.0f,
                // Front face
                -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                -1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 0.0f,
                -1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                // Left face
                -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                -2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                // Right face
                -1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                -1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                -1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                -1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                // Bottom face
                -2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                -1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                -1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                // Top face
                -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                -1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                -1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,

                //Right left
                // Back face
                2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, 1.0f,
                // Front face
                2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 0.0f,
                1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                // Left face
                2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                // Right face
                1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 1.0f,
                1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f,
                // Bottom face
                2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
                // Top face
                2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f,
                1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f,
                1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 1.0f,
                2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f,
            };



            uint[] indices =
            {
                // Tabla superior
                0, 1, 2, 2, 3, 0, // Back face
                4, 5, 6, 6, 7, 4, // Front face
                8, 9, 10, 10, 11, 8, // Left face
                12, 13, 14, 14, 15, 12, // Right face
                16, 17, 18, 18, 19, 16, // Bottom face
                20, 21, 22, 22, 23, 20, // Top face

                // Leg 1
                24, 25, 26, 26, 27, 24, // Back face
                28, 29, 30, 30, 31, 28, // Front face
                32, 33, 34, 34, 35, 32, // Left face
                36, 37, 38, 38, 39, 36, // Right face
                40, 41, 42, 42, 43, 40, // Bottom face
                44, 45, 46, 46, 47, 44, // Top face

                // Leg 2
                48, 49, 50, 50, 51, 48, // Back face
                52, 53, 54, 54, 55, 52, // Front face
                56, 57, 58, 58, 59, 56, // Left face
                60, 61, 62, 62, 63, 60, // Right face
                64, 65, 66, 66, 67, 64, // Bottom face
                68, 69, 70, 70, 71, 68, // Top face


            };

            _vertices = vertices;
            _indices = indices;

            hitboxTableSize = new Vector3(size.X * 0.7f, size.Y * 3.1f, size.Z * 0.9f);
            hitboxTablePos = new Vector3(0,-1f,0) + position;
            subHitboxes = new Hitbox(hitboxTablePos, hitboxTableSize);

            SetupBuffers();
        }

        public override void RenderHitbox(Camera camera, Shader hitboxShader)
        {
            hitboxShader.Use();
            Matrix4 hitboxMatrix =
                Matrix4.CreateScale(subHitboxes.Size) * Matrix4.CreateTranslation(subHitboxes.Position);
            hitboxShader.SetMatrix4("model", hitboxMatrix);
            hitboxShader.SetMatrix4("view", camera.GetViewMatrix());
            hitboxShader.SetMatrix4("projection", camera.GetProjectionMatrix());

            // Renderiza como un cubo o líneas
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 24);

        }
        public override bool CheckCollision(Hitbox other)
        {
            if (subHitboxes.CheckCollision(other))
            {
                return true; // Hay una colisión
            }

            return false; // No hay colisión
        }

        public override Hitbox getHitbox()
        {
            return subHitboxes;
        }
    }
}
