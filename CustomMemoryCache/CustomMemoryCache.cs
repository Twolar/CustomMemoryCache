using CustomMemoryCache.Models;

namespace CustomMemoryCache;

#region Requirements

// [x] create a generic in-memory cache component, which other developers can use in their applications.
// [x] component should be able to store arbitrary types of objects, which are added and retrieved using a unique key (similar to a dictionary).
// [x] configurable threshold for max number of items, if full it should evict a least used item to make space for a new item
// [x] should be thread-safe singleton
// [] notify consumer if items evicted

#endregion

public class CustomMemoryCache<TKey> where TKey : notnull
{
    private static readonly object padlock = new object();
    private static CustomMemoryCache<TKey>? instance = null;

    private readonly int _cacheCapacity;
    private readonly Dictionary<TKey, LinkedListNode<CacheItem<TKey>>> _cache;
    private readonly LinkedList<CacheItem<TKey>> _lruList;

    private CustomMemoryCache(int size = 5)
    {
        _cacheCapacity = size;
        _cache = new Dictionary<TKey, LinkedListNode<CacheItem<TKey>>>(_cacheCapacity);
        _lruList = new LinkedList<CacheItem<TKey>>();
    }

    public static void Initialize(int size)
    {
        lock (padlock)
        {
            if (instance == null)
            {
                instance = new CustomMemoryCache<TKey>(size);
            }
        }
    }

    public static CustomMemoryCache<TKey> Instance
    {
        get
        {
            if (instance == null)
            {
                throw new InvalidOperationException("Must call Initialize before using this cache");
            }
            return instance;
        }
    }

    public void Add(TKey key, object value)
    {
        lock (padlock)
        {
            // Duplicate Key Check
            if (_cache.TryGetValue(key, out var duplicateNode))
            {
                throw new Exception("The key already exists in the cache");
            }

            // Duplicate Value Check
            if (_cache.Select(kvp => kvp.Value.Value)
                .Any(cacheItem => cacheItem.CIValue.Equals(value)))
            {
                throw new Exception($"The value already exists in the cache");
            }

            if (_cache.Count >= _cacheCapacity)
            {
                RemoveLeastUsed();
            }

            var cacheItem = new CacheItem<TKey>(key, value);
            var node = new LinkedListNode<CacheItem<TKey>>(cacheItem);

            _lruList.AddFirst(node);
            _cache.Add(key, node);
        }

    }

    public object Get(TKey key)
    {
        lock (padlock)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                return node.Value.CIValue;
            }
            throw new KeyNotFoundException($"The key {key} was not found in the store.");
        }
    }

    public int Count()
    {
        return _cache.Count;
    }

    private void RemoveLeastUsed()
    {
        lock (padlock)
        {
            var lastNode = _lruList.Last;
            if (lastNode != null)
            {
                _cache.Remove(lastNode.Value.CIKey);
                _lruList.RemoveLast();
            }
        }
    }
}