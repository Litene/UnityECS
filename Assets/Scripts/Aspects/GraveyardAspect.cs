using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace ComponentsAndTags {
	public readonly partial struct GraveyardAspect : IAspect {
		public readonly Entity Entity;

		private readonly RefRO<LocalTransform> _localTransform;

		private readonly RefRO<GraveyardProperties> _graveyardProperties;
		private readonly RefRW<GraveyardRandom> _graveyardRandom;
		private readonly RefRW<ZombieSpawnPoints> _zombieSpawnPoints;
		private readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimer;
		public int NumberToSpawn => _graveyardProperties.ValueRO.NumberOfTombstones;
		public Entity TombStonePrefab => _graveyardProperties.ValueRO.TombstonePrefab;

		public BlobArray<float3> ZombieSpawnPoints {
			get => _zombieSpawnPoints.ValueRO.Value.Value.Value;
			set => _zombieSpawnPoints.ValueRW.Value.Value.Value = value;
		}

		public bool ZombieSpawnPointInitialized() {
			return _zombieSpawnPoints.ValueRO.Value.IsCreated && ZombieSpawnPointCount > 0;
		}

		private int ZombieSpawnPointCount => _zombieSpawnPoints.ValueRO.Value.Value.Value.Length;

		public LocalTransform GetRandomTombstoneTransform() {
			return new LocalTransform {
				Position = GetRandomPosition(),
				Rotation = GetRandomRotation(),
				Scale = GetRandomScale(0.2f)
			};
		}

		public LocalTransform GetZombieSpawnPoint() {
			var position = GetRandomZombieSpawnPoint();
			return new LocalTransform {
				Position = position,
				Rotation = quaternion.RotateY(MathHelper.GetHeading(position,_localTransform.ValueRO.Position)),
				Scale = 1.0f
			};
		}

		private float3 GetRandomZombieSpawnPoint() {
			return GetZombieSpawnPoint(_graveyardRandom.ValueRW.Value.NextInt(ZombieSpawnPointCount));
		}

		private float3 GetZombieSpawnPoint(int i) => _zombieSpawnPoints.ValueRO.Value.Value.Value[i];

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

		public float ZombieSpawnRate => _graveyardProperties.ValueRO.ZombieSpawnRate;
		public bool TimeToSpawnZombie => ZombieSpawnTimer <= 0.0f;
		public Entity ZombiePrefab => _graveyardProperties.ValueRO.ZombiePrefab;
		
		public float ZombieSpawnTimer {
			get => _zombieSpawnTimer.ValueRO.Value;
			set => _zombieSpawnTimer.ValueRW.Value = value;
		}

		private const float BrainSafetyRadiusSQ = 100f;

		private quaternion GetRandomRotation() => quaternion.RotateY(_graveyardRandom.ValueRW.Value.NextFloat(-0.24f, 0.25f));
		private float GetRandomScale(float min) => _graveyardRandom.ValueRW.Value.NextFloat(min, 1.0f);
	}
}