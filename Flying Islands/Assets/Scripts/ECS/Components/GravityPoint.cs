using Unity.Entities;

[GenerateAuthoringComponent]
public struct GravityPoint : IComponentData
{
    public float mass;
    public float constForce;
}