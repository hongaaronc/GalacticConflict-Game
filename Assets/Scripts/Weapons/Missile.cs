﻿using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    public float fireForce;
    public float fireRandomForce = 3.2f;
    public float fireRandomTorque = 30f;
    public float fireRandomAngle = 30f;

    public float thrust;
    public float turnRate;

    public float detonateRange = 0.2f;
    public float lifetime = 1f;
    public float deathTime = 1f;
    public float damage;
    public GameObject explosion;
    public GameObject deathParticle;
    public ParticleSystem[] particleSystems;

    public float topSpeed = 9.0f;
    public float topAngularSpeed = 10f;

    public float handling;

    private bool dead = false;

    private GameObject myTarget;

    private Rigidbody myRigidBody;
    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;

    void OnValidate()
    {
        handling = Mathf.Clamp(handling, 0f, 1f);
    }

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();

        myRigidBody.maxAngularVelocity = topAngularSpeed;
        myRigidBody.AddRelativeForce(fireForce * Vector3.forward);
        myRigidBody.AddRelativeForce(Random.Range(-fireRandomForce, fireRandomForce) * Vector3.right);
        myRigidBody.AddTorque(Random.Range(-fireRandomTorque, fireRandomTorque) * Vector3.up);
        transform.Rotate(Vector3.up, Random.Range(-fireRandomAngle, fireRandomAngle));

        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            myTarget = new GameObject("Target");
            myTarget.transform.position = Camera.main.GetComponent<ControlsHandler>().mousePosition;
            myTarget.transform.parent = Camera.main.GetComponent<ControlsHandler>().target;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
            {
                if (!dead)
                {
                    if (myNetworkManager.multiplayerEnabled && myNetworkView.isMine)
                    {
                        myNetworkView.RPC("die", RPCMode.All);
                    }
                    else if (!myNetworkManager.multiplayerEnabled)
                    {
                        die();
                    }
                    dead = true;
                }
            }
            if (dead)
            {
                deathTime -= Time.deltaTime;
                if (deathTime <= 0f)
                {
                    Destroy(myTarget);
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
                home();
                myRigidBody.AddRelativeForce(thrust * Vector3.forward);
                glide();
                myRigidBody.velocity = Vector3.ClampMagnitude(myRigidBody.velocity, topSpeed);
                if ((transform.position - myTarget.transform.position).magnitude <= detonateRange)
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

        dead = true;
    }

    [RPC]
    public void die()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.emissionRate = 0f;
        }
        myRigidBody.velocity = Vector3.zero;

        dead = true;
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

    private void home()
    {
        float targetAngle = Mathf.Atan2(transform.position.z - myTarget.transform.position.z, myTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90f;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnRate), transform.eulerAngles.z);
    }

    void OnTriggerEnter(Collider other)
    {
        handleHit(other);
    }

    void OnCollisionEnter(Collision col)
    {
        handleHit(col.collider);
    }

    void handleHit(Collider other)
    {
        if (other.tag == "Ship")
        {
            if (other.GetComponent<Health>() != null)
            {
                if (myNetworkManager.multiplayerEnabled)
                {
                    if (myNetworkView.isMine)
                    {
                        other.GetComponent<Health>().myNetworkView.RPC("takeDamage", RPCMode.All, damage);
                    }
                }
                else
                {
                    other.GetComponent<Health>().takeDamage(damage);
                }
            }
        }
        if (myNetworkManager.multiplayerEnabled)
        {
            if (myNetworkView.isMine)
            {
                myNetworkView.RPC("detonate", RPCMode.All);
            }
        }
        else
        {
            detonate();
        }
    }
}
