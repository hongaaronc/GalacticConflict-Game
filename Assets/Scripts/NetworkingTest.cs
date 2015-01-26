using UnityEngine;
using System.Collections;

public class NetworkingTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<NetworkView>().isMine) {
			if (Input.GetKey (KeyCode.S)) {
				transform.Translate(0.01f*Vector3.up);
			}
		}
	}
}
