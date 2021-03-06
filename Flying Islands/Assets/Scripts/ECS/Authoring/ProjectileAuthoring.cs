using Unity.Entities;
using UnityEngine;

public class ProjectileAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float moveSpeed;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new Projectile { moveSpeed = moveSpeed });
    }
}