using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 5f;

    // Assign these in the Inspector:
    public Mesh quadMesh;
    public Material material;

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
                canJump = true
            });

            AddComponent(entity, new VelocityComponent
            {
                velocity = new float2(0f, 0f)
            });
            /*
            AddComponent(entity, new RenderMeshUnmanaged
            {
                mesh = authoring.quadMesh,
                materialForSubMesh = authoring.material
            });
            */
            AddComponent<LocalToWorld>(entity);

            AddComponent(entity, new HitboxComponent
            {
                size = new float2(1f, 1f) // Set the size of the hitbox (in world units)
            });
            /*
            AddComponent(entity, new GravityComponent
            {
                mass = 1f
            });*/
        }
    }
}
