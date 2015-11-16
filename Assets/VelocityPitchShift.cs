using UnityEngine;
using System.Collections;

public class VelocityPitchShift : MonoBehaviour {
    Rigidbody myRigidBody;
    public float minPitch;
    public float maxPitch;
    public float minVolume;
    public float maxVolume;
    private float topspeed;
    public ShipMovement2D shipMove;
    public AudioSource engineSound;
	// Use this for initialization
	void Start () {
        myRigidBody = GetComponentInParent<Rigidbody>();
        topspeed = shipMove.topSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        //engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, myRigidBody.velocity.magnitude/shipMove.topSpeed);
        engineSound.pitch = minPitch + (maxPitch - minPitch) * myRigidBody.velocity.magnitude / topspeed;
        engineSound.volume = Mathf.Lerp(minVolume, maxVolume, myRigidBody.velocity.magnitude / topspeed);
	}
}
