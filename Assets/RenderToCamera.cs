using UnityEngine;
using System.Collections;

public class RenderToCamera : MonoBehaviour {

	public Material illusionMat;
	public Material realMat;
	public RenderTexture illusionTex;
	public RenderTexture realTex;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		Graphics.Blit(illusionTex, null, illusionMat);
		Graphics.Blit(realTex, null, realMat);
	}
}
