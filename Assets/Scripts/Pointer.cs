public struct Pointer 
{
    public int X { get; set; }
    public int Y { get; set; }

    public Pointer(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static bool operator == (Pointer x, Pointer y)
    {
        return x.X == y.X || x.Y == y.Y;
    }
    public static bool operator !=(Pointer x, Pointer y)
    {
        return x.X != y.X || x.Y != y.Y;
    }
    public static Pointer operator -(Pointer x, Pointer y)
    {
        return new Pointer(x.X - y.X, x.Y - y.Y);
    }

}
