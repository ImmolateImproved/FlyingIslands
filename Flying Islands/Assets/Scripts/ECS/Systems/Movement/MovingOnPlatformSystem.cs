using Latios;
using Latios.Psyshock;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;

public partial class MovingOnPlatformSystem : SubSystem
{
    private BuildPhysicsWorld physicsWorldSystem;

    protected override void OnCreate()
    {
        physicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        var collisionWorld = physicsWorldSystem.PhysicsWorld.CollisionWorld;

        Entities.ForEach((ThirdPersonController controller, PlayerInput playerInput) =>
        {
            var platformCheck = controller.platformCheck;

            var raycastInput = new RaycastInput()
            {
                Start = platformCheck.position,
                End = platformCheck.position - platformCheck.up * controller.platformCheckDistance,
                Filter = controller.platformLayer
            };

            if (collisionWorld.CastRay(raycastInput, out var hit))
            {
                if (EntityManager.HasComponent<MoveDirection>(hit.Entity))
                {
                    var velocity = EntityManager.GetComponentData<PhysicsVelocity>(hit.Entity).Linear;
                    controller.Move(velocity);

                    EntityManager.SetComponentData(hit.Entity, new MoveDirection { value = controller.TargetDirection });

                    if (playerInput.switchControll)
                    {
                        playerInput.switchControll = false;

                        if (controller.disableMovement)
                        {
                            EntityManager.RemoveComponent<InputReceiver>(hit.Entity);
                            controller.disableMovement = false;
                        }
                        else
                        {
                            EntityManager.AddComponent<InputReceiver>(hit.Entity);
                            controller.disableMovement = true;
                        }
                    }
                }
            }

        }).WithStructuralChanges().Run();
    }
}