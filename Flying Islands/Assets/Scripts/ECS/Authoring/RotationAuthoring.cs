using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

public class RotationAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Vector3 rotationSpeed;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new RotationData
        {
            rotationSpeed = rotationSpeed
        });

        dstManager.AddComponentData(entity, new RotationEulerXYZ());
    }
}