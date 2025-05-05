using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public struct HitboxComponent : IComponentData
{
    public float2 size; // Width and height of the collider
}
