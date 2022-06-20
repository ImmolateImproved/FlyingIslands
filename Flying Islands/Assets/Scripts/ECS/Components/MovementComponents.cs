using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

public struct MoveSpeed : IComponentData
{
    public float value;
}

public struct ImpulseData : IComponentData
{
    public float3 impulse;
}

public struct RotationData : IComponentData
{
    public float3 rotationSpeed;
}

public struct MoveDirection : IComponentData
{
    public float3 value;
}