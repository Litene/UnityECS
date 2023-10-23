using ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.iOS;
using UnityEngine.UIElements;

namespace Systems {
    [BurstCompile]
    [UpdateAfter(typeof(ZombieRiseSystem))][UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct ZombieWalkSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var deltaTime = SystemAPI.Time.DeltaTime;
            new ZombieWalkJob {
                DeltaTime = deltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity {
        public float DeltaTime;

        private void Execute(ZombieWalkAspect zombie) {
            zombie.Walk(DeltaTime);
        }
    }
}