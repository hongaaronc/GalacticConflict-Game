﻿using UnityEngine;
using System.Collections;

public class ShipMovement2D : MonoBehaviour {
	public float forwardThrust = 10f;
	public float reverseThrust = 0f;
	public float brakeDrag = 10f;
	public float baseHandling = 0.2f;
	public float thrustHandling = 0.1f;
	public float turnRate = 4f;

	public float topSpeed = 9.0f;
	public float topAngularSpeed = 10f;
	public float turnAngleMin = 20f;
	public float turnAngleMax = 90f;

	private float terminalVelocity;
	private float terminalAngularVelocity;
	private float handling;
	
	private Rigidbody myRigidBody;
	private NetworkView myNetworkView;
	private NetworkManager myNetworkManager;

	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();
        terminalVelocity = forwardThrust / (myRigidBody.mass * myRigidBody.drag);
		terminalAngularVelocity = ((turnRate / myRigidBody.angularDrag) - Time.fixedDeltaTime * turnRate) / myRigidBody.mass;
        terminalAngularVelocity = turnRate / (myRigidBody.mass * myRigidBody.angularDrag);
		if (terminalVelocity > topSpeed) {
			terminalVelocity = topSpeed;
		}
		myRigidBody.maxAngularVelocity = topAngularSpeed;
		if (terminalAngularVelocity > myRigidBody.maxAngularVelocity) {
			terminalAngularVelocity = myRigidBody.maxAngularVelocity;
		}
		myNetworkView = GetComponent<NetworkView>();
		myNetworkManager = Camera.main.GetComponent<NetworkManager> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (myRigidBody.velocity.magnitude > topSpeed)
        {
            myRigidBody.velocity = topSpeed * myRigidBody.velocity.normalized;
        }
		print (myRigidBody.angularVelocity.y +"/"+ terminalAngularVelocity);
	}
	
	void FixedUpdate() {
        handling = baseHandling;
		if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine) {
			if (Input.GetAxis ("Throttle") > 0f) {
					myRigidBody.AddRelativeForce (Input.GetAxis ("Throttle") * forwardThrust * Vector3.forward);
					handling = baseHandling + (thrustHandling - baseHandling) * Input.GetAxis ("Throttle");
			} else if (Input.GetAxis ("Throttle") < 0f) {
					myRigidBody.AddRelativeForce (Input.GetAxis ("Throttle") * reverseThrust * Vector3.forward);
			}
			if (Input.GetAxis ("Rudder") != 0f) {
					myRigidBody.AddTorque (Input.GetAxis ("Rudder") * turnRate * Vector3.up);
			}
		}
        float targetTiltAngle = -(turnAngleMin + (turnAngleMax - turnAngleMin) * myRigidBody.velocity.magnitude / terminalVelocity) * myRigidBody.angularVelocity.y / terminalAngularVelocity;
        float angleDiffZ = targetTiltAngle - transform.eulerAngles.z;
        if (angleDiffZ > 180f)
            angleDiffZ -= 360f;
        if (angleDiffZ < -180f)
            angleDiffZ += 360f;
        float angleDiffX = 0f - transform.eulerAngles.x;
        if (angleDiffX > 180f)
            angleDiffX -= 360f;
        if (angleDiffX < -180f)
            angleDiffX += 360f;
        myRigidBody.AddRelativeTorque(angleDiffX, 0f, 15f * angleDiffZ);
        glide();
	}
	
	private void glide() {
		//Add speeds due to handling
		Vector3 newVelocity = myRigidBody.velocity;
		float handlingMagnitude = newVelocity.magnitude * handling;
		newVelocity.x += Mathf.Sin (Mathf.Deg2Rad*transform.rotation.eulerAngles.y) * Mathf.Cos (Mathf.Deg2Rad*transform.rotation.eulerAngles.x) * handlingMagnitude;
		newVelocity.y -= Mathf.Sin (Mathf.Deg2Rad*transform.rotation.eulerAngles.x) * handlingMagnitude;
		newVelocity.z += Mathf.Cos (Mathf.Deg2Rad*transform.rotation.eulerAngles.y) * Mathf.Cos (Mathf.Deg2Rad*transform.rotation.eulerAngles.x) * handlingMagnitude;
		
		//ensures new velocity is never more than original velocity
		if (newVelocity.magnitude > 0f) {
			newVelocity *= (newVelocity.magnitude - handlingMagnitude) / newVelocity.magnitude;
		}
		
		//sets rigidbody velocity to new velocity
		myRigidBody.velocity = newVelocity;
	}
}
