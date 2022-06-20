using Latios;
using Unity.Entities;

public partial class ShootRateSystem : SubSystem
{
    protected override void OnUpdate()
    {
        var dt = Time.DeltaTime;

        Entities.ForEach((ref ShootRateData shootRate, ref ShootingInput input) =>
        {
            shootRate.timer += dt;

            input.canShoot = input.shootInput && shootRate.timer >= shootRate.timeBetweenShot;

            if (input.canShoot)
            {
                shootRate.timer = 0;
            }

        }).Run();
    }
}