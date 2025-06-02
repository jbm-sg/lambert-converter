namespace Lambert;

public class LambertConverter
{
    public static Point ConvertToWgs84(double x, double y, Zone zone)
    {
        Point pt = new(x, y, 0);
        return ConvertToWgs84(pt, zone);
    }

    public static Point ConvertToWgs84Deg(double x, double y, Zone zone)
    {
        Point pt = new(x, y, 0);
        pt = ConvertToWgs84(pt, zone);
        pt.ToDegree();
        return pt;
    }

    public static Point ConvertToWgs84(Point org, Zone zone)
    {
        LambertZone lzone = new(zone);

        if (zone == Zone.Lambert93)
        {
            return LambertToGeographic(org, lzone, LambertZone.LonMeridIers, LambertZone.EWgs84, LambertZone.DefaultEps);
        }
        Point pt1 = LambertToGeographic(org, lzone, LambertZone.LonMeridParis, LambertZone.EClarkIgn, LambertZone.DefaultEps);

        Point pt2 = GeographicToCartesian(pt1.X, pt1.Y, pt1.Z, LambertZone.AClarkIgn, LambertZone.EClarkIgn);

        pt2.Translate(-168, -60, 320);

        //WGS84 refers to greenwich
        return CartesianToGeographic(pt2, LambertZone.LonMeridGreenwich, LambertZone.AWgs84, LambertZone.EWgs84,
                                     LambertZone.DefaultEps);
    }

    private static Point LambertToGeographic(Point org, LambertZone zone, double lonMeridian, double e, double eps)
    {
        double n = zone.N();
        double c = zone.C();
        double xs = zone.Xs();
        double ys = zone.Ys();

        double x = org.X;
        double y = org.Y;


        double lon, gamma, r, latIso;

        r = Math.Sqrt((x - xs) * (x - xs) + (y - ys) * (y - ys));

        gamma = Math.Atan((x - xs) / (ys - y));

        lon = lonMeridian + gamma / n;

        latIso = -1 / n * Math.Log(Math.Abs(r / c));

        double lat = LatitudeFromLatitudeIso(latIso, e, eps);

        Point dest = new(lon, lat, 0);
        return dest;
    }

    private static double LatitudeFromLatitudeIso(double latISo, double e, double eps)
    {
        double phi0 = 2 * Math.Atan(Math.Exp(latISo)) - LambertZone.MPi2;
        double phiI = 2 * Math.Atan(Math.Pow((1 + e * Math.Sin(phi0)) / (1 - e * Math.Sin(phi0)), e / 2d) * Math.Exp(latISo)) -
                      LambertZone.MPi2;
        double delta = Math.Abs(phiI - phi0);

        while (delta > eps)
        {
            phi0 = phiI;
            phiI = 2 * Math.Atan(Math.Pow((1 + e * Math.Sin(phi0)) / (1 - e * Math.Sin(phi0)), e / 2d) * Math.Exp(latISo)) -
                   LambertZone.MPi2;
            delta = Math.Abs(phiI - phi0);
        }

        return phiI;
    }

    private static Point GeographicToCartesian(double lon, double lat, double he, double a, double e)
    {
        double n = LambertNormal(lat, a, e);

        Point pt = new(0, 0, 0);

        pt.X = (n + he) * Math.Cos(lat) * Math.Cos(lon);
        pt.Y = (n + he) * Math.Cos(lat) * Math.Sin(lon);
        pt.Z = (n * (1 - e * e) + he) * Math.Sin(lat);

        return pt;
    }

    private static double LambertNormal(double lat, double a, double e) => a / Math.Sqrt(1 - e * e * Math.Sin(lat) * Math.Sin(lat));

    private static Point CartesianToGeographic(Point org, double meridien, double a, double e, double eps)
    {
        double x = org.X, y = org.Y, z = org.Z;

        double lon = meridien + Math.Atan(y / x);

        double module = Math.Sqrt(x * x + y * y);

        double phi0 = Math.Atan(z / (module * (1 - (a * e * e) / Math.Sqrt(x * x + y * y + z * z))));
        double phiI = Math.Atan(z / module / (1 - a * e * e * Math.Cos(phi0) /
                                              (module * Math.Sqrt(1 - e * e * Math.Sin(phi0) * Math.Sin(phi0)))));
        double delta = Math.Abs(phiI - phi0);
        while (delta > eps)
        {
            phi0 = phiI;
            phiI = Math.Atan(z / module / (1 - a * e * e * Math.Cos(phi0) /
                                           (module * Math.Sqrt(1 - e * e * Math.Sin(phi0) * Math.Sin(phi0)))));
            delta = Math.Abs(phiI - phi0);
        }

        double he = module / Math.Cos(phiI) - a / Math.Sqrt(1 - e * e * Math.Sin(phiI) * Math.Sin(phiI));

        Point pt = new(lon, phiI, he);

        return pt;
    }
}