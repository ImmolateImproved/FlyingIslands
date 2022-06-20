using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject[] prefabs;

    public Vector3 spawnRange;

    public int spawnCountPerIteration;

    public float timeBetweenSpawn;

    public bool gridSpawner;
    public Vector3 dimensions;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new SpawnerData
        {
            spawnRange = spawnRange,
            spawnCountPerIteration = spawnCountPerIteration,
            timeBetweenSpawn = timeBetweenSpawn,
            spawnTimer = 0
        });

        var prefabsBuffer = dstManager.AddBuffer<PrefabToSpawn>(entity).Reinterpret<Entity>();
        for (int i = 0; i < prefabs.Length; i++)
        {
            prefabsBuffer.Add(conversionSystem.GetPrimaryEntity(prefabs[i]));
        }

        if (gridSpawner)
        {
            dstManager.AddComponentData(entity, new GridSpawner { dimensions = dimensions });
        }
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.AddRange(prefabs);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnRange);
    }
}