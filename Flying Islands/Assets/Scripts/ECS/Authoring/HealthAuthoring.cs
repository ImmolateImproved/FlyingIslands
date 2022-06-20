using Unity.Entities;
using UnityEngine;

public class HealthAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float health;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Health { value = health });
    }
}