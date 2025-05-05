using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerMovementComponent
            {
                moveSpeed = authoring.moveSpeed,
                jumpForce = authoring.jumpForce,
                isCrouching = false,
                canJump = true,
                Left = false,
                Right = false,
                Grounded = false,
                Ceiling = false
            });

            AddComponent(entity, new VelocityComponent
            {
                velocity = new float2(0f, 0f)
            });
            AddComponent<LocalToWorld>(entity);

            AddComponent(entity, new HitboxComponent
            {
                size = new float2(authoring.transform.localScale.x, authoring.transform.localScale.y) // Set the size of the hitbox
            });
            
            AddComponent(entity, new GravityComponent
            {
                gravityMultiplier = 1f
            });
        }
    }
}
