﻿using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    public Vector3 targetVector;
    public float fireForce;
    public float thrust;
    public float detonateRange = 0.2f;
    public float lifetime = 1f;
    public float deathTime = 1f;
    public GameObject explosion;
    public ParticleSystem[] particleSystems;

    public float topSpeed = 9.0f;
    public float topAngularSpeed = 10f;

    public float handling;

    private bool dead = false;

    private Rigidbody myRigidBody;
    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;

    // Use this for initialization
    void Start()
    {
        //targetVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //targetVector.y = transform.position.y;
        myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.maxAngularVelocity = topAngularSpeed;
        myRigidBody.AddRelativeForce(fireForce * Vector3.forward);
        myRigidBody.AddRelativeForce(Random.Range(-3.2f, 3.2f) * Vector3.right);
        myRigidBody.AddTorque(Random.Range(-30.0f, 30.0f) * Vector3.up);

        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            targetVector = Vector3.zero;
            transform.LookAt(targetVector);            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
            {
                if (!dead)
                {
                    if (myNetworkManager.multiplayerEnabled && myNetworkView.isMine)
                    {
                        myNetworkView.RPC("detonate", RPCMode.All);
                    }
                    else if (!myNetworkManager.multiplayerEnabled)
                    {
                        detonate();
                    }
                    dead = true;
                }
            }
            if (dead)
            {
                deathTime -= Time.deltaTime;
                if (deathTime <= 0f)
                {
                    if (myNetworkManager.multiplayerEnabled)
                        Network.Destroy(gameObject);
                    else
                        Destroy(gameObject);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            if (!dead)
            {
                myRigidBody.AddRelativeForce(thrust * Vector3.forward);
                glide();
                if (myRigidBody.velocity.magnitude > topSpeed)
                {
                    myRigidBody.velocity = topSpeed * myRigidBody.velocity.normalized;
                }
                if ((transform.position - targetVector).magnitude <= detonateRange)
                {
                    if (myNetworkManager.multiplayerEnabled && myNetworkView.isMine)
                    {
                        myNetworkView.RPC("detonate", RPCMode.All);
                    }
                    else if (!myNetworkManager.multiplayerEnabled)
                    {
                        detonate();
                    }
                    dead = true;
                }
            }
            else
            {
                myRigidBody.velocity = Vector3.zero;
            }
        }
    }

    [RPC]
    public void detonate()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.emissionRate = 0f;
        }
        myRigidBody.velocity = Vector3.zero;
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