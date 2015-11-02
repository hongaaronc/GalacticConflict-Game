using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class HUDChromaticAbberation : MonoBehaviour {
    public CustomVignetteAndChromaticAbberation vignetteAndChrom;
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
            vignetteAndChrom.chromaticAberration = baseAberation;
            test = false;
        }
    }

	// Use this for initialization
	void Start () {
        vignetteAndChrom.chromaticAberration = baseAberation;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1)) {
            distort(testAberation);
        }
        changeVelocity *= 1f - dampening * Time.deltaTime;
        changeVelocity += springiness * (baseAberation - vignetteAndChrom.chromaticAberration) * Time.deltaTime;
        changeVelocity += Random.Range(-jitterAberation, jitterAberation) * Time.deltaTime;
        vignetteAndChrom.chromaticAberration += changeVelocity;
        vignetteAndChrom.intensity = baseVignette + Random.Range(-jitterVignette, jitterVignette);
	}

    public void distort(float value)
    {
        vignetteAndChrom.chromaticAberration += value;
    }
}
