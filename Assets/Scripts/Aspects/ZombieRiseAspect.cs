using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ComponentsAndTags {
	public readonly partial struct ZombieRiseAspect : IAspect {

		public readonly Entity Entity;

		private readonly RefRW<LocalTransform> _localTransform;
		private readonly RefRO<ZombieProperties.RiseRate> _zombieRiseRate;
		public bool IsAboveGround => _localTransform.ValueRO.Position.y >= 0f;
		public void Rise(float DeltaTime) {
			if (!IsAboveGround) {
				_localTransform.ValueRW.Position += math.up() * _zombieRiseRate.ValueRO.Value * DeltaTime;
			}
		}


	}
}