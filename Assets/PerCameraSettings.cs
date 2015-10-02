using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]
[ExecuteInEditMode]
public class PerCameraSettings : MonoBehaviour {

	public bool fog;
	public Color fogColor;
	public float fogDensity;
	public Color ambientLight;
	public float haloStrength;
	public float flareStrength;

	bool previousFog;
	Color previousFogColor;
	float previousFogDensity;
	Color previousAmbientLight;
	float previousHaloStrength;
	float previousFlareStrength;

	void OnPreRender() {
		previousFog = RenderSettings.fog;
		previousFogColor = RenderSettings.fogColor;
		previousFogDensity = RenderSettings.fogDensity;
		previousAmbientLight = RenderSettings.ambientLight;
		previousHaloStrength = RenderSettings.haloStrength;
		previousFlareStrength = RenderSettings.flareStrength;

		RenderSettings.ambientLight = ambientLight;
		RenderSettings.haloStrength = haloStrength;
		RenderSettings.flareStrength = flareStrength;
		if (fog) {
			RenderSettings.fog = fog;
			RenderSettings.fogColor = fogColor;
			RenderSettings.fogDensity = fogDensity;
		}
	}

	void OnPostRender() {
		RenderSettings.fog = previousFog;
		RenderSettings.fogColor = previousFogColor;
		RenderSettings.fogDensity = previousFogDensity;
		RenderSettings.ambientLight = previousAmbientLight;
		RenderSettings.haloStrength = previousHaloStrength;
		RenderSettings.flareStrength = previousFlareStrength;
	}
}
