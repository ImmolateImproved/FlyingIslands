using Latios;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public partial class SpawnerSystem : SubSystem
{
    private Rng m_rng;

    public override void OnNewScene()
    {
        m_rng = new Rng("SpawnerSystem");
    }

    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;

        var rng = m_rng.Shuffle();

        var ecb = latiosWorld.syncPoint.CreateInstantiateCommandBuffer<Translation, Rotation>().AsParallelWriter();

        Entities.WithNone<GridSpawner>()
            .ForEach((int entityInQueryIndex, ref SpawnerData spawnerData, in DynamicBuffer<PrefabToSpawn> prefabs, in LocalToWorld ltw) =>
            {
                spawnerData.spawnTimer -= dt;

                if (spawnerData.spawnTimer <= 0)
                {
                    spawnerData.spawnTimer = spawnerData.timeBetweenSpawn;

                    var random = rng.GetSequence(entityInQueryIndex);

                    for (int i = 0; i < spawnerData.spawnCountPerIteration; i++)
                    {
                        var leftUpperCorner = ltw.Position - spawnerData.spawnRange / 2;
                        var rightBottomCorner = ltw.Position + spawnerData.spawnRange / 2;

                        var position = new float3(random.NextFloat3(leftUpperCorner, rightBottomCorner));

                        ecb.Add(prefabs[random.NextInt(0, prefabs.Length)].value,
                            new Translation { Value = position },
                            new Rotation { Value = quaternion.LookRotationSafe(ltw.Forward, ltw.Up) },
                            //new PhysicsGravityFactor { Value = random.NextFloat(-10, 10) },
                            //new PhysicsVelocity { Linear = random.NextFloat3Direction() * 50 },
                            entityInQueryIndex);
                    }
                }

            }).ScheduleParallel();

        Entities.ForEach((int entityInQueryIndex, ref SpawnerData spawnerData, in DynamicBuffer<PrefabToSpawn> prefabs, in GridSpawner gridSpawner, in LocalToWorld ltw) =>
        {
            spawnerData.spawnTimer -= dt;

            if (spawnerData.spawnTimer <= 0)
            {
                spawnerData.spawnTimer = spawnerData.timeBetweenSpawn;

                var random = rng.GetSequence(entityInQueryIndex);

                for (int y = 0; y < gridSpawner.dimensions.y; y++)
                {
                    for (int x = 0; x < gridSpawner.dimensions.x; x++)
                    {
                        var position = new float3(ltw.Position.x + x * 4, ltw.Position.y + y * 4, ltw.Position.z) - gridSpawner.dimensions * 2;

                        ecb.Add(prefabs[random.NextInt(0, prefabs.Length)].value,
                            new Translation { Value = position },
                            new Rotation { Value = quaternion.LookRotationSafe(ltw.Forward, ltw.Up) },
                            //new PhysicsGravityFactor { Value = random.NextFloat(-10, 10) },
                            //new PhysicsVelocity { Linear = random.NextFloat3Direction() * 50 },
                            entityInQueryIndex);
                    }
                }
            }

        }).ScheduleParallel();
    }
}