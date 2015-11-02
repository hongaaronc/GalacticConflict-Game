using UnityEngine;
using System.Collections;

public class GUIHudArrow : MonoBehaviour {
    public GameObject playerShip;
    private Rigidbody playerRigidbody;
    public GameObject targetShip;
    private Rigidbody targetRigidbody;
    public GameObject model;
    private Rigidbody modelRigidbody;
	// Use this for initialization
	void Start () {
        playerRigidbody = playerShip.GetComponent<Rigidbody>();
        targetRigidbody = targetShip.GetComponent<Rigidbody>();
        modelRigidbody = model.GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        Vector3 relativeVelocity = targetRigidbody.velocity - playerRigidbody.velocity;
        modelRigidbody.angularVelocity = new Vector3(relativeVelocity.z, targetRigidbody.angularVelocity.y, -relativeVelocity.x);
	}
}
