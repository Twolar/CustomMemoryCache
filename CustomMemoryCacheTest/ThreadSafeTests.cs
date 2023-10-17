namespace CustomMemoryCacheTest;
using CustomMemoryCache;

public class ThreadSafeTests
{
    // Arrange
    // Act
    // Assert

    [Test]
    public void IsThreadSafe()
    {
        var cacheMaxSize = 3;
        CustomMemoryCache<int>.Initialize(cacheMaxSize);
        var cacheInstance1 = CustomMemoryCache<int>.Instance;
        var cacheInstance2 = CustomMemoryCache<int>.Instance;
        var cacheInstance3 = CustomMemoryCache<int>.Instance;

        Task task1 = Task.Factory.StartNew(() => cacheInstance1.Add(1, "value1"));
        Task task2 = Task.Factory.StartNew(() => cacheInstance2.Add(2, "value2"));
        Task task3 = Task.Factory.StartNew(() => cacheInstance3.Add(3, "value3"));

        Task.WaitAll(task1, task2, task3);

        Assert.That(cacheInstance1.Get(1), Is.EqualTo("value1"));
        Assert.That(cacheInstance1.Get(2), Is.EqualTo("value2"));
        Assert.That(cacheInstance1.Get(3), Is.EqualTo("value3"));

        Assert.That(cacheMaxSize, Is.EqualTo(cacheInstance1.Count()));
    }
}