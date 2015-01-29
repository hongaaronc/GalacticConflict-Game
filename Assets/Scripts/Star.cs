using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	public float range = 100f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if((transform.position-Camera.main.transform.position).magnitude > range) {
			transform.position = Camera.main.transform.position + Random.onUnitSphere * range;
		}
	}
}
    