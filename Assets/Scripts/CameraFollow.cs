using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Rigidbody[] myTargets;
    public float distance = 60f;
    public float easing = 2f;
    public float leadMultiplier = 5f;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadMultiplier * myTargets[0].velocity;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += ((new Vector3(myTargets[0].transform.position.x, distance, myTargets[0].transform.position.z) + leadMultiplier * myTargets[0].velocity) - transform.position) / easing;
		transform.eulerAngles = new Vector3 (90f, 0f, 0f);
	}
}
