using UnityEngine;
using System.Collections;

public enum IllusionRevealerState { Faded, RevealDelay, Revealing, Revealed, Fading };

public class IllusionRevealer : MonoBehaviour {
	public Material illusionMaterial;
	[SerializeField] 
	float revealRadius = 0.5f; // Radius is defined as % of vertical screen space

	[SerializeField] 
	float featherRadius = 0.25f;

	[SerializeField] 
	float angleChangeFadeRate = 360f; //Degrees/second of rotation that will cause the view to fade in 1 second

	[SerializeField]
	float angleChangeDeadZone = 90f; //How many degrees/second of rotation is allowed before fading kicks in?

	[SerializeField] 
	float delayTime = 1.0f;

	[SerializeField]
	float fadeInTime = 3.0f;

	[SerializeField]
	float fadeOutTime = 0.5f;


	float timerStart = 0;
	float currentRadius = 0;
	float currentFade = 1;
	IllusionRevealerState state;
	Vector3 prevAngle;
	float angleChangeRate;

	public Texture2D noiseTexture;

	// Use this for initialization
	void Start () {
		illusionMaterial.SetFloat("_Radius", 0.0f);
		illusionMaterial.SetFloat("_Feather", 0.0f);
		illusionMaterial.SetFloat("_AlphaMod", 1.0f);
		illusionMaterial.SetFloat("_Fade", 1.0f);
		
		illusionMaterial.SetTexture("_NoiseTex", noiseTexture);

		state = IllusionRevealerState.Faded;

		prevAngle = Camera.main.transform.forward;
	}
	
	// Update is called once per frame
	void Update () {
		angleChangeRate = Vector3.Angle(Camera.main.transform.forward, prevAngle) / Time.deltaTime;

		switch (state) {
			case IllusionRevealerState.Faded:
				currentRadius = 0;
				currentFade = 1;
				if (Input.GetMouseButtonDown(1)) {
					timerStart = Time.time;
					state = IllusionRevealerState.RevealDelay;
				}
				break;

			case IllusionRevealerState.RevealDelay:
				if (!Input.GetMouseButton(1)) {
					state = IllusionRevealerState.Fading;
				}
				if (Time.time - timerStart >= delayTime) {
					state = IllusionRevealerState.Revealing;
				}
				break;

			case IllusionRevealerState.Revealing:
				if (Input.GetMouseButtonUp(1)) {
					state = IllusionRevealerState.Fading;
				}
				else {
					currentRadius += (revealRadius / fadeInTime) * Time.deltaTime;
					currentFade += (1 / fadeInTime) * Time.deltaTime;

					if (angleChangeRate > angleChangeDeadZone) {
						state = IllusionRevealerState.Fading;
					}
					else {
						currentFade -= (angleChangeRate / angleChangeFadeRate) * Time.deltaTime * 2;
					}

					currentRadius = Mathf.Clamp(currentRadius, 0, revealRadius);
					currentFade = Mathf.Clamp(currentFade, -1, 1);

					if (currentFade == -1) {
						state = IllusionRevealerState.Faded;
					}
				}
				break;

			case IllusionRevealerState.Fading:
				currentFade -= (1 / fadeOutTime) * Time.deltaTime;
				currentFade = Mathf.Clamp(currentFade, -1, 1);
				if (currentFade == -1) {
					currentRadius = 0;
					state = IllusionRevealerState.Faded;
				}
				break;
		}

		illusionMaterial.SetFloat("_Radius", currentRadius);
		illusionMaterial.SetFloat("_Feather", currentRadius * (featherRadius / revealRadius));
		illusionMaterial.SetFloat("_Fade", currentFade);

		prevAngle = Camera.main.transform.forward;
		
	}
}
