using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public struct VelocityComponent : IComponentData
{
    public float2 velocity;
}