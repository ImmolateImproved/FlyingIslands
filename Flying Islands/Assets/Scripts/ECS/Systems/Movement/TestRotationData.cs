using Latios;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct TestRotationComponentData : IComponentData
{

}

public class TestRotationData : MonoBehaviour, IConvertGameObjectToEntity
{
    public Transform targetRotation;

    public bool addTestCompData;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (addTestCompData)
        {
            dstManager.AddComponentData(entity, new TestRotationComponentData());
        }
    }
}

public partial class TestRotationSystem : SubSystem
{
    protected override void OnUpdate()
    {
        var input = this.GetSingleton<PlayerInputRef>().playerInput;

        var noInput = Mathf.Approximately(input.move.magnitude, 0);

        if (noInput)
            return;

        Entities.WithAll<TestRotationComponentData>()
            .ForEach((TestRotationData rotationData, ref Rotation rotation) =>
            {
                var rotationY = rotationData.targetRotation.eulerAngles.y + Mathf.Atan2(input.move.x, input.move.y) * Mathf.Rad2Deg;
                //moveDirection = Quaternion.Euler(0, rotationY, 0) * Vector3.forward;
                rotationData.transform.rotation = quaternion.EulerXYZ(0, math.radians(rotationY), 0);

            }).WithoutBurst().Run();

        Entities.WithNone<TestRotationComponentData>()
            .ForEach((TestRotationData rotationData) =>
            {
                var rotationY = rotationData.targetRotation.eulerAngles.y + Mathf.Atan2(input.move.x, input.move.y) * Mathf.Rad2Deg;
                //moveDirection = Quaternion.Euler(0, rotationY, 0) * Vector3.forward;
                rotationData.transform.rotation = Quaternion.Euler(0, rotationY, 0);

            }).WithoutBurst().Run();
    }
}