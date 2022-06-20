using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public struct RayCastData : IComponentData
{
    public UnityEngine.Ray ray;
}

public struct ShootDirectionProvider : IComponentData
{
    public float3 direction;
}

public struct TargetProviderData : IComponentData
{
    public Entity target;

    public float3 targetPosition;

    public float distance;
    public CollisionFilter filter;
}

public struct RaycastTargetProvider : IComponentData
{

}

public struct DistanceTargetProvider : IComponentData
{ 

}

public struct RaycastShootingData : IComponentData
{
    public float impulse;
}

public struct ShootRateData : IComponentData
{
    public float timeBetweenShot;
    public float timer;
}

public struct ProjectileShootingData : IComponentData
{
    public Entity projectilePrefab;
    public int spreadAmount;
    public float projectileSpeed;
}

public struct Projectile : IComponentData
{
    public float moveSpeed;
}

public struct ShootPoint : IComponentData
{
    public float3 value;
}

public class ShootPointGORef : IComponentData
{
    public Transform value;
}