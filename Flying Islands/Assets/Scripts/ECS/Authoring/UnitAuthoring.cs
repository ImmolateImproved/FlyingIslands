using Unity.Entities;
using UnityEngine;

public enum UnitType
{
    Player, AI
}

public class UnitAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public UnitType unitType;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        switch (unitType)
        {
            case UnitType.Player:
                {
                    dstManager.AddComponentData(entity, new Player());
                    break;
                }

            case UnitType.AI:
                {
                    dstManager.AddComponentData(entity, new AI());
                    break;
                }
        }


    }
}