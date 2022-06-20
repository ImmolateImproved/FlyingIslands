using Latios;
using Unity.Entities;

public partial class PlayerInputSystem : SubSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((PlayerInput playerInput, ref ShootingInput shootInput) =>
        {
            shootInput.shootInput = playerInput.combatState;

        }).WithoutBurst().Run();
    }
}