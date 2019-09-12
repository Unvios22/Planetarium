using System;
using System.Collections.Generic;

namespace ReadOnlyData {
	public static class PhysicsLayers {
		public enum Layers {
			Default = 0,
			TransparentFX = 1,
			IgnoreRaycast = 2,
			Water = 4,
			UI = 5,
			Ground = 8,
			Seeds = 9
		}

		private static Dictionary<int, string> LayerNamesMap = new Dictionary<int, string>() {
			[(int)Layers.Default] = "Default",
			[(int)Layers.TransparentFX] = "TransparentFX",
			[(int)Layers.IgnoreRaycast] = "IgnoreRaycast",
			[(int)Layers.Water] = "Water",
			[(int)Layers.UI] = "UI",
			[(int)Layers.Ground] = "Ground",
			[(int)Layers.Seeds] = "Seeds"
		};

		public static string GetLayerName(Layers layer) {
			return LayerNamesMap[(int) layer];
		}

		public static int GetLayerMask(int[] fromLayers) {
			//TODO: Add simplified method to get LayerMask.GetMask() value
			throw new NotImplementedException();
		}

	}
}