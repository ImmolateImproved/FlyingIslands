using Latios;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class RaycastTargetProviderSystem : SubSystem
{
    private BuildPhysicsWorld physicsWorldSystem;

    protected override void OnCreate()
    {
        physicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;

        var rayOrigin = new Vector3(0.5f, 0.5f, 0f);
        var ray = Camera.main.ViewportPointToRay(rayOrigin);

        Entities.ForEach((ref RayCastData raycastData) =>
        {
            raycastData.ray = ray;

        }).Run();

        Entities.WithAll<RaycastTargetProvider>()
            .ForEach((ref TargetProviderData targetData, in RayCastData rayCastData, in LocalToWorld ltw) =>
            {
                var rayEnd = rayCastData.ray.GetPoint(targetData.distance);

                var raycastInput = new RaycastInput()
                {
                    Filter = targetData.filter,
                    Start = rayCastData.ray.origin,
                    End = rayEnd
                };

                if (collisionWorld.CastRay(raycastInput, out var hit))
                {
                    targetData.targetPosition = math.transform(math.inverse(ltw.Value), hit.Position);

                    targetData.target = hit.Entity;
                }
                else
                {
                    var rayEndPoint = rayEnd;

                    targetData.targetPosition = math.transform(math.inverse(ltw.Value), rayEndPoint);
                    targetData.target = Entity.Null;
                }

            }).Run();
    }
}
