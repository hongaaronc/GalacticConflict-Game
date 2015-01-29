using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float fireForce;
    public float lifetime = 1f;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(fireForce * Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
	}
}
