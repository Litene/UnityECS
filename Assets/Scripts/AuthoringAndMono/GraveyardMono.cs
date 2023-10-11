using ComponentsAndTags;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace AuthoringAndMono {
	public class GraveyardMono : MonoBehaviour {
		public float2 FieldDimensions;
		public int NumbersOfTombStonesToSpawn;
		public GameObject GravePrefab;
		public GameObject ZombiePrefab;
		public uint RandomSeed;
		public float ZombieSpawnRate;
	}

	public class GraveyardBaker : Baker<GraveyardMono> {
		public override void Bake(GraveyardMono authoring) {
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new GraveyardProperties {
				FieldDimensions = authoring.FieldDimensions,
				NumberOfTombstones = authoring.NumbersOfTombStonesToSpawn,
				TombstonePrefab = GetEntity(authoring.GravePrefab, TransformUsageFlags.Dynamic),
				ZombiePrefab = GetEntity(authoring.ZombiePrefab, TransformUsageFlags.Dynamic),
				ZombieSpawnRate = authoring.ZombieSpawnRate
			});
			AddComponent(entity,new GraveyardRandom {
				Value = Random.CreateFromIndex(authoring.RandomSeed)
			});
			AddComponent<ZombieSpawnPoints>(entity);
			AddComponent<ZombieSpawnTimer>(entity);
		}
	}
}