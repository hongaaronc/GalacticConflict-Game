using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Rigidbody[] myTargets;
    public float distance = 60f;
    public float easing = 2f;
    public float leadMultiplier = 5f;
    public float maxLead = 30f;
	// Use this for initialization
	void Start () {
        try
        {
            Vector3 leadValue = leadMultiplier * myTargets[0].velocity;
            if (leadValue.magnitude > maxLead)
                leadValue = maxLead * leadValue.normalized;
            transform.position = new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue;
        }
        catch { }
	}
	
	// Update is called once per frame
	void Update () {
        try
        {
            Vector3 leadValue = leadMultiplier * myTargets[0].velocity;
            if (leadValue.magnitude > maxLead)
                leadValue = maxLead * leadValue.normalized;
            transform.position += ((new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadValue) - transform.position) / easing;
        }
        catch { }
		transform.eulerAngles = new Vector3 (90f, 0f, 0f);
	}
}
