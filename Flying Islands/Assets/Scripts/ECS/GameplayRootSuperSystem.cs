using Latios;
using Unity.Entities;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public class GameplayRootSuperSystem : RootSuperSystem
{
    protected override void CreateSystems()
    {
        GetOrCreateAndAddSystem<PlayerInputSystem>();
        GetOrCreateAndAddSystem<TestRotationSystem>();

        GetOrCreateAndAddSystem<SpawnerSystem>();
        GetOrCreateAndAddSystem<RotationSystem>();
        //GetOrCreateAndAddSystem<CollisionSystem>();

        GetOrCreateAndAddSystem<PhysicsMovementSystem>();
        GetOrCreateAndAddSystem<MovingOnPlatformSystem>();

        //Shooting
        GetOrCreateAndAddSystem<RaycastTargetProviderSystem>();
        GetOrCreateAndAddSystem<DistanceTargetProviderSystem>();

        GetOrCreateAndAddSystem<UpdateShootPointSystem>();
        GetOrCreateAndAddSystem<ShootDirectionSystem>();

        GetOrCreateAndAddSystem<ShootRateSystem>();
        GetOrCreateAndAddSystem<ProjectileShootingSystem>();
        GetOrCreateAndAddSystem<RaycastShootingSystem>();
        //

        GetOrCreateAndAddSystem<ApplyImpulseSystem>();

        GetOrCreateAndAddSystem<LifetimeSystem>();
    }
}