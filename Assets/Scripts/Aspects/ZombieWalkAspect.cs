using Unity.Entities;
using Unity.Transforms;
using UnityEngine.Rendering;

namespace ComponentsAndTags {
	public readonly partial struct ZombieWalkAspect : IAspect {
		public readonly Entity Entity;

		private readonly RefRW<LocalTransform> _localTransform;
		private readonly RefRW<ZombieProperties.Timer> _walkTimer;
		private readonly RefRO<ZombieProperties.Walk> _zombieWalk;
		private readonly RefRO<ZombieProperties.Heading> _zombieHeading;

		private float WalkSpeed => _zombieWalk.ValueRO.WalkSpeed;
		private float WalkAmplitude => _zombieWalk.ValueRO.WalkAmplitude;
		private float WalkFrequency => _zombieWalk.ValueRO.WalkFrequency;
		private float Heading => _zombieHeading.ValueRO.Value;

		private float WalkTimer {
			get => _walkTimer.ValueRO.Value;
			set => _walkTimer.ValueRW.Value = value;
		}

		public void Walk(float deltaTime) {
			WalkTimer += deltaTime;
			_localTransform.ValueRW.Position += _localTransform.ValueRO.Forward() * WalkSpeed * deltaTime;
		}
	}
}