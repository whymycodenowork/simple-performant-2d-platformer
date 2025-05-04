using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct ApplyVelocitySystem : ISystem
{
    public readonly void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach ((RefRO<VelocityComponent> movement, RefRW<LocalTransform> transform) in
                 SystemAPI.Query<RefRO<VelocityComponent>, RefRW<LocalTransform>>())
        {
            float3 velocity = new(movement.ValueRO.velocity, 0f);
            transform.ValueRW.Position += velocity * deltaTime;
        }
    }
}
