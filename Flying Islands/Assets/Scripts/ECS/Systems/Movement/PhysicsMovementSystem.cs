using Latios;
using Latios.Psyshock;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public partial class PhysicsMovementSystem : SubSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<InputReceiver>().ForEach((ref PhysicsVelocity physicsVelocity, in MoveDirection moveDirection, in MoveSpeed moveSpeed) =>
        {
            var velocity = moveDirection.value * moveSpeed.value;
            physicsVelocity.Linear = velocity;

        }).Run();
    }
}