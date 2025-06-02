namespace Lambert;

public enum Unit { Degree, Grad, Radian, Meter };

public class Point
{
    private const double RadianTodegree = 180.0 / Math.PI;

    public Point(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public void Translate(double x, double y, double z)
    {
        X += x;
        Y += y;
        Z += z;
    }

    private void Scale(double scale)
    {
        X *= scale;
        Y *= scale;
        Z *= scale;
    }

    public void ToDegree() => Scale(RadianTodegree);
}