namespace CustomMemoryCacheTest;
using CustomMemoryCache;

public class Tests
{
    // Arrange
    // Act
    // Assert

    private CustomMemoryCache<string> cache;

    [SetUp]
    public void SetUp()
    {
        CustomMemoryCache<string>.Initialize(3);
        cache = CustomMemoryCache<string>.Instance;
    }

    [Test]
    public void AddAndGet()
    {
        var key = "ANG-item1";
        var value = "ANG-value1";
        cache.Add(key, value);
        Assert.That(cache.Get(key), Is.EqualTo(value));
    }

    [Test]
    public void AddDoesNotAddDuplicateKeyToCache()
    {
        var duplicateKey = "ADNADKTC-item1";

        cache.Add(duplicateKey, "ADNADKTC-value1");

        var exception = Assert.Throws<Exception>(() => cache.Add(duplicateKey, "ADNADKTC-value2"));
        Assert.That(exception.Message, Is.EqualTo("The key already exists in the cache"));
    }

    [Test]
    public void AddDoesNotAddDuplicateObjectValueToCache()
    {
        var duplicateValue = "ADNADOVTC-value1";

        cache.Add("ADNADOVTC-item1", duplicateValue);

        var exception = Assert.Throws<Exception>(() => cache.Add("ADNADOVTC-item2", duplicateValue));
        Assert.That(exception.Message, Is.EqualTo("The value already exists in the cache"));
    }

    [Test]
    public void GetThrowsExceptionWhenKeyDoesNotExist()
    {
        var keyThatDoesNotExist = "GTEWKDNE-item1";
        var exception = Assert.Throws<KeyNotFoundException>(() => cache.Get(keyThatDoesNotExist));
        Assert.That(exception.Message, Is.EqualTo($"The key {keyThatDoesNotExist} was not found in the store."));
    }

    [Test]
    public void RemoveLeastUsed()
    {
        var keyToBeEvictedFromCache = "RLU-item1";

        cache.Add(keyToBeEvictedFromCache, "RLU-value1");
        cache.Add("RLU-item2", "RLU-value2");
        cache.Add("RLU-item3", "RLU-value3");
        cache.Add("RLU-item4", "RLU-value4");

        var exception = Assert.Throws<KeyNotFoundException>(() => cache.Get(keyToBeEvictedFromCache));
        Assert.That(exception.Message, Is.EqualTo($"The key {keyToBeEvictedFromCache} was not found in the store."));
    }
}