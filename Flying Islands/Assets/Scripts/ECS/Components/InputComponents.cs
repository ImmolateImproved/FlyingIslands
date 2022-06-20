using Unity.Entities;
using Unity.Mathematics;

public class PlayerInputRef : IComponentData
{
    public PlayerInput playerInput;
}

public struct ShootingInput : IComponentData
{
    public bool shootInput;

    public bool canShoot;
}