using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	public float range = 100f;
    public float parallaxMult = 1f;

    public float warpSpeed = 0.5f;
    public float warpRange = 2f;
    public float warpDistortion = 40f;
    public bool noReverseDistortion = true;
    private float startWarpTime;

    private Vector3 cameraLastPosition;
	// Use this for initialization
	void Start () {
        parallaxMult = Random.Range(0.2f, 1.0f);
        GetComponent<LensFlare>().brightness = Random.Range(0.8f, 3f) + 1.5f * parallaxMult;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position -= (parallaxMult - 1f) * (Camera.main.transform.position - cameraLastPosition);
        if (Input.GetKeyDown(KeyCode.I))
        {
            GetComponent<TrailRenderer>().enabled = true;
            startWarpTime = Time.time;
        }
        if (Input.GetKey(KeyCode.I))
        {
            GetComponent<TrailRenderer>().enabled = true;
            Transform warpingShip = Camera.main.GetComponent<CameraFollow>().myTargets[0].transform;
            float warpConst = (warpRange - (transform.position - warpingShip.position).magnitude)/warpRange;
            if (noReverseDistortion && warpConst < 0f)
                warpConst = 0f;
                transform.position += 1f / (1f + warpDistortion * warpConst) * warpSpeed * (Time.time - startWarpTime) * (parallaxMult - 1f) * new Vector3(Mathf.Sin(Mathf.PI / 180f * warpingShip.eulerAngles.y), 0f, Mathf.Cos(Mathf.PI / 180f * warpingShip.eulerAngles.y));
            }
        else
        {
            GetComponent<TrailRenderer>().enabled = false;
            wrap();
        }
        cameraLastPosition = Camera.main.transform.position;
	}

    private void wrap()
    {
        if (transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position -= 2f * new Vector3(Camera.main.orthographicSize * Screen.width / Screen.height, 0f, 0f);
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
    }
}
    