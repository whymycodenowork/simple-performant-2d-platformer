using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class GroundAuthoring : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float jumpForce = 5f;

    // Assign these in the Inspector:
    public Mesh quadMesh;
    public Material material;

    class Baker : Baker<GroundAuthoring>
    {
        public override void Bake(GroundAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

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
                size = new float2(authoring.transform.localScale.x, authoring.transform.localScale.x) // Set the size of the hitbox
            });

            AddComponent(entity, new GroundComponent());
        }
    }
}
