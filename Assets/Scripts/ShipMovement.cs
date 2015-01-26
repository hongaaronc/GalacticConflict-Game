using UnityEngine;
using System.Collections;

public class ShipMovement : MonoBehaviour {
	public float handling = 0.2f;
	public float torqueYaw = 5f;
	public float torqueRoll = 5f;
	public float torquePitch = 5f;
	public float forwardThrust = 10f;
	public float reverseThrust = 0f;
	public Object myWeapon;

	Rigidbody myRigidBody;
	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		if (Input.GetKey (KeyCode.Space)) {
			myRigidBody.AddRelativeForce(forwardThrust*Vector3.forward);
		}
		if(Input.GetKey(KeyCode.W)) {
			myRigidBody.AddRelativeTorque (torquePitch*Vector3.right);
		}
		if(Input.GetKey(KeyCode.S)) {
			myRigidBody.AddRelativeTorque (torquePitch*Vector3.left);
		}
		if(Input.GetKey(KeyCode.A)) {
			myRigidBody.AddRelativeTorque (torqueRoll*Vector3.forward);
		}
		if(Input.GetKey(KeyCode.D)) {
			myRigidBody.AddRelativeTorque (torqueRoll*Vector3.back);
		}
		if(Input.GetKey(KeyCode.Q)) {
			myRigidBody.AddRelativeTorque (torqueYaw*Vector3.down);
		}
		if(Input.GetKey(KeyCode.E)) {
			myRigidBody.AddRelativeTorque (torqueYaw*Vector3.up);
		}
		if(Input.GetKeyDown(KeyCode.J)) {
			Instantiate (myWeapon, transform.position, Quaternion.identity);
		}
		glide ();
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
