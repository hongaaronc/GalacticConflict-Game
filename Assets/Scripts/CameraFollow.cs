using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Rigidbody[] myTargets;
    public Transform GUIcursor;
    public Vector3 mousePosition;
    public float distance = 60f;
    public float easing = 2f;
    public float leadMultiplier = 5f;
    public float maxLead = 30f;
    Vector3 velocity;
    public float dampTime = 1.0f;

    // Use this for initialization
	void Start () {
        velocity = Vector3.zero;
        try
        {
            Vector3 leadValue = Vector3.ClampMagnitude(leadMultiplier * myTargets[0].velocity, maxLead);
            transform.position = new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue;
        }
        catch { }
	}
	
	// Update is called once per frame
	void Update () {
        if (myTargets[0] == null && Camera.main.GetComponent<NetworkManager>().gameStarted)
        {
            Camera.main.GetComponent<NetworkManager>().spawnShip();
        }
        try
        {
            Vector3 leadValue = Vector3.ClampMagnitude(leadMultiplier * myTargets[0].velocity, maxLead);
            //transform.position += ((new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue) - transform.position) / easing;
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue, ref velocity, dampTime);
        }
        catch { }
        transform.eulerAngles = new Vector3(90f, 0f, 0f);
        mousePosition = Camera.main.ScreenToWorldPoint(GUIcursor.position);
        mousePosition.y = 0f;
	}
}
