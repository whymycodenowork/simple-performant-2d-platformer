using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct GravitySystem : ISystem
{
    public readonly void OnUpdate(ref SystemState state)
    {
        float gravity = -0.2f; // Gravitational acceleration

        // Iterate through all entities with both VelocityComponent and GravityComponent
        foreach ((RefRW<VelocityComponent> velocityComp, RefRO<GravityComponent> gravityComp) in SystemAPI.Query<RefRW<VelocityComponent>, RefRO<GravityComponent>>())
        {
            // Apply the gravity force to the y-component of the velocity (vertical movement)
            velocityComp.ValueRW.velocity.y += gravity * gravityComp.ValueRO.gravityMultiplier;
            velocityComp.ValueRW.velocity.y *= 0.99f; // Apply damping to the y-component of the velocity
        }
    }
}