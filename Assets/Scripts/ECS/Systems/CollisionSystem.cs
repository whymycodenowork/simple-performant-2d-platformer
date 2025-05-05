using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct CollisionSystem : ISystem
{
    struct StaticCollider { public float2 min, max; }

    public void OnUpdate(ref SystemState state)
    {
        var staticList = new NativeList<StaticCollider>(Allocator.TempJob);
        foreach (var (transform, hitbox) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<HitboxComponent>>()
                                         .WithAll<GroundComponent>())
        {
            float2 pos = transform.ValueRO.Position.xy;
            float2 half = hitbox.ValueRO.size * 0.5f;
            staticList.Add(new StaticCollider { min = pos - half, max = pos + half });
        }
        var statics = staticList.ToArray(Allocator.TempJob);
        staticList.Dispose();

        var job = new CollisionJob { statics = statics };
        var handle = job.ScheduleParallel(state.Dependency);

        // Clean up
        handle.Complete();
        statics.Dispose();
        state.Dependency = handle;
    }

    [BurstCompile]
    private partial struct CollisionJob : IJobEntity
    {
        [ReadOnly] public NativeArray<StaticCollider> statics;

        public void Execute(
            ref LocalTransform dynTransform,
            ref HitboxComponent dynHitbox,
            ref VelocityComponent velocity,
            ref PlayerMovementComponent move)
        {
            // Compute dynamic AABB once
            float2 dynPos = dynTransform.Position.xy;
            float2 dynHalf = dynHitbox.size * 0.5f;
            float2 dynMin = dynPos - dynHalf;
            float2 dynMax = dynPos + dynHalf;

            // Reset contact flags
            move.Left = move.Right = move.Grounded = move.Ceiling = false;

            float2 bestMTV = float2.zero;
            float bestDist = float.MaxValue;
            bool bestIsVert = false;

            // Narrowphase: test against all cached statics
            for (int i = 0; i < statics.Length; i++)
            {
                var s = statics[i];

                // AABB overlap check
                if (dynMax.x <= s.min.x || dynMin.x >= s.max.x ||
                    dynMax.y <= s.min.y || dynMin.y >= s.max.y)
                    continue;

                // Compute penetration on each axis
                float left = s.max.x - dynMin.x;
                float right = dynMax.x - s.min.x;
                float down = s.max.y - dynMin.y;
                float up = dynMax.y - s.min.y;

                float2 mtv;
                bool isVert;

                if (math.min(left, right) < math.min(down, up))
                {
                    mtv = new float2((left < right ? -left : right), 0);
                    isVert = false;
                }
                else
                {
                    mtv = new float2(0, (down < up ? -down : up));
                    isVert = true;
                }

                float d = mtv.x * mtv.x + mtv.y * mtv.y;
                if (d < bestDist)
                {
                    bestDist = d;
                    bestMTV = mtv;
                    bestIsVert = isVert;
                }
            }
            
            // Resolve the best collision if any
            if (bestDist < float.MaxValue)
            {
                dynTransform.Position.xy -= bestMTV;

                if (bestIsVert)
                {
                    velocity.velocity.y = 0;
                    move.Grounded = bestMTV.y > 0;
                    move.Ceiling = bestMTV.y < 0;
                }
                else
                {
                    velocity.velocity.x = 0;
                    move.Left = bestMTV.x > 0;
                    move.Right = bestMTV.x < 0;
                }
            }
        }
    }
}
