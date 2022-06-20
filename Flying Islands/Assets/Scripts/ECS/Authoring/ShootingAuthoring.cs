using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

public enum TargetProvider
{
    None, Raycast, PointDistance
}

public enum ShootingType
{
    Projectile, Raycast
}

public class ShootingAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    [Header("Shooting data")]
    public GameObject projectilePrefab;
    public GameObject shootPoint;
    public bool useGOShootPoint;
    public float timeBetweenShot;

    public ShootingType shootingType;

    [Header("Projectile shooting data")]
    public int spreadAmount;
    public float projectileSpeed;

    [Header("Raycast/Distance target provider data")]
    public TargetProvider targetProvider;

    public float distance;
    public float impulse;

    public PhysicsCategoryTags belongsTo;
    public PhysicsCategoryTags collidesWith;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new ShootingInput());
        dstManager.AddComponentData(entity, new RayCastData());

        dstManager.AddComponentData(entity, new ShootRateData
        {
            timeBetweenShot = timeBetweenShot
        });

        dstManager.AddComponentData(entity, new ShootPoint());

        if (useGOShootPoint)
        {
            dstManager.AddComponentData(entity, new ShootPointGORef
            {
                value = shootPoint.transform
            });
        }

        switch (shootingType)
        {
            case ShootingType.Projectile:
                {
                    dstManager.AddComponentData(entity, new ShootDirectionProvider());

                    dstManager.AddComponentData(entity, new ProjectileShootingData
                    {
                        projectilePrefab = conversionSystem.GetPrimaryEntity(projectilePrefab),
                        spreadAmount = spreadAmount,
                        projectileSpeed = projectileSpeed

                    });

                    break;
                }

            case ShootingType.Raycast:
                {
                    dstManager.AddComponentData(entity, new RaycastShootingData
                    {
                        impulse = impulse
                    });

                    break;
                }
        }

        if (targetProvider == TargetProvider.None)
            return;

        dstManager.AddComponentData(entity, new TargetProviderData
        {
            distance = distance,
            filter = new CollisionFilter
            {
                BelongsTo = belongsTo.Value,
                CollidesWith = collidesWith.Value
            }
        });

        switch (targetProvider)
        {
            case TargetProvider.Raycast:
                {
                    dstManager.AddComponentData(entity, new RaycastTargetProvider());
                    break;
                }

            case TargetProvider.PointDistance:
                {
                    dstManager.AddComponentData(entity, new DistanceTargetProvider());
                    break;
                }
        }
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(projectilePrefab);
        referencedPrefabs.Add(shootPoint);
    }
}