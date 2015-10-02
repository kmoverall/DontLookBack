using UnityEngine;
using System.Collections;

public class RenderTextureCorrection : MonoBehaviour {

	public RenderTexture texture;
	Camera renderTexCamera;
	
	void Start () {
		texture.height = Screen.height;
		texture.width = Screen.width;

		//Weird hack to deal with a unity bug involving incorrect rendering to the render texture
		renderTexCamera = gameObject.GetComponent<Camera>();
		renderTexCamera.targetTexture = null;
		renderTexCamera.targetTexture = texture;
	}
}
