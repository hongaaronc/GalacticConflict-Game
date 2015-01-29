using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform[] myTargets;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(myTargets [0].position.x, 60f, myTargets [0].position.z);
		transform.eulerAngles = new Vector3 (90f, 0f, 0f);
	}
}
