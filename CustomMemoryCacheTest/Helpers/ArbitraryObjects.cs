namespace CustomMemoryCacheTest.Helpers.ArbitraryObjects;

public class ArbitraryObjectOne
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class ArbitraryObjectTwo
{
    public Guid UniqueId { get; set; }
    public double Value { get; set; }
}

public class ArbitraryObjectThree
{
    public string Key { get; set; }
    public List<int> Values { get; set; }
}
