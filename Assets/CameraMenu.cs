using UnityEngine;
using System.Collections;

public class CameraMenu : MonoBehaviour {
    public Transform target;
    public float smoothTime = 0.5f;
    Vector3 velocity;
    public float rotateSpeed = 0.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, rotateSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, 1.0f / (1.0f + (transform.position - target.position).magnitude));
        }
	}
}
