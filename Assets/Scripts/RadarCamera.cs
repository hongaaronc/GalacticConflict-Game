using UnityEngine;
using System.Collections;

public class RadarCamera : MonoBehaviour {
    Camera radarCam;
    public Shader hologramShader;
    public float inside;
    public float rim;
    public float strength;
	// Use this for initialization
	void Start () {
        radarCam = GetComponent<Camera>();
        Shader.SetGlobalFloat("_Inside", inside);
        Shader.SetGlobalFloat("_Rim", rim);
        Shader.SetGlobalFloat("_Strength", strength);
        radarCam.SetReplacementShader(hologramShader, "ReplacementTag");
	}
	
	// Update is called once per frame
	void Update () {
        radarCam.RenderWithShader(hologramShader, "ReplacementTag");
        Shader.SetGlobalFloat("_Inside", inside);
        Shader.SetGlobalFloat("_Rim", rim);
        Shader.SetGlobalFloat("_Strength", strength);
	}
}
