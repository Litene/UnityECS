using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ComponentsAndTags {
	public readonly partial struct GraveyardAspect : IAspect {
		public readonly Entity Entity;

		private readonly RefRO<LocalTransform> _localTransform;

		private readonly RefRO<GraveyardProperties> _graveyardProperties;
		private readonly RefRW<GraveyardRandom> _graveyardRandom;

		public int NumberToSpawn => _graveyardProperties.ValueRO.NumberOfTombstones;
		public Entity TombStonePrefab => _graveyardProperties.ValueRO.TombstonePrefab;


		public LocalTransform GetRandomTombstoneTransform() {
			return new LocalTransform {
				Position = GetRandomPosition(),
				Rotation = GetRandomRotation(),
				Scale = GetRandomScale(0.2f)
			};
		}

		private float3 GetRandomPosition() {
			float3 randomPosition;
			do {
				randomPosition = _graveyardRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
			} while (math.distancesq(_localTransform.ValueRO.Position, randomPosition) <= BrainSafetyRadiusSQ);

			return randomPosition;
		}

		private float3 MinCorner => _localTransform.ValueRO.Position - HalfDimensions;
		private float3 MaxCorner => _localTransform.ValueRO.Position + HalfDimensions;

		private float3 HalfDimensions => new() {
			x = _graveyardProperties.ValueRO.FieldDimensions.x * 0.5f,
			y = 0f,
			z = _graveyardProperties.ValueRO.FieldDimensions.y * 0.5f
		};

		private const float BrainSafetyRadiusSQ = 100f;

		private quaternion GetRandomRotation() => quaternion.RotateY(_graveyardRandom.ValueRW.Value.NextFloat(-0.24f, 0.25f));
		private float GetRandomScale(float min) => _graveyardRandom.ValueRW.Value.NextFloat(min, 1.0f);
	}
}