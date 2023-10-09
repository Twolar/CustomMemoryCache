namespace CustomMemoryCacheTest;
using CustomMemoryCache;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TemporaryProjectInitPass()
    {
        var result = CustomMemoryCache.AddIntegers(1, 2);
        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public void TemporaryProjectInitFail()
    {
        var result = CustomMemoryCache.AddIntegers(1, 2);
        Assert.That(result, Is.EqualTo(3));
    }
}