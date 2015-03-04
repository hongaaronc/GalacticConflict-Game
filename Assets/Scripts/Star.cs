using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
    public float brightnessBase = 1.5f;
    public float brightnessRandMin = 0.8f;
    public float brightnessRandMax = 3f;

    private float parallaxMult = 1f;
    private float parallaxMultMin = 0.2f;
    private float parallaxMultMax = 1.0f;

    public float warpThrust = 5f;
    public float warpPower = 2f;
    public float warpRange = 2f;
    public float warpDistortion = 40f;
    public bool noReverseDistortion = true;
    private float startWarpTime;
    private bool warpingLastFrame = false;

    private Vector3 cameraLastPosition;

    void OnValidate()
    {
        warpPower = Mathf.Clamp(warpPower, 1f, float.PositiveInfinity);
    }

	// Use this for initialization
	void Start () {
        parallaxMult = Random.Range(parallaxMultMin, parallaxMultMax);
        GetComponent<LensFlare>().brightness = Random.Range(brightnessRandMin, brightnessRandMax) + brightnessBase * parallaxMult;
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody mainShip = Camera.main.GetComponent<CameraFollow>().myTargets[0];
        transform.position -= (parallaxMult - 1f) * (Camera.main.transform.position - cameraLastPosition);
        if (mainShip != null)
        {
            if (!warpingLastFrame && mainShip.GetComponent<ShipMovement2D>().warping)
            {
                GetComponent<TrailRenderer>().enabled = true;
                startWarpTime = Time.time;
            }
            if (mainShip.GetComponent<ShipMovement2D>().warping)
            {
                GetComponent<TrailRenderer>().enabled = true;
                float warpConst = (warpRange - (transform.position - mainShip.position).magnitude) / warpRange;
                if (noReverseDistortion)
                    warpConst = Mathf.Clamp(warpConst, 0f, float.PositiveInfinity);
                transform.position += 1f / (1f + warpDistortion * warpConst) * warpThrust * Mathf.Pow(Time.time - startWarpTime, warpPower - 1f) * (parallaxMult - 1f) * new Vector3(Mathf.Sin(Mathf.PI / 180f * mainShip.transform.eulerAngles.y), 0f, Mathf.Cos(Mathf.PI / 180f * mainShip.transform.eulerAngles.y));
            }
            else
            {
                GetComponent<TrailRenderer>().enabled = false;
                wrap();
            }
            warpingLastFrame = mainShip.GetComponent<ShipMovement2D>().warping;
        }
        cameraLastPosition = Camera.main.transform.position;
	}

    private void wrap()
    {
        if (transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position -= 2f * new Vector3(Camera.main.orthographicSize * Screen.width / Screen.height, 0f, 0f);
            parallaxMult = Random.Range(parallaxMultMin, parallaxMultMax);
            GetComponent<LensFlare>().brightness = Random.Range(brightnessRandMin, brightnessRandMax) + brightnessBase * parallaxMult;
        }
        else if (transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height)
        {
            transform.position += 2f * new Vector3(Camera.main.orthographicSize * Screen.width / Screen.height, 0f, 0f);
            parallaxMult = Random.Range(parallaxMultMin, parallaxMultMax);
            GetComponent<LensFlare>().brightness = Random.Range(brightnessRandMin, brightnessRandMax) + brightnessBase * parallaxMult;
        }
        if (transform.position.z > Camera.main.transform.position.z + Camera.main.orthographicSize)
        {
            transform.position -= 2f * new Vector3(0f, 0f, Camera.main.orthographicSize);
            parallaxMult = Random.Range(parallaxMultMin, parallaxMultMax);
            GetComponent<LensFlare>().brightness = Random.Range(brightnessRandMin, brightnessRandMax) + brightnessBase * parallaxMult;
        }
        else if (transform.position.z < Camera.main.transform.position.z - Camera.main.orthographicSize)
        {
            transform.position += 2f * new Vector3(0f, 0f, Camera.main.orthographicSize);
            parallaxMult = Random.Range(parallaxMultMin, parallaxMultMax);
            GetComponent<LensFlare>().brightness = Random.Range(brightnessRandMin, brightnessRandMax) + brightnessBase * parallaxMult;
        }
    }
}
    