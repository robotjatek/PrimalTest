namespace PrimalTestDotNet;

public struct IntVector2(int x, int y)
{
    public static readonly IntVector2 UnitX = new(1, 0);
    public static readonly IntVector2 UnitY = new(0, 1);

    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;
    public readonly int Length => (int)Math.Sqrt(X * X + Y * Y);

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X + b.X, a.Y + b.Y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.X - b.X, a.Y - b.Y);
    }

    public static IntVector2 operator *(IntVector2 a, int b)
    {
        return new IntVector2(a.X * b, a.Y * b);
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        return !a.Equals(b);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (IntVector2) obj;
        return X == other.X && Y == other.Y;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
