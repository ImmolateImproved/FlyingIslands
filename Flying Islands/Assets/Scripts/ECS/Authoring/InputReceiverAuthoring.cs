using Unity.Entities;
using UnityEngine;

public class InputReceiverAuthoring : MonoBehaviour,IConvertGameObjectToEntity
{
    public bool isEnabled;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (!isEnabled)
            return;

        dstManager.AddComponentData(entity, new InputReceiver());
    }
}