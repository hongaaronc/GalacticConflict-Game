using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	public float range = 100f;
    public float parallaxMult = 1f;
	// Use this for initialization
	void Start () {
        GetComponent<LensFlare>().brightness = Random.Range(0.5f, 5f);
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position -= 2f * new Vector3(Camera.main.orthographicSize * Screen.width / Screen.height,0f,0f);
            GetComponent<LensFlare>().brightness = Random.Range(0.5f, 5f);
        }
        else if (transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position += 2f * new Vector3(Camera.main.orthographicSize * Screen.width / Screen.height, 0f, 0f);
            GetComponent<LensFlare>().brightness = Random.Range(0.5f, 5f);
        }
        if (transform.position.z > Camera.main.transform.position.z + Camera.main.orthographicSize)
        {
            transform.position -= 2f * new Vector3(0f, 0f, Camera.main.orthographicSize);
            GetComponent<LensFlare>().brightness = Random.Range(0.5f, 5f);
        }
        else if (transform.position.z < Camera.main.transform.position.z - Camera.main.orthographicSize)
        {
            transform.position += 2f * new Vector3(0f, 0f, Camera.main.orthographicSize);
            GetComponent<LensFlare>().brightness = Random.Range(0.5f, 5f);
        }
	}
}
    