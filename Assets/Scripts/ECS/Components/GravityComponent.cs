using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public struct GravityComponent : IComponentData
{
    public float mass;
}