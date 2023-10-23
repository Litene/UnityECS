using ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor.SceneManagement;

namespace Systems {

	[BurstCompile] [UpdateAfter(typeof(SpawnZombieSystem))] [UpdateBefore(typeof(TransformSystemGroup))]
	public partial struct ZombieRiseSystem : ISystem {

		[BurstCompile] public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
		}

		[BurstCompile] public void OnUpdate(ref SystemState state) {
			var deltaTime = SystemAPI.Time.DeltaTime;
			var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
			new ZombieRiseJob {
				DeltaTime = deltaTime,
				ECB = ecb.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
			}.ScheduleParallel();
		}
	}

	[BurstCompile] public partial struct ZombieRiseJob : IJobEntity {

		public float DeltaTime;
		public EntityCommandBuffer.ParallelWriter ECB;

		[BurstCompile] public void Execute(ZombieRiseAspect zombie, [EntityIndexInQuery] int sortKey) {
			zombie.Rise(DeltaTime);

			if (!zombie.IsAboveGround) return;

			ECB.RemoveComponent<ZombieProperties.RiseRate>(sortKey, zombie.Entity);
			ECB.SetComponentEnabled<ZombieProperties.Walk>(sortKey, zombie.Entity, true);
		}

	}
}