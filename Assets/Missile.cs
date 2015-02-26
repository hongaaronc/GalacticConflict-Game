using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    public float fireForce;
    public float lifetime = 1f;

    private float terminalVelocity;
    private float terminalAngularVelocity;
    public float handling;

    private Rigidbody myRigidBody;
    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.AddRelativeForce(fireForce * Vector3.forward);
        myRigidBody.AddRelativeForce(Random.Range(-3.0f, 3.0f) * Vector3.right);
        myRigidBody.AddTorque(Random.Range(-40.0f, 40.0f) * Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        glide();
    }

    private void glide()
    {
        //Add speeds due to handling
        Vector3 newVelocity = myRigidBody.velocity;
        float handlingMagnitude = newVelocity.magnitude * handling;
        newVelocity.x += Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y) * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.x) * handlingMagnitude;
        newVelocity.y -= Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.x) * handlingMagnitude;
        newVelocity.z += Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y) * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.x) * handlingMagnitude;

        //ensures new velocity is never more than original velocity
        if (newVelocity.magnitude > 0f)
        {
            newVelocity *= (newVelocity.magnitude - handlingMagnitude) / newVelocity.magnitude;
        }

        //sets rigidbody velocity to new velocity
        myRigidBody.velocity = newVelocity;
    }
}
