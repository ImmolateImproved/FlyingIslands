using Unity.Entities;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputRefAothoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new PlayerInputRef 
        {
            playerInput = GetComponent<PlayerInput>()
        });
    }
}