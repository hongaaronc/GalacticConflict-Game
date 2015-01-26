using UnityEngine;
using System.Collections;

public class ShipMovement2D : MonoBehaviour {
	public float forwardThrust = 10f;
	public float reverseThrust = 0f;
	public float brakeDrag = 10f;
	public float baseHandling = 0.2f;
	public float thrustHandling = 0.1f;
	public float turnRate = 4f;
	public Object myWeapon;

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
		terminalVelocity = ((forwardThrust / myRigidBody.drag) - Time.fixedDeltaTime * forwardThrust) / myRigidBody.mass;
		terminalAngularVelocity = ((turnRate / myRigidBody.angularDrag) - Time.fixedDeltaTime * turnRate) / myRigidBody.mass;
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
		if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine) {
				if (Input.GetKeyDown (KeyCode.Space)) {
						Instantiate (myWeapon, transform.position, Quaternion.identity);
				}
				//Changing euler angles directly causes massive performance issues - figure out how to use rigid body functions for this later
				//transform.localEulerAngles = new Vector3 (transform.localEulerAngles.x, transform.localEulerAngles.y, -(turnAngleMin + (turnAngleMax - turnAngleMin) * myRigidBody.velocity.magnitude / terminalVelocity) * myRigidBody.angularVelocity.y / terminalAngularVelocity);
			float targetTiltAngle = -(turnAngleMin + (turnAngleMax - turnAngleMin) * myRigidBody.velocity.magnitude / terminalVelocity) * myRigidBody.angularVelocity.y / terminalAngularVelocity;
		}
		//print (myRigidBody.angularVelocity.y +"/"+ terminalAngularVelocity);
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
