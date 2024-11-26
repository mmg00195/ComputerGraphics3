using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace ComputerGraphics3
{
    internal class table:furniture
    {
        
        // Inicializar vértices e índices (debe ser implementado por las subclases)
        float legWidth; // Ancho de las patas
        float legHeight; // Altura de las patas (mitad del total)

        // Posiciones relativas de las patas
        float halfWidth;
        float halfDepth;
        float halfHeight;
        float thickness;

        public table(Shader shader, Texture texture, Vector3 size, Vector3 position):
            base(shader, texture,size,position)
        {
            legWidth = 0.1f; // Ancho de las patas
            legHeight = size.Y / 2; // Altura de las patas (mitad del total)
            thickness = 0.5f;

            // Posiciones relativas de las patas
            halfWidth = size.X / 2;
            halfDepth = size.Z / 2;
            halfHeight = size.Y / 2;

            // Vértices de la mesa (tablero superior y patas)
            float[] vertices =
            {
                // Tabla superior (cubo)
                -halfWidth,  halfHeight, -halfDepth, 0.0f, 0.0f, // V0
                 halfWidth,  halfHeight, -halfDepth, 1.0f, 0.0f, // V1
                 halfWidth,  halfHeight,  halfDepth, 1.0f, 1.0f, // V2
                -halfWidth,  halfHeight,  halfDepth, 0.0f, 1.0f, // V3
                -halfWidth,  halfHeight - thickness, -halfDepth, 0.0f, 0.0f, // V4
                 halfWidth,  halfHeight - thickness, -halfDepth, 1.0f, 0.0f, // V5
                 halfWidth,  halfHeight - thickness,  halfDepth, 1.0f, 1.0f, // V6
                -halfWidth,  halfHeight - thickness,  halfDepth, 0.0f, 1.0f, // V7

                // Pata 1 (esquina frontal izquierda, cubo)
                -halfWidth + legWidth, -legHeight, -halfDepth + legWidth, 0.0f, 0.0f, // V8
                -halfWidth + legWidth, -legHeight, -halfDepth, 1.0f, 0.0f, // V9
                -halfWidth + legWidth, legHeight - thickness, -halfDepth, 1.0f, 1.0f, // V10
                -halfWidth + legWidth, legHeight - thickness, -halfDepth + legWidth, 0.0f, 1.0f, // V11
                -halfWidth, -legHeight, -halfDepth + legWidth, 0.0f, 0.0f, // V12
                -halfWidth, -legHeight, -halfDepth, 1.0f, 0.0f, // V13
                -halfWidth, legHeight - thickness, -halfDepth, 1.0f, 1.0f, // V14
                -halfWidth, legHeight - thickness, -halfDepth + legWidth, 0.0f, 1.0f, // V15

                // Pata 2 (esquina frontal derecha, cubo)
                 halfWidth - legWidth, -legHeight, -halfDepth + legWidth, 0.0f, 0.0f, // V16
                 halfWidth - legWidth, -legHeight, -halfDepth, 1.0f, 0.0f, // V17
                 halfWidth - legWidth, legHeight - thickness, -halfDepth, 1.0f, 1.0f, // V18
                 halfWidth - legWidth, legHeight - thickness, -halfDepth + legWidth, 0.0f, 1.0f, // V19
                 halfWidth, -legHeight, -halfDepth + legWidth, 0.0f, 0.0f, // V20
                 halfWidth, -legHeight, -halfDepth, 1.0f, 0.0f, // V21
                 halfWidth, legHeight - thickness, -halfDepth, 1.0f, 1.0f, // V22
                 halfWidth, legHeight - thickness, -halfDepth + legWidth, 0.0f, 1.0f, // V23

                // Pata 3 (esquina trasera izquierda, cubo)
                -halfWidth + legWidth, -legHeight,  halfDepth - legWidth, 0.0f, 0.0f, // V24
                -halfWidth + legWidth, -legHeight,  halfDepth, 1.0f, 0.0f, // V25
                -halfWidth + legWidth, legHeight - thickness,  halfDepth, 1.0f, 1.0f, // V26
                -halfWidth + legWidth, legHeight - thickness,  halfDepth - legWidth, 0.0f, 1.0f, // V27
                -halfWidth, -legHeight,  halfDepth - legWidth, 0.0f, 0.0f, // V28
                -halfWidth, -legHeight,  halfDepth, 1.0f, 0.0f, // V29
                -halfWidth, legHeight - thickness,  halfDepth, 1.0f, 1.0f, // V30
                -halfWidth, legHeight - thickness,  halfDepth - legWidth, 0.0f, 1.0f, // V31

                // Pata 4 (esquina trasera derecha, cubo)
                 halfWidth - legWidth, -legHeight,  halfDepth - legWidth, 0.0f, 0.0f, // V32
                 halfWidth - legWidth, -legHeight,  halfDepth, 1.0f, 0.0f, // V33
                 halfWidth - legWidth, legHeight - thickness,  halfDepth, 1.0f, 1.0f, // V34
                 halfWidth - legWidth, legHeight - thickness,  halfDepth - legWidth, 0.0f, 1.0f, // V35
                 halfWidth, -legHeight,  halfDepth - legWidth, 0.0f, 0.0f, // V36
                 halfWidth, -legHeight,  halfDepth, 1.0f, 0.0f, // V37
                 halfWidth, legHeight - thickness,  halfDepth, 1.0f, 1.0f, // V38
                 halfWidth, legHeight - thickness,  halfDepth - legWidth, 0.0f, 1.0f, // V39
            };

            uint[] indices =
            {
                // Tabla superior (rellena)
                0, 1, 2, 2, 3, 0, // Cara superior
                4, 5, 6, 6, 7, 4, // Cara inferior
                0, 1, 5, 5, 4, 0, // Cara lateral 1
                1, 2, 6, 6, 5, 1, // Cara lateral 2
                2, 3, 7, 7, 6, 2, // Cara lateral 3
                3, 0, 4, 4, 7, 3, // Cara lateral 4

                // Pata 1
                8, 9, 10, 10, 11, 8, // Cara frontal
                12, 13, 14, 14, 15, 12, // Cara trasera
                8, 9, 13, 13, 12, 8, // Cara izquierda
                10, 11, 15, 15, 14, 10, // Cara derecha
                8, 12, 15, 15, 11, 8, // Cara superior
                9, 13, 14, 14, 10, 9, // Cara inferior

                // Pata 2
                16, 17, 18, 18, 19, 16, // Cara frontal
                20, 21, 22, 22, 23, 20, // Cara trasera
                16, 17, 21, 21, 20, 16, // Cara izquierda
                18, 19, 23, 23, 22, 18, // Cara derecha
                16, 20, 23, 23, 19, 16, // Cara superior
                17, 21, 22, 22, 18, 17, // Cara inferior

                // Pata 3
                24, 25, 26, 26, 27, 24, // Cara frontal
                28, 29, 30, 30, 31, 28, // Cara trasera
                24, 25, 29, 29, 28, 24, // Cara izquierda
                26, 27, 31, 31, 30, 26, // Cara derecha
                24, 28, 31, 31, 27, 24, // Cara superior
                25, 29, 30, 30, 26, 25, // Cara inferior

                // Pata 4
                32, 33, 34, 34, 35, 32, // Cara frontal
                36, 37, 38, 38, 39, 36, // Cara trasera
                32, 33, 37, 37, 36, 32, // Cara izquierda
                34, 35, 39, 39, 38, 34, // Cara derecha
                32, 36, 39, 39, 35, 32, // Cara superior
                33, 37, 38, 38, 34, 33  // Cara inferior 

            };
            _vertices = vertices;
            _indices = indices;

            SetupBuffers();
        }
    }
}
