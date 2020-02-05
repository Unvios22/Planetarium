using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReadOnlyData {
	public static class PhysicsLayers {
		public enum Layers {
			Default = 0,
			TransparentFX = 1,
			IgnoreRaycast = 2,
			Water = 4,
			UI = 5,
			Ground = 8,
			Seeds = 9,
			Boids = 10,
			Debug = 30
		}

		private static Dictionary<int, string> LayerNamesMap = new Dictionary<int, string>() {
			[(int)Layers.Default] = "Default",
			[(int)Layers.TransparentFX] = "TransparentFX",
			[(int)Layers.IgnoreRaycast] = "IgnoreRaycast",
			[(int)Layers.Water] = "Water",
			[(int)Layers.UI] = "UI",
			[(int)Layers.Ground] = "Ground",
			[(int)Layers.Seeds] = "Seeds",
			[(int)Layers.Boids] = "Boids",
			[(int)Layers.Debug] = "Debug"
		};

		public static string GetLayerName(Layers layer) {
			return LayerNamesMap[(int) layer];
		}

		public static int GetLayerMask(int[] fromLayers) {
			throw new NotImplementedException();
		}
		
		public static int GetLayerMask(Layers[] fromLayers) {
			var layersNames = new string[fromLayers.Length];
			for (int i = 0; i < fromLayers.Length; i++) {
				layersNames[i] = GetLayerName(fromLayers[i]);
			}
			return LayerMask.GetMask(layersNames);
		}

	}
}