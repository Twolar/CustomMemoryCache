using CustomMemoryCache.Models;

namespace CustomMemoryCacheTest;

public class CacheItemTests
{
    // Arrange
    // Act
    // Assert

    [Test]
    public void SuccessfullyCreate() {
        var key = 1;
        var value = "value1";

        var cacheItem = new CacheItem<int>(key, value);

        Assert.That(cacheItem.CIKey, Is.EqualTo(key));
        Assert.That(cacheItem.CIValue, Is.EqualTo(value));
    }

    [Test]
    public void CIKeyCannotBeNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CacheItem<string>(null, "test"));

        Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'key')"));
    }
    
    [Test]
    public void CIValueCannotBeNull()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new CacheItem<int>(1, null));

        Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'value')"));
    }
}