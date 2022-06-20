using Latios;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class ShootDirectionSystem : SubSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<Player>().ForEach((ref ShootDirectionProvider directionProvider, in RayCastData rayCastData) =>
        {
            directionProvider.direction = rayCastData.ray.direction;

        }).Run();

        Entities.WithAll<AI, DistanceTargetProvider>()
            .ForEach((ref ShootDirectionProvider directionProvider, ref ShootingInput inputData, in TargetProviderData targetProvider, in Translation translation) =>
            {
                directionProvider.direction = math.normalizesafe(targetProvider.targetPosition - translation.Value);

                inputData.shootInput = targetProvider.target != Entity.Null;

            }).Run();
    }
}