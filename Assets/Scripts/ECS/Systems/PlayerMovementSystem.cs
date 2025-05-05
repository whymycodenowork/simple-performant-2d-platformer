using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    public readonly void OnUpdate(ref SystemState state)
    {
        (bool left, bool right, bool jump) = GetInput();
        foreach ((RefRW<VelocityComponent> velocity, RefRO<PlayerMovementComponent> playerMoveComp) in
            SystemAPI.Query<RefRW<VelocityComponent>, RefRO<PlayerMovementComponent>>())
        {
            if (/*playerMoveComp.ValueRO.Grounded && */jump)
            {
                velocity.ValueRW.velocity.y = playerMoveComp.ValueRO.jumpForce; // Jump velocity
            }
            if (left)
            {
                velocity.ValueRW.velocity.x -= playerMoveComp.ValueRO.moveSpeed;
            }
            else if (right)
            {
                velocity.ValueRW.velocity.x += playerMoveComp.ValueRO.moveSpeed;
            }
        }
    }

    private readonly (bool left, bool right, bool jump) GetInput()
    {
        return (
            left: Input.GetKey(KeyCode.A),
            right: Input.GetKey(KeyCode.D),
            jump: Input.GetKeyDown(KeyCode.W)
        );
    }
}
