using Latios;
using Latios.Psyshock;
using Unity.Entities;

public partial class LifetimeSystem : SubSystem
{
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;

        var ecb = latiosWorld.syncPoint.CreateDestroyCommandBuffer().AsParallelWriter();

        Entities.ForEach((Entity e, int entityInQueryIndex, ref Lifetime lifetime) =>
        {
            lifetime.timeToDeath -= dt;

            if (lifetime.timeToDeath <= 0)
            {
                ecb.Add(e, entityInQueryIndex);
            }

        }).ScheduleParallel();
    }
}