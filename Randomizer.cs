using System.Drawing;
using OpenTK.Mathematics;

namespace ComputerGraphics3;

internal class Randomizer
{
    private const int LOW_INT_VAL = -25;
    private const int HIGH_INT_VAL = 25;
    private const int LOW_COORD_VAL = 50;
    private const int HIGH_COORD_VAL = 50;
    private readonly Random r;

    public Randomizer()
    {
        r = new Random();
    }

    public Color RandomColor()
    {
        var genR = r.Next(0, 255);
        var genG = r.Next(0, 255);
        var genB = r.Next(0, 255);

        var col = Color.FromArgb(genR, genG, genB);

        return col;
    }

    public Vector3 Random3DPoint()
    {
        var genA = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL);
        var genB = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL);
        var genC = r.Next(LOW_COORD_VAL, HIGH_COORD_VAL);
        var vec = new Vector3(genA, genB, genC);
        return vec;
    }


    public int RandomInt()
    {
        var i = r.Next(LOW_INT_VAL, HIGH_INT_VAL);
        return i;
    }

    public int RandomInt(int MIN_VAL, int MAX_VAL)
    {
        var i = r.Next(MIN_VAL, MAX_VAL);
        return i;
    }

    public int RandomInt(int MAX_VAL)
    {
        var i = r.Next(MAX_VAL);
        return i;
    }
}