using Latios;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

public partial class ProjectileShootingSystem : SubSystem
{
    protected override void OnUpdate()
    {
        var ecb = latiosWorld.syncPoint.CreateInstantiateCommandBuffer<Translation, PhysicsVelocity>();

        Entities.ForEach((in ProjectileShootingData projectileShootingData, in ShootingInput shootInput, in ShootDirectionProvider directionProvider, in ShootPoint shootPoint) =>
        {
            if (shootInput.canShoot)
            {
                var spreadAmount = projectileShootingData.spreadAmount;

                for (int i = 0; i < spreadAmount; i++)
                {
                    ecb.Add(projectileShootingData.projectilePrefab,
                        new Translation { Value = shootPoint.value },
                            new PhysicsVelocity { Linear = directionProvider.direction * projectileShootingData.projectileSpeed });
                }
            }

        }).Run();
    }
}