using Latios;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;

public partial class ApplyImpulseSystem : SubSystem
{
    protected override void OnUpdate()
    {
        Entities.WithChangeFilter<ImpulseData>()
            .ForEach((ref PhysicsVelocity physicsVelocity, ref ImpulseData impulseData, in PhysicsMass physicsMass) =>
            {
                physicsVelocity.ApplyLinearImpulse(physicsMass, impulseData.impulse);
                impulseData.impulse = 0;

            }).ScheduleParallel();
    }
}