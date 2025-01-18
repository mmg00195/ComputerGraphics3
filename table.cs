using OpenTK.Mathematics;

namespace ComputerGraphics3;

internal class table : furniture
{
    public table(Shader shader, int i, List<Texture> texture, Vector3 size, Vector3 position) :
        base(shader, i, texture, size, position)
    {
        // size.X, size.Y, size.Z
        float[] vertices =
        {
            // Tabletop
            // Back face
            -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
            2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
            2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
            -2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,

            // Front face
            -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
            -2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,

            // Left face
            -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            -2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            -2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,

            // Right face
            2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,

            // Bottom face
            -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
            2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
            2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
            -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,

            // Top face
            -2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
            2.0f * size.X, 0.5f * size.Y, -1.0f * size.Z, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
            2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            -2.0f * size.X, 0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,

            //Leg left
            // Back face
            -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
            -1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
            -1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
            -2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
            // Front face
            -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            -1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            -1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
            -2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
            // Left face
            -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            -2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            -2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            // Right face
            -1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            -1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            -1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            -1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            // Bottom face
            -2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
            -1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
            -1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
            -2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
            // Top face
            -2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
            -1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
            -1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            -2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,

            //Right leg
            // Back face
            2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
            1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
            1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
            2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
            // Front face
            2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
            1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
            1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
            2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
            // Left face
            2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            // Right face
            1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
            1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
            1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
            // Bottom face
            2.0f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
            1.7f * size.X, -2.9f * size.Y, -1.0f * size.Z, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
            1.7f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
            2.0f * size.X, -2.9f * size.Y, 1.0f * size.Z, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
            // Top face
            2.0f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
            1.7f * size.X, -0.5f * size.Y, -1.0f * size.Z, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
            1.7f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            2.0f * size.X, -0.5f * size.Y, 1.0f * size.Z, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f
        };


        uint[] indices =
        {
            // sup table
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
            68, 69, 70, 70, 71, 68 // Top face
        };

        _vertices = vertices;
        _indices = indices;

        SetupBuffers();
    }
}