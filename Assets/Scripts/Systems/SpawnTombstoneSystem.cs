using ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems {
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	[BurstCompile] public partial struct SpawnTombstoneSystem : ISystem {
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<GraveyardProperties>();
		}

		[BurstCompile] public void OnUpdate(ref SystemState state) {
			state.Enabled = false;
			var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
			var graveyard = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);

			var ecb = new EntityCommandBuffer(Allocator.Temp);
			
			var blobBuilder = new BlobBuilder(Allocator.Temp);
			ref var spawnPoints = ref blobBuilder.ConstructRoot<ZombieSpawnsPointsBlob>();
			var arrayBuilder = blobBuilder.Allocate(ref spawnPoints.Value, graveyard.NumberToSpawn);
			
			var tombStoneOFFset = new float3(0, -2f, 1f);
			
			
			for (int i = 0; i < graveyard.NumberToSpawn; i++) {
				var newEntity = ecb.Instantiate(graveyard.TombStonePrefab);
				var tombStone = graveyard.GetRandomTombstoneTransform();
				ecb.SetComponent(newEntity, new LocalTransform{
					Position = tombStone.Position,
                    Scale = tombStone.Scale,
                    Rotation = tombStone.Rotation});
				var newZombieSpawnPoint = tombStone.Position + tombStoneOFFset;
				arrayBuilder[i] = newZombieSpawnPoint;
			}

			var blobAsset = blobBuilder.CreateBlobAssetReference<ZombieSpawnsPointsBlob>(Allocator.Persistent);
			ecb.SetComponent(graveyardEntity, new ZombieSpawnPoints{Value = blobAsset});
			blobBuilder.Dispose();
			ecb.Playback(state.EntityManager);
		}
	}
}