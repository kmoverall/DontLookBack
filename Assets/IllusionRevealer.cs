using UnityEngine;
using System.Collections;

public class IllusionRevealer : MonoBehaviour {
	public Material illusionMaterial;
	public float revealRadius = 0.5f; // Radius is defined as % of vertical screen space
	public float featherRadius = 0.25f;
	public float revealDelay = 0.5f; 
	public float revealTime = 1.0f;
	public float fadeTime = 1.0f;
	public float fadeDelayTime = 0.5f;

	float delayTimer = 0;
	float currentRadius = 0;
	float currentFade = 1;

	bool isFading = false;

	public Texture2D noiseTexture;

	// Use this for initialization
	void Start () {
		illusionMaterial.SetFloat("_Radius", 0.0f);
		illusionMaterial.SetFloat("_Feather", 0.0f);
		illusionMaterial.SetFloat("_AlphaMod", 1.0f);
		illusionMaterial.SetFloat("_Fade", 1.0f);
		
		illusionMaterial.SetTexture("_NoiseTex", noiseTexture);
	}
	
	// Update is called once per frame
	void Update () {

		//Start timing delay on the frame the key is pressed or released
		if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1)) {
			delayTimer = 0;
		}

		//Increase radius of reveal as key is held, after a brief delay, but not if the view is currently fading
		if (Input.GetMouseButton(1) && currentRadius < revealRadius && !isFading) {
			delayTimer += Time.deltaTime;

			if (delayTimer > revealDelay) {
				currentRadius += (revealRadius / revealTime) * Time.deltaTime;
			}
		}
		//Reduce radius of reveal when key has been let go
		else if ((!Input.GetMouseButton(1) && currentFade > -1) || isFading) {
			delayTimer += Time.deltaTime;
			isFading = true; 

			if (delayTimer > fadeDelayTime) {
				currentFade -= (2 / fadeTime) * Time.deltaTime;
			}
		}

		currentRadius = Mathf.Clamp(currentRadius, 0, revealRadius);
		currentFade = Mathf.Clamp(currentFade, -1, 1);

		//Reset values once fadine is complete
		if (currentFade == -1) {
			isFading = false;
			currentRadius = 0;
			currentFade = 1;
		}

		illusionMaterial.SetFloat("_Radius", currentRadius);
		illusionMaterial.SetFloat("_Feather", currentRadius * (featherRadius / revealRadius));
		illusionMaterial.SetFloat("_Fade", currentFade);
		
	}
}
