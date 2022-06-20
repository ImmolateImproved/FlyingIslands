using Latios;
using Latios.Psyshock;
using Unity.Collections;
using Unity.Entities;

public class CollisionSystem : SubSystem
{
    private EntityQuery colliderQuery;
    private EntityQuery playerQuery;

    protected override void OnCreate()
    {
        colliderQuery = Fluent.WithAll<MoveSpeed>().PatchQueryForBuildingCollisionLayer().Build();
        playerQuery = Fluent.WithAll<Player>().WithAll<Health>().PatchQueryForBuildingCollisionLayer().Build();
    }

    protected override void OnUpdate()
    {
        Dependency = Physics.BuildCollisionLayer(colliderQuery, this).ScheduleParallel(out var colliderLayer, Allocator.TempJob, Dependency);
        Dependency = Physics.BuildCollisionLayer(playerQuery, this).ScheduleParallel(out var playerLayer, Allocator.TempJob, Dependency);

        var collisionProcessor = new CollisionProcessor
        {
            health = GetComponentDataFromEntity<Health>(),
            destroyCommandBuffer = latiosWorld.syncPoint.CreateDestroyCommandBuffer().AsParallelWriter()

        };

        Dependency = Physics.FindPairs(playerLayer, colliderLayer, collisionProcessor).ScheduleParallel(Dependency);

        Dependency = colliderLayer.Dispose(Dependency);
        Dependency = playerLayer.Dispose(Dependency);
    }
}

public struct CollisionProcessor : IFindPairsProcessor
{
    public PhysicsComponentDataFromEntity<Health> health;

    public DestroyCommandBuffer.ParallelWriter destroyCommandBuffer;

    public void Execute(FindPairsResult result)
    {
        if (Physics.DistanceBetween(result.bodyA.collider, result.bodyA.transform, result.bodyB.collider, result.bodyB.transform, 0, out var distanceResult))
        {
            var hp = health[result.entityA].value;
            hp -= 1;
            health[result.entityA] = new Health { value = hp };

            destroyCommandBuffer.Add(result.entityA, result.jobIndex);
        }
    }
}