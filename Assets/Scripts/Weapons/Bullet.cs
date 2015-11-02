using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    public float fireForce;
    public float detonateRange = 0.2f;
    public float lifetime = 1f;
    public float deathTime = 1f;
    public float damage;
    public GameObject explosion;
    public GameObject deathParticle;
    public ParticleSystem[] particleSystems;

    private bool dead = false;

    private Vector3 lastPosition;

    private GameObject myTarget;

    private Rigidbody myRigidBody;
    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        lastPosition = transform.position;
        myRigidBody = GetComponent<Rigidbody>();
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();

        myRigidBody.AddRelativeForce(fireForce * Vector3.forward);

        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            myTarget = new GameObject("Target");
            myTarget.transform.position = Camera.main.GetComponent<ControlsHandler>().mousePosition;
            myTarget.transform.parent = Camera.main.GetComponent<ControlsHandler>().target;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            transform.LookAt(myTarget.transform.position);
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
        int layerMask = ~(1 << gameObject.layer);
        RaycastHit hit;
        if (Physics.SphereCast(lastPosition, 3f, transform.position - lastPosition, out hit, (transform.position - lastPosition).magnitude, layerMask))
        {
            handleHit(hit.collider);
        }
        else
        {
        }
        lastPosition = transform.position;
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

    void OnTriggerEnter(Collider other)
    {
        handleHit(other);
    }

    void OnCollisionEnter(Collision col)
    {
        handleHit(col.collider);
    }
}
