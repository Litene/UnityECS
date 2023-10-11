using ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;

namespace Systems {
	[BurstCompile] public partial struct SpawnZombieSystem : ISystem {
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state) {
			var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
			new SpawnZombieJob {
				DeltaTime = SystemAPI.Time.DeltaTime,
				ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
			}.Run();
		}
	}

	[BurstCompile] public partial struct SpawnZombieJob : IJobEntity {
		public float DeltaTime;
		public EntityCommandBuffer ECB;

		private void Execute(GraveyardAspect graveyard) {
			graveyard.ZombieSpawnTimer -= DeltaTime;
			if (!graveyard.TimeToSpawnZombie) return;
			if (!graveyard.ZombieSpawnPointInitialized()) return;


			graveyard.ZombieSpawnTimer = graveyard.ZombieSpawnRate;
			var newZombie = ECB.Instantiate(graveyard.ZombiePrefab);

			var newZombieTf = graveyard.GetZombieSpawnPoint();
			ECB.SetComponent(newZombie, newZombieTf);
		}
	}
}