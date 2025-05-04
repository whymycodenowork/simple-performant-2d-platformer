using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    public readonly void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerMovementComponent>();
    }

    public readonly void OnUpdate(ref SystemState state)
    {
        (bool left, bool right, bool jump) = GetInput();
        foreach ((RefRW<PlayerMovementComponent> movement, RefRW<VelocityComponent> velocityComponent) in SystemAPI.Query<RefRW<PlayerMovementComponent>, RefRW<VelocityComponent>>())
        {
            float2 velocity = float2.zero;

            if (jump && movement.ValueRO.canJump)
            {
                velocity.y += movement.ValueRO.jumpForce;
                Debug.Log("Jumping!");
            }
            if (left)
            {
                velocity.x -= movement.ValueRO.moveSpeed;
            }

            if (right)
            {
                velocity.x += movement.ValueRO.moveSpeed;
            }

            velocityComponent.ValueRW.velocity += velocity;
        }
    }

    private readonly (bool left, bool right, bool jump) GetInput()
    {
        return (
            left: Input.GetKey(KeyCode.LeftArrow),
            right: Input.GetKey(KeyCode.RightArrow),
            jump: Input.GetKeyDown(KeyCode.UpArrow)
        );
    }
}
