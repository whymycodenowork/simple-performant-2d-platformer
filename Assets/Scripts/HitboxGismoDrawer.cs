using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

[ExecuteAlways]
public class HitboxGizmoDrawer : MonoBehaviour
{
    void OnDrawGizmos()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null || !world.IsCreated) return;

        var entityManager = world.EntityManager;
        var query = entityManager.CreateEntityQuery(
            ComponentType.ReadOnly<LocalTransform>(),
            ComponentType.ReadOnly<HitboxComponent>()
        );

        var transforms = query.ToComponentDataArray<LocalTransform>(Allocator.Temp);
        var hitboxes = query.ToComponentDataArray<HitboxComponent>(Allocator.Temp);

        Gizmos.color = Color.green;
        for (int i = 0; i < transforms.Length; i++)
        {
            float3 pos = transforms[i].Position;
            float2 size = hitboxes[i].size;
            Vector3 center = new (pos.x, pos.y, 0f);
            Vector3 half = new Vector3(size.x, size.y, 0f) * 0.5f;

            Gizmos.DrawWireCube(center, half * 2f);
        }

        transforms.Dispose();
        hitboxes.Dispose();
    }
}
