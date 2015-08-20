using UnityEngine;
using System.Collections;

public class NoiseGenerator {
	public static Texture2D Simplex2D(int width, int height, float scale, NoiseStyle style, float min, float max) {
		Texture2D noiseTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);

		Noise noise = new Noise();
		noise.addChannel(new Channel("Color", Algorithm.Simplex2d, scale, style, min, max, Edge.Smooth));
		
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Vector3 vector = new Vector3((float)x, 0.0f, (float)y);

				float sample = noise.getNoise(vector, "Color");
				noiseTexture.SetPixel(x, y, new Color(sample, sample, sample, 1.0f));
			}
		}

		noiseTexture.Apply();
		return noiseTexture;
	}
}
