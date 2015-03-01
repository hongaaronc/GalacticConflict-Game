using UnityEngine;
using System.Collections;

public class ShipMovement2D : MonoBehaviour {
    
	public float forwardThrust = 10f;
	public float reverseThrust = 0f;
    private float baseDrag;
	public float brakeDrag = 10f;
	public float baseHandling = 0.2f;
	public float thrustHandling = 0.1f;
	public float turnRate = 4f;

	public float topSpeed = 9.0f;
    private float baseTopSpeed;
    public float warpTopSpeed = 800f;
	public float topAngularSpeed = 10f;
	public float turnAngleMin = 20f;
	public float turnAngleMax = 90f;

    public float tiltTorque = 15f;

	private float terminalVelocity;
	private float terminalAngularVelocity;
	private float handling;

    public float warpSpeed = 0.5f;
    private float startWarpTime;
    private float warpTime;
	
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
        baseDrag = myRigidBody.drag;
        baseTopSpeed = topSpeed;
		myNetworkView = GetComponent<NetworkView>();
		myNetworkManager = Camera.main.GetComponent<NetworkManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		//print (myRigidBody.angularVelocity.y +"/"+ terminalAngularVelocity);
	}
	
	void FixedUpdate() {
        handling = baseHandling;
        myRigidBody.drag = baseDrag;
		if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine) {
			if (Input.GetAxisRaw ("Throttle") > 0f) {
					myRigidBody.AddRelativeForce (Input.GetAxis ("Throttle") * forwardThrust * Vector3.forward);
					handling = baseHandling + (thrustHandling - baseHandling) * Input.GetAxis ("Throttle");
			} else if (Input.GetAxisRaw ("Throttle") < 0f) {
					myRigidBody.AddRelativeForce (Input.GetAxis ("Throttle") * reverseThrust * Vector3.forward);
                    myRigidBody.drag = baseDrag + (brakeDrag - baseDrag) * Mathf.Abs(Input.GetAxis("Throttle"));
			}
			if (Input.GetAxisRaw ("Rudder") != 0f) {
					myRigidBody.AddTorque (Input.GetAxis ("Rudder") * turnRate * Vector3.up);
			}
		}
        float targetTiltAngleZ = -(turnAngleMin + (turnAngleMax - turnAngleMin) * myRigidBody.velocity.magnitude / terminalVelocity) * myRigidBody.angularVelocity.y / terminalAngularVelocity;
        float angleDiffZ = targetTiltAngleZ - transform.eulerAngles.z;
        if (angleDiffZ > 180f)
            angleDiffZ -= 360f;
        if (angleDiffZ < -180f)
            angleDiffZ += 360f;
        float angleDiffX = 0f - transform.eulerAngles.x;
        if (angleDiffX > 180f)
            angleDiffX -= 360f;
        if (angleDiffX < -180f)
            angleDiffX += 360f;
        myRigidBody.AddRelativeTorque(angleDiffX, 0f, tiltTorque * angleDiffZ);
        glide();

        if (Input.GetKeyDown(KeyCode.I))
        {
            startWarpTime = Time.time;
            warpTime = 0f;
        }
        if (Input.GetKey(KeyCode.I))
        {
            //transform.position += warpSpeed * (Time.time - startWarpTime) * new Vector3(Mathf.Sin(Mathf.PI / 180f * transform.eulerAngles.y), 0f, Mathf.Cos(Mathf.PI / 180f * transform.eulerAngles.y));
            myRigidBody.AddRelativeForce(warpSpeed * (warpTime) * Vector3.forward);
            warpTime++;
            topSpeed = warpTopSpeed;
        }
        else
        {
            myRigidBody.isKinematic = false;
            topSpeed = baseTopSpeed;
        }

        if (myRigidBody.velocity.magnitude > topSpeed)
        {
            myRigidBody.velocity = topSpeed * myRigidBody.velocity.normalized;
        }
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
