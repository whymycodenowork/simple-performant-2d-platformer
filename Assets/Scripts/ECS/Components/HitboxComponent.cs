using Unity.Entities;
using Unity.Mathematics;

public struct HitboxComponent : IComponentData
{
    public float2 size; // Width and height of the collider (in world units)
}
