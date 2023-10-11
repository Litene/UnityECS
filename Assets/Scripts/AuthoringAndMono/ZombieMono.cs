using ComponentsAndTags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono {
	public class ZombieMono : MonoBehaviour {
		public float RiseRate;
	}

	public class ZombieBaker : Baker<ZombieMono> {

		public override void Bake(ZombieMono authoring) {
			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new ZombieProperties.RiseRate{
				Value = authoring.RiseRate
			});
			
			// AddComponent(entity, new ZombieProperties.Walk {
			// 	
			// });
			
		}
	}
}