namespace Lambert.Tests;

public class LambertConverterTests
{
    [Test]
    public void TestBug()
    {
        Point pt = LambertConverter.ConvertToWgs84Deg(668832.5384, 6950138.7285, Zone.Lambert93);
        Assert.Multiple(() =>
        {
            Assert.That(pt.X, Is.EqualTo(2.56865).Within(0.00001));
            Assert.That(pt.Y, Is.EqualTo(49.64961).Within(0.00001));
        });
    }
}