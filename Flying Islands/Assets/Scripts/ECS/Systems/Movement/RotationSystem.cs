using Latios;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class RotationSystem : SubSystem
{
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;

        Entities.ForEach((ref RotationEulerXYZ rotation, in RotationData rotationData) =>
        {
            rotation.Value += rotationData.rotationSpeed * dt;

        }).ScheduleParallel();
    }
}