using Latios;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial class RaycastShootingSystem : SubSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((in ShootingInput shootInput, in TargetProviderData targetData, in RaycastShootingData raycastShootingData, in LocalToWorld ltw) =>
        {
            var canAttack = shootInput.canShoot && HasComponent<Translation>(targetData.target);

            if (canAttack)
            {
                var direction = ltw.Forward;//math.normalizesafe(findTargetData.targetPosition - ltw.Position);

                SetComponent(targetData.target, new ImpulseData
                {
                    impulse = direction * raycastShootingData.impulse
                });
            }

        }).Run();

        Entities.WithAll<RaycastShootingData>()
            .ForEach((LineRenderer lineRenderer, in ShootPoint shootPoint, in TargetProviderData targetData, in ShootingInput input, in LocalToWorld ltw) =>
            {
                lineRenderer.enabled = input.shootInput;
                var startPoint = math.transform(math.inverse(ltw.Value), shootPoint.value);

                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, targetData.targetPosition);

            }).WithoutBurst().Run();
    }
}