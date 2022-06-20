using Latios;
using Unity.Entities;
using Unity.Transforms;

public partial class UpdateShootPointSystem : SubSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ShootPointGORef shootPointGO, ref ShootPoint shootPoint, in Translation translation) =>
        {
            shootPoint.value = shootPointGO.value.position;

        }).WithoutBurst().Run();

        Entities.WithNone<ShootPointGORef>()
            .ForEach((ref ShootPoint shootPoint, in Translation translation) =>
            {
                shootPoint.value = translation.Value;

            }).Run();
    }
}