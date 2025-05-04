using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct GravitySystem : ISystem
{
    public readonly void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float gravity = -1f; // Gravitational acceleration

        // Iterate through all entities with both VelocityComponent and GravityComponent
        foreach ((RefRW<VelocityComponent> velocityComp, RefRO<GravityComponent> gravityComp) in SystemAPI.Query<RefRW<VelocityComponent>, RefRO<GravityComponent>>())
        {
            // Calculate gravity force for the entity
            float force = gravity * gravityComp.ValueRO.mass * deltaTime;

            // Apply the gravity force to the y-component of the velocity (vertical movement)
            velocityComp.ValueRW.velocity.y += -10;// force;
        }
    }
}