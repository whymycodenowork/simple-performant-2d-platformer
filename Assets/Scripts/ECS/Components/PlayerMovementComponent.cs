using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public struct PlayerMovementComponent : IComponentData
{
    public float moveSpeed;
    public float jumpForce;
    public bool isCrouching;
    public bool canJump;
}