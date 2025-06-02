namespace Lambert;

public enum Zone
{
    LambertI = 0,
    LambertIi = 1,
    LambertIii = 2,
    LambertIv = 3,
    LambertIiExtended = 4,
    Lambert93 = 5
}

public class LambertZone(Zone Zone)
{
    private static readonly double[] LambertN = [0.7604059656, 0.7289686274, 0.6959127966, 0.6712679322, 0.7289686274, 0.7256077650];
    private static readonly double[] LambertC = [11603796.98, 11745793.39, 11947992.52, 12136281.99, 11745793.39, 11754255.426];
    private static readonly double[] LambertXs = [600000.0, 600000.0, 600000.0, 234.358, 600000.0, 700000.0];
    private static readonly double[] LambertYs = [5657616.674, 6199695.768, 6791905.085, 7239161.542, 8199695.768, 12655612.050];

    public static readonly double MPi2 = Math.PI / 2.0;
    public static readonly double DefaultEps = 1e-10;
    public static readonly double EClarkIgn = 0.08248325676;
    public static readonly double EWgs84 = 0.08181919106;

    public static readonly double AClarkIgn = 6378249.2;
    public static readonly double AWgs84 = 6378137.0;
    public static readonly double LonMeridParis = 0;
    public static readonly double LonMeridGreenwich = 0.04079234433;
    public static readonly double LonMeridIers = 3.0 * Math.PI / 180.0;

    public double N() => LambertN[(int)Zone];

    public double C() => LambertC[(int)Zone];
    public double Xs() => LambertXs[(int)Zone];
    public double Ys() => LambertYs[(int)Zone];

    public Zone Zone { get; private set; } = Zone;
}