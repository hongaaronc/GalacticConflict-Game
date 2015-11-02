using UnityEngine;
using System.Collections;

public class TargetCamera : MonoBehaviour {
    public MouseInput cursor;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (cursor.lockedTarget != null)
        {
            transform.position = cursor.lockedTarget.position;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
        }
	}
}
