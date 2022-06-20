using Unity.Entities;
using Unity.Mathematics;

public struct SpawnerData : IComponentData
{
    public float3 spawnRange;

    public int spawnCountPerIteration;
    public float timeBetweenSpawn, spawnTimer;
}

public struct PrefabToSpawn : IBufferElementData
{
    public Entity value;
}

public struct GridSpawner : IComponentData
{
    public float3 dimensions;
}