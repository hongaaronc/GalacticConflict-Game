using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {
    public float duration = 1.0f;
    public float endIntensity = 0.0f;
    public float endRange = 0.0f;

    private Light myLight;
    private float startTime;
    private float startRange;
    private float startIntensity;
	// Use this for initialization
	void Start () {
        myLight = GetComponent<Light>();
        startTime = Time.time;
        startIntensity = myLight.intensity;
        startRange = myLight.range;
	}
	
	// Update is called once per frame
	void Update () {
        myLight.intensity = Mathf.Lerp(startIntensity, endIntensity, (Time.time - startTime) / duration);
        myLight.range = Mathf.Lerp(startRange, endRange, (Time.time - startTime) / duration);
        if (Time.time - startTime >= duration)
            Destroy(gameObject);
	}
}
