using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace ComputerGraphics3
{
    public class Hitbox
    {
        public Vector3 Position { get; private set; } // Centro de la hitbox
        public Vector3 Size { get; private set; }     // Tamaño (ancho, alto, profundidad)

        public Hitbox(Vector3 position, Vector3 size)
        {
            Position = position;
            Size = size;
        }

        public void UpdatePosition(Vector3 newSize, Vector3 newPosition)
        {
            Size = newSize;
            Position = newPosition;
        }

        // Verificar colisión con otra hitbox
        public bool CheckCollision(Hitbox other)
        {
            if (Position.X - Size.X / 2 <= other.Position.X + other.Size.X / 2 &&
                Position.X + Size.X / 2 >= other.Position.X - other.Size.X / 2 &&
                Position.Y - Size.Y / 2 <= other.Position.Y + other.Size.Y / 2 &&
                Position.Y + Size.Y / 2 >= other.Position.Y - other.Size.Y / 2 &&
                Position.Z - Size.Z / 2 <= other.Position.Z + other.Size.Z / 2 &&
                Position.Z + Size.Z / 2 >= other.Position.Z - other.Size.Z / 2)
                return true;
            return false;


            /*return
                Position.X - Size.X / 2 <= other.Position.X + other.Size.X / 2 &&
                Position.X + Size.X / 2 >= other.Position.X - other.Size.X / 2 &&
                Position.Y - Size.Y / 2 <= other.Position.Y + other.Size.Y / 2 &&
                Position.Y + Size.Y / 2 >= other.Position.Y - other.Size.Y / 2 &&
                Position.Z - Size.Z / 2 <= other.Position.Z + other.Size.Z / 2 &&
                Position.Z + Size.Z / 2 >= other.Position.Z - other.Size.Z / 2;*/
        }

    }

}
