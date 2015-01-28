using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float fireSpeed;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(fireSpeed * Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
