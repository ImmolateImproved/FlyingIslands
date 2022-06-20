using Latios;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine.InputSystem;

public partial class DistanceTargetProviderSystem : SubSystem
{
    private BuildPhysicsWorld physicsWorldSystem;

    protected override void OnCreate()
    {
        physicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;

        Entities.WithAll<DistanceTargetProvider>()
            .ForEach((ref TargetProviderData targetData, in LocalToWorld ltw) =>
            {
                var pointDistance = new PointDistanceInput()
                {
                    Filter = targetData.filter,
                    MaxDistance = targetData.distance,
                    Position = ltw.Position
                };

                if (collisionWorld.CalculateDistance(pointDistance, out var hit))
                {
                    targetData.targetPosition = math.transform(math.inverse(ltw.Value), hit.Position);

                    targetData.target = hit.Entity;
                }
                else
                {
                    targetData.targetPosition = 0;
                    targetData.target = Entity.Null;
                }

            }).Run();
    }
}