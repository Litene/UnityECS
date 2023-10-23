using Unity.Entities;

namespace ComponentsAndTags {
	public static class ZombieProperties {
		public struct Walk : IComponentData, IEnableableComponent {
			public float WalkSpeed;
			public float WalkAmplitude;
			public float WalkFrequency;

		}
		public struct Timer : IComponentData {
			public float Value;
		}

		public struct RiseRate : IComponentData {
			public float Value;
		}
		
		public struct Heading : IComponentData {
			public float Value;
		}

		public struct Tag : IComponentData {}
	}
}