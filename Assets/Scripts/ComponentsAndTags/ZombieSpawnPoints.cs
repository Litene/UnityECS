using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace ComponentsAndTags {
	public struct ZombieSpawnPoints : IComponentData {
		public BlobAssetReference<ZombieSpawnsPointsBlob> Value;
	}

	public struct ZombieSpawnsPointsBlob {
		public BlobArray<float3> Value;
	}
}