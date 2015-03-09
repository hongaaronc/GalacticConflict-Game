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
    public bool freeCam = false;
    public float scrollMargin = 5f;
    public float scrollSpeed = 2f;
	// Use this for initialization
	void Start () {
        try
        {
            Vector3 leadValue = Vector3.ClampMagnitude(leadMultiplier * myTargets[0].velocity, maxLead);
            transform.position = new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue;
        }
        catch { }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
            freeCam = true;
        else
            freeCam = false;
        if (freeCam)
        {
            if (GUIcursor.position.x < scrollMargin)
                transform.Translate(scrollSpeed * Vector3.left);
            if (GUIcursor.position.x > Screen.width - scrollMargin)
                transform.Translate(scrollSpeed * Vector3.right);
            if (GUIcursor.position.y < scrollMargin)
                transform.Translate(scrollSpeed * Vector3.down);
            if (GUIcursor.position.y > Screen.height - scrollMargin)
                transform.Translate(scrollSpeed * Vector3.up);
        }
        else
        {
            try
            {
                Vector3 leadValue = Vector3.ClampMagnitude(leadMultiplier * myTargets[0].velocity, maxLead);
                transform.position += ((new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue) - transform.position) / easing;
            }
            catch { }
            transform.eulerAngles = new Vector3(90f, 0f, 0f);
        }
        mousePosition = Camera.main.ScreenToWorldPoint(GUIcursor.position);
        mousePosition.y = 0f;
	}
}
