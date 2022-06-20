using Unity.Entities;


public struct Lifetime : IComponentData
{
    public float timeToDeath;
}

public struct Health : IComponentData
{
    public float value;
}