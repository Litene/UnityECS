using ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Systems {
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	[BurstCompile] public partial struct SpawnTombstoneSystem : ISystem {
		
		[BurstCompile] public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<GraveyardProperties>();
		}

		[BurstCompile] public void OnDestroy(ref SystemState state) {
		
		}

		[BurstCompile] public void OnUpdate(ref SystemState state) {
			state.Enabled = false;
			var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
			var graveyard = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);

			var ecb = new EntityCommandBuffer(Allocator.Temp);
			
			for (int i = 0; i < graveyard.NumberToSpawn; i++) {
				var newEntity = ecb.Instantiate(graveyard.TombStonePrefab);
				var tombStone = graveyard.GetRandomTombstoneTransform();
				ecb.SetComponent(newEntity, new LocalTransform{
					Position = tombStone.Position,
                    Scale = tombStone.Scale,
                    Rotation = tombStone.Rotation});
			}
			
			ecb.Playback(state.EntityManager);
		}
	}
}