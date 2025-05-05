using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

//[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ApplyVelocitySystem : ISystem
{
    public readonly void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<VelocityComponent> movement, RefRW<LocalTransform> transform) in
                 SystemAPI.Query<RefRW<VelocityComponent>, RefRW<LocalTransform>>())
        {
            movement.ValueRW.velocity = new float2(movement.ValueRW.velocity.x * 0.90f, movement.ValueRW.velocity.y);
            transform.ValueRW.Position.xy += movement.ValueRW.velocity * SystemAPI.Time.DeltaTime;
            Debug.Log($"current velocity: {movement.ValueRW.velocity}");
        }
    }
}
