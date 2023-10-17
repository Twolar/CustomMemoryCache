namespace CustomMemoryCache.Models;

public class CacheItem<TKey> where TKey : notnull
{
    public TKey CIKey { get; }
    public object CIValue { get; }

    public CacheItem(TKey key, object value)
    {
        CIKey = key ?? throw new ArgumentNullException(nameof(key));
        CIValue = value ?? throw new ArgumentNullException(nameof(value));
    }
}