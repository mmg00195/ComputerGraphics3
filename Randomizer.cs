﻿using System.Drawing;
using OpenTK.Mathematics;

namespace ComputerGraphics3
{
    internal class Randomizer
    {
        private Random r;
        private const int LOW_INT_VAL = -25;
        private const int HIGH_INT_VAL = 25;
        private const int LOW_COORD_VAL = 50;
        private const int HIGH_COORD_VAL = 50;

        public Randomizer()
        {
            r = new Random();
        }

        public Color RandomColor()
        {
            int genR = r.Next(0, 255);
            int genG = r.Next(0, 255);
            int genB = r.Next(0, 255);

            Color col = Color.FromArgb(genR, genG, genB);

            return col;
        }

        public Vector3 Random3DPoint()
        {
            int genA = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL);
            int genB = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL);
            int genC = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL);
            Vector3 vec = new Vector3(genA, genB, genC);
            return vec;
        }


        public int RandomInt()
        {
            int i = r.Next(LOW_INT_VAL, HIGH_INT_VAL);
            return i;
        }

        public int RandomInt(int MIN_VAL, int MAX_VAL)
        {
            int i = r.Next(MIN_VAL, MAX_VAL);
            return i;
        }

        public int RandomInt(int MAX_VAL)
        {
            int i = r.Next(MAX_VAL);
            return i;
        }
    }
}