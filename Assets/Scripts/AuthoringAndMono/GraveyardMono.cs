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
		public uint RandomSeed;
	}

	public class GraveyardBaker : Baker<GraveyardMono> {

		public override void Bake(GraveyardMono authoring) {
			AddComponent(new GraveyardProperties {
				FieldDimensions = authoring.FieldDimensions,
				NumberOfTombstones = authoring.NumbersOfTombStonesToSpawn,
				TombstonePrefab = GetEntity(authoring.GravePrefab)
			});
			AddComponent(new GraveyardRandom {
				Value = Random.CreateFromIndex(authoring.RandomSeed)
			});
		}
	}
}