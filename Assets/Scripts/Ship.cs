using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {
	
	public float forwardThrust= 5.0f;
	public float reverseThrust = 0.0f;
	public float brakeDrag = 10f;
	public float turnRate = 5f;
	public float handling = 0.2f;
	Rigidbody2D myRigidBody;
	public float topSpeed = 9.0f;
	public float turnAngle = 90f;
	private float terminalVelocity;
	// Use this for initialization
	void Start () {
		myRigidBody = GetComponent<Rigidbody2D> ();
		terminalVelocity = ((forwardThrust / myRigidBody.drag) - Time.fixedDeltaTime * forwardThrust) / myRigidBody.mass;
		if (terminalVelocity > topSpeed) {
			terminalVelocity = topSpeed;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (myRigidBody.velocity.magnitude > topSpeed) {
			myRigidBody.velocity = topSpeed * myRigidBody.velocity.normalized;
		}
	}
	
	void FixedUpdate() {
		if(Input.GetKey(KeyCode.W)) {
			myRigidBody.AddRelativeForce(-forwardThrust*Vector2.right);
		}
		if(Input.GetKey(KeyCode.S)) {
			myRigidBody.AddRelativeForce(reverseThrust*Vector2.right);
		}
		if(Input.GetKey(KeyCode.A)) {
			myRigidBody.AddTorque (turnRate);
		}
		if(Input.GetKey(KeyCode.D)) {
			myRigidBody.AddTorque (-turnRate);
		}
		if(Input.GetKeyDown(KeyCode.J)) {
			//Instantiate (myWeapon, transform.position, Quaternion.identity);
		}
		print (myRigidBody.velocity.magnitude + "/" + terminalVelocity);
		glide ();
	}
	
	private void glide() {
		//Add speeds due to handling
		Vector2 newVelocity = myRigidBody.velocity;
		float handlingMagnitude = newVelocity.magnitude * handling;
		newVelocity.x += Mathf.Sin (Mathf.Deg2Rad*transform.rotation.eulerAngles.y) * Mathf.Cos (Mathf.Deg2Rad*transform.rotation.eulerAngles.x) * handlingMagnitude;
		newVelocity.y -= Mathf.Sin (Mathf.Deg2Rad*transform.rotation.eulerAngles.x) * handlingMagnitude;
		//newVelocity.z += Mathf.Cos (Mathf.Deg2Rad*transform.rotation.eulerAngles.y) * Mathf.Cos (Mathf.Deg2Rad*transform.rotation.eulerAngles.x) * handlingMagnitude;
		
		//ensures new velocity is never more than original velocity
		if (newVelocity.magnitude > 0f) {
			newVelocity *= (newVelocity.magnitude - handlingMagnitude) / newVelocity.magnitude;
		}
		
		//sets rigidbody velocity to new velocity
		myRigidBody.velocity = newVelocity;
	}
}
