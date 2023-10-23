using ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems {
    [BurstCompile][UpdateInGroup(typeof(InitializationSystemGroup))] public partial struct InitializeZombieSystem : ISystem {
        [BurstCompile] public void OnCreate(ref SystemState state) {
        }

        [BurstCompile] public void OnUpdate(ref SystemState state) {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var zombie in SystemAPI.Query<ZombieWalkAspect>().WithAll<ZombieProperties.Tag>()) {
                ecb.RemoveComponent<ZombieProperties.Tag>(zombie.Entity);
                ecb.SetComponentEnabled<ZombieProperties.Walk>(zombie.Entity, false);
                
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
}