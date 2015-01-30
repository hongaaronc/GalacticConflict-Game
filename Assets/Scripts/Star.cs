using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	public float range = 100f;
    public float parallaxMult = 1f;

    private Vector3 cameraLastPosition;
	// Use this for initialization
	void Start () {
        parallaxMult = Random.Range(0.2f, 1.0f);
        GetComponent<LensFlare>().brightness = Random.Range(0.8f, 3f) + 1.5f * parallaxMult;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position -= (parallaxMult - 1f) * (Camera.main.transform.position - cameraLastPosition);
        if (transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position -= 2f * new Vector3(Camera.main.orthographicSize * Screen.width / Screen.height,0f,0f);
            parallaxMult = Random.Range(0.2f, 1.0f);
            GetComponent<LensFlare>().brightness = Random.Range(0.8f, 3f) + 1.5f * parallaxMult;
        }
        else if (transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position += 2f * new Vector3(Camera.main.orthographicSize * Screen.width / Screen.height, 0f, 0f);
            parallaxMult = Random.Range(0.2f, 1.0f);
            GetComponent<LensFlare>().brightness = Random.Range(0.8f, 3f) + 1.5f * parallaxMult;
        }
        if (transform.position.z > Camera.main.transform.position.z + Camera.main.orthographicSize)
        {
            transform.position -= 2f * new Vector3(0f, 0f, Camera.main.orthographicSize);
            parallaxMult = Random.Range(0.2f, 1.0f);
            GetComponent<LensFlare>().brightness = Random.Range(0.8f, 3f) + 1.5f * parallaxMult;
        }
        else if (transform.position.z < Camera.main.transform.position.z - Camera.main.orthographicSize)
        {
            transform.position += 2f * new Vector3(0f, 0f, Camera.main.orthographicSize);
            parallaxMult = Random.Range(0.2f, 1.0f);
            GetComponent<LensFlare>().brightness = Random.Range(0.8f, 3f) + 1.5f * parallaxMult;

        }
        cameraLastPosition = Camera.main.transform.position;
	}
}
    