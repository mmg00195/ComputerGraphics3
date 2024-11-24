using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace ComputerGraphics3
{
    public class BoundingBox
    {
        public Vector3 Min { get; private set; }
        public Vector3 Max { get; private set; }

        public BoundingBox(Vector3 position, Vector3 scale)
        {
            Update(position, scale);
        }

        public void Update(Vector3 position, Vector3 scale)
        {
            Min = position - scale / 2.0f;
            Max = position + scale / 2.0f;
        }

        public bool Intersects(BoundingBox other)
        {
            return (Min.X <= other.Max.X && Max.X >= other.Min.X) &&
                   (Min.Y <= other.Max.Y && Max.Y >= other.Min.Y) &&
                   (Min.Z <= other.Max.Z && Max.Z >= other.Min.Z);
        }
    }

}
