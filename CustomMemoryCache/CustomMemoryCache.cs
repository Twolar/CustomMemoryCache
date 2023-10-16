namespace CustomMemoryCache;

#region Requirements

// [] create a generic in-memory cache component, which other developers can use in their applications.
// [] component should be able to store arbitrary types of objects, which are added and retrieved using a unique key (similar to a dictionary).
// [] configurable threshold for max number of items, if full it should evict a least used item to make space for a new item
// [] should be thread-safe singleton
// [] notify consumer if items evicted

#endregion

// TODO: Double check data structure layout
// TODO: Unique key, some sort of hashing?
// TODO: Singleton / Thread safe tests
// TODO: Notify consumer of items evicted
// TODO: Investigate compiler warnings regarding null references

public class CustomMemoryCache<TKey>
{
    private static readonly object padlock = new object();
    private static CustomMemoryCache<TKey> instance = null;

    private readonly int _cacheCapacity;
    private readonly Dictionary<TKey, LinkedListNode<CacheItem>> _cache;
    private readonly LinkedList<CacheItem> _lruList;

    private CustomMemoryCache(int size = 5)
    {
        _cacheCapacity = size;
        _cache = new Dictionary<TKey, LinkedListNode<CacheItem>>(_cacheCapacity);
        _lruList = new LinkedList<CacheItem>();
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
            lock (padlock)
            {
                if (instance == null)
                {
                    throw new InvalidOperationException("Must call Initialize before using this cache");
                }
                return instance;
            }
        }
    }

    public bool Add(TKey key, object value)
    {
        if (_cache.Count >= _cacheCapacity)
        {
            RemoveLeastUsed();
        }

        // Duplicate Key Check
        if (_cache.TryGetValue(key, out var duplicateNode))
        {
            throw new Exception("The key already exists in the cache");
        }

        var cacheItem = new CacheItem { CIKey = key, CIValue = value };
        var node = new LinkedListNode<CacheItem>(cacheItem);

        // Duplicate Value Check
        if (_cache.Select(kvp => kvp.Value.Value)
            .Any(cacheItem => cacheItem.CIValue.Equals(value)))
        {
            throw new Exception($"The value already exists in the cache");
        }

        _lruList.AddFirst(node);
        _cache.Add(key, node);

        return true;
    }

    public object Get(TKey key)
    {
        if (_cache.TryGetValue(key, out var node))
        {
            _lruList.Remove(node);
            _lruList.AddFirst(node);
            return node.Value.CIValue;
        }
        throw new KeyNotFoundException($"The key {key} was not found in the store.");
    }

    private void RemoveLeastUsed()
    {
        var lastNode = _lruList.Last;
        _cache.Remove(lastNode.Value.CIKey);
        _lruList.RemoveLast();
    }

    private class CacheItem
    {
        public TKey CIKey { get; set; }
        public object CIValue { get; set; }
    }
}