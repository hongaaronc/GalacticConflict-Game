using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class HUDChromaticAbberation : MonoBehaviour {
    public CustomVignetteAndChromaticAbberation effect;
    public float baseAberation = 2f;
    public float testAberation;
    public bool test = false;
    private float changeVelocity = 0f;
    public float springiness = 100f;
    public float dampening = 10f;
    public bool playInEditMode = false;
    public float jitterAberation = 10f;
    public float baseVignette = -1f;
    public float jitterVignette = 0.2f;

    void OnValidate()
    {
        if (test)
        {
            effect.chromaticAberration = baseAberation;
            test = false;
        }
    }

	// Use this for initialization
	void Start () {
        effect.chromaticAberration = baseAberation;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1)) {
            effect.chromaticAberration = testAberation;
        }
        changeVelocity *= 1f - dampening * Time.deltaTime;
        changeVelocity += springiness * (baseAberation - effect.chromaticAberration) * Time.deltaTime;
        changeVelocity += Random.Range(-jitterAberation, jitterAberation) * Time.deltaTime;
        effect.chromaticAberration += changeVelocity;
        effect.intensity = baseVignette + Random.Range(-jitterVignette, jitterVignette);
	}
}
