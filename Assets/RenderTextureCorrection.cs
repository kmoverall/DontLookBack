using UnityEngine;
using System.Collections;

public class RenderTextureCorrection : MonoBehaviour {

	public RenderTexture[] textures; 
	
	// Update is called once per frame
	void Start () {
		foreach (RenderTexture rt in textures) {
			rt.height = Screen.height;
			rt.width = Screen.width;
		}
	}
}
