using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct CollisionSystem : ISystem
{
    public readonly void OnUpdate(ref SystemState state)
    {
        // Query for entities
        foreach ((RefRW<LocalTransform> transform1, RefRO<HitboxComponent> hitbox1, _, Entity entity1) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<HitboxComponent>, RefRO<PlayerMovementComponent>>().WithEntityAccess())
        {
            foreach ((RefRW<LocalTransform> transform2, RefRO<HitboxComponent> hitbox2, _, Entity entity2) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<HitboxComponent>, RefRO<GroundComponent>>().WithEntityAccess())
            {
                // Skip checking the same entity
                if (entity1 == entity2) continue;

                // Check for collision between entity1 and entity2
                float2 collisionNormal = CheckCollision(transform1.ValueRW, hitbox1.ValueRO, transform2.ValueRW, hitbox2.ValueRO);

                if (!math.all(collisionNormal == float2.zero))
                {
                    transform1.ValueRW.Position.x += collisionNormal.x; // Adjust position based on collision normal
                    transform1.ValueRW.Position.y += collisionNormal.y;
                    Debug.Log($"Collision detected between entity {entity1} and entity {entity2}, Normal: {collisionNormal}");
                }
            }
        }
    }

    public readonly float2 CheckCollision(LocalTransform transform1, HitboxComponent hitbox1, LocalTransform transform2, HitboxComponent hitbox2)
    {
        // Calculate the AABBs of both entities
        float2 min1 = transform1.Position.xy - hitbox1.size / 2;
        float2 max1 = transform1.Position.xy + hitbox1.size / 2;

        float2 min2 = transform2.Position.xy - hitbox2.size / 2;
        float2 max2 = transform2.Position.xy + hitbox2.size / 2;

        // Check for overlap on both axes (X and Y)
        bool xOverlap = min1.x < max2.x && max1.x > min2.x;
        bool yOverlap = min1.y < max2.y && max1.y > min2.y;

        // If no overlap in either axis, no collision
        if (!xOverlap || !yOverlap)
        {
            return float2.zero;
        }

        // Calculate the overlap amounts for both axes
        float xOverlapAmount = Mathf.Min(max1.x - min2.x, max2.x - min1.x);
        float yOverlapAmount = Mathf.Min(max1.y - min2.y, max2.y - min1.y);

        // Determine the collision normal based on the smallest overlap
        float2 normal = float2.zero;

        if (xOverlapAmount < yOverlapAmount)
        {
            normal.x = (max1.x > max2.x) ? 1 : -1;
        }
        else
        {
            normal.y = (max1.y > max2.y) ? 1 : -1;
        }

        // Return the collision normal (float2.zero means no collision)
        return normal;
    }
}
