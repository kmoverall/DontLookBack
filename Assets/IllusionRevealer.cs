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
	public float angleChangeFadeRate = 90f; //Degrees/second of rotation that will cause the view to fade in 1 second

	float delayTimer = 0;
	float currentRadius = 0;
	float currentFade = 1;
	Vector3 prevAngle;

	bool isFading = false;
	bool isFocused = false;

	public Texture2D noiseTexture;

	// Use this for initialization
	void Start () {
		illusionMaterial.SetFloat("_Radius", 0.0f);
		illusionMaterial.SetFloat("_Feather", 0.0f);
		illusionMaterial.SetFloat("_AlphaMod", 1.0f);
		illusionMaterial.SetFloat("_Fade", 1.0f);
		
		illusionMaterial.SetTexture("_NoiseTex", noiseTexture);

		prevAngle = Camera.main.transform.forward;
	}
	
	//Refactor this into a nice state machine, maybe using unity animation
	// Update is called once per frame
	void Update () {

		//Start timing delay on the frame the key is pressed or released
		if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(1)) {
			delayTimer = 0;
		}

		//Only allows reveal to begin if it has not faded
		if (Input.GetMouseButtonDown(1)) {
			isFocused = true;
		}

		//Increase radius of reveal as key is held, after a brief delay, but not if the view is currently fading
		if (Input.GetMouseButton(1) && !isFading && isFocused) {
			Debug.Log("Show");
			delayTimer += Time.deltaTime;

			if (delayTimer > revealDelay) {
				currentRadius += (revealRadius / revealTime) * Time.deltaTime;

				currentFade -= 2 * Vector3.Angle(Camera.main.transform.forward, prevAngle) / (Time.deltaTime * angleChangeFadeRate);
				currentFade += (2 / revealTime) * Time.deltaTime;
			}
		}
		//Reduce radius of reveal when key has been let go
		else if ((!Input.GetMouseButton(1) && currentFade > -1) || isFading) {
			Debug.Log("Fade");
			delayTimer += Time.deltaTime;
			isFading = true; 

			if (delayTimer > fadeDelayTime) {
				currentFade -= (2 / fadeTime) * Time.deltaTime;
			}
		}


		//Debug.Log(currentFade);

		currentRadius = Mathf.Clamp(currentRadius, 0, revealRadius);
		currentFade = Mathf.Clamp(currentFade, -1, 1);

		//Reset values once fading is complete
		if (currentFade == -1) {
			isFading = false;
			isFocused = false;
			currentRadius = 0;
			currentFade = 1;
		}

		illusionMaterial.SetFloat("_Radius", currentRadius);
		illusionMaterial.SetFloat("_Feather", currentRadius * (featherRadius / revealRadius));
		illusionMaterial.SetFloat("_Fade", currentFade);

		prevAngle = Camera.main.transform.forward;
		
	}
}
