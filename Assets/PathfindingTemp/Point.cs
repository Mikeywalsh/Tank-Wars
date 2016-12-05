[System.Serializable]
public struct Point {

    public int X;
    public int Y;

	public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        return obj is Point && X == ((Point)obj).X && Y == ((Point)obj).Y;
    }

    public override int GetHashCode()
    {
        return int.Parse(X.ToString("00") + Y.ToString("00"));
    }

    public static bool operator ==(Point pa, Point pb)
    {
        return pa.X == pb.X && pa.Y == pb.Y;
    }

    public static bool operator !=(Point pa, Point pb)
    {
        return !(pa == pb);
    }

    public static Point operator +(Point pa, Point pb)
    {
        return new Point(pa.X + pb.X, pa.Y + pb.Y);
    }

    public static Point operator -(Point pa, Point pb)
    {
        return new Point(pa.X - pb.X, pa.Y - pb.Y);
    }
}
