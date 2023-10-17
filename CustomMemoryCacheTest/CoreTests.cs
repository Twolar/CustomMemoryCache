using CustomMemoryCache;
using CustomMemoryCacheTest.Helpers.ArbitraryObjects;

namespace CustomMemoryCacheTest;

public class CoreTests
{
    // Arrange
    // Act
    // Assert

    private CustomMemoryCache<string> _cache;
    private int _cacheMaxSize = 3;

    [SetUp]
    public void SetUp()
    {
        CustomMemoryCache<string>.Initialize(_cacheMaxSize);
        _cache = CustomMemoryCache<string>.Instance;
    }

    [Test]
    public void IsSingleton()
    {
        var cache1 = CustomMemoryCache<string>.Instance;
        var cache2 = CustomMemoryCache<string>.Instance;

        Assert.That(_cache, Is.SameAs(cache1));
        Assert.That(_cache, Is.SameAs(cache2));
        Assert.That(cache1, Is.SameAs(cache2));
    }

    [Test]
    public void AddAndGetAndCount()
    {
        var key = "ANG-item1";
        var value = "ANG-value1";
        _cache.Add(key, value);
        Assert.That(_cache.Get(key), Is.EqualTo(value));
        Assert.That(_cache.Count(), Is.EqualTo(1));
    }

    [Test]
    public void AddDoesNotAddDuplicateKeyToCache()
    {
        var duplicateKey = "ADNADKTC-item1";

        _cache.Add(duplicateKey, "ADNADKTC-value1");
        var exception = Assert.Throws<Exception>(() => _cache.Add(duplicateKey, "ADNADKTC-value2"));

        Assert.That(exception.Message, Is.EqualTo("The key already exists in the cache"));
        Assert.That(_cache.Count(), Is.EqualTo(2));
    }

    [Test]
    public void AddDoesNotAddDuplicateObjectValueToCache()
    {
        var duplicateValue = "ADNADOVTC-value1";

        _cache.Add("ADNADOVTC-item1", duplicateValue);
        var exception = Assert.Throws<Exception>(() => _cache.Add("ADNADOVTC-item2", duplicateValue));

        Assert.That(exception.Message, Is.EqualTo("The value already exists in the cache"));
        Assert.That(_cache.Count(), Is.EqualTo(_cacheMaxSize));
    }

    [Test]
    public void GetThrowsExceptionWhenKeyDoesNotExist()
    {
        var keyThatDoesNotExist = "GTEWKDNE-item1";

        var exception = Assert.Throws<KeyNotFoundException>(() => _cache.Get(keyThatDoesNotExist));

        Assert.That(exception.Message, Is.EqualTo($"The key {keyThatDoesNotExist} was not found in the store."));
        Assert.That(_cache.Count(), Is.EqualTo(_cacheMaxSize));
    }

    [Test]
    public void RemoveLeastUsed()
    {
        var keyToBeEvictedFromCache = "RLU-item1";

        _cache.Add(keyToBeEvictedFromCache, "RLU-value1");
        _cache.Add("RLU-item2", "RLU-value2");
        _cache.Add("RLU-item3", "RLU-value3");
        _cache.Add("RLU-item4", "RLU-value4");

        var exception = Assert.Throws<KeyNotFoundException>(() => _cache.Get(keyToBeEvictedFromCache));

        Assert.That(exception.Message, Is.EqualTo($"The key {keyToBeEvictedFromCache} was not found in the store."));
        Assert.That(_cache.Count(), Is.EqualTo(_cacheMaxSize));
    }

    [Test]
    public void StoreArbitraryObjects()
    {
        var arbitraryObjectOne = new ArbitraryObjectOne
        {
            Id = 1,
            Name = "TestOne"
        };
        var arbitraryObjectTwo = new ArbitraryObjectTwo
        {
            UniqueId = Guid.NewGuid(),
            Value = 123.45
        };
        var arbitraryObjectThree = new ArbitraryObjectThree
        {
            Key = "TestKey",
            Values = new List<int> { 1, 2, 3 }
        };

        var aOKeyOne = "SAO-One";
        var aOKeyTwo = "SAO-Two";
        var aOKeyThree = "SAO-Three";
        _cache.Add(aOKeyOne, arbitraryObjectOne);
        _cache.Add(aOKeyTwo, arbitraryObjectTwo);
        _cache.Add(aOKeyThree, arbitraryObjectThree);

        Assert.That(_cache.Get(aOKeyOne), Is.EqualTo(arbitraryObjectOne));
        Assert.That(_cache.Get(aOKeyTwo), Is.EqualTo(arbitraryObjectTwo));
        Assert.That(_cache.Get(aOKeyThree), Is.EqualTo(arbitraryObjectThree));
    }
}