using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet : NetworkBehaviour {
    public float fireForce;
    public float detonateRange = 0.2f;
    public float lifetime = 1f;
    public float deathTime = 1f;
    public GameObject explosion;
    public GameObject deathParticle;
    public ParticleSystem[] particleSystems;

    private bool dead = false;

    private GameObject myTarget;

    private Rigidbody rigidbody;
    private NetworkIdentity networkIdentity;
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        networkIdentity = GetComponent<NetworkIdentity>();

        rigidbody.AddRelativeForce(fireForce * Vector3.forward);

        if (networkIdentity.hasAuthority)
        {
            myTarget = new GameObject("Target");
            myTarget.transform.position = Camera.main.GetComponent<ControlsHandler>().mousePosition;
            myTarget.transform.parent = Camera.main.GetComponent<ControlsHandler>().target;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (networkIdentity.hasAuthority)
        {
            transform.LookAt(myTarget.transform.position);
            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
            {
                if (!dead)
                {
                    CmdDie();
                    dead = true;
                }
            }
            if (dead)
            {
                deathTime -= Time.deltaTime;
                if (deathTime <= 0f)
                {
                    Destroy(myTarget);
                    Destroy(gameObject);
                }
            }
        }
	}

    void FixedUpdate()
    {
        if (networkIdentity.hasAuthority)
        {
            if (!dead)
            {
                if ((transform.position - myTarget.transform.position).magnitude <= detonateRange)
                {
                    CmdDetonate();
                    dead = true;
                }
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
            }
        }
    }

    [Command]
    public void CmdDetonate()
    {
        RpcDetonate();
    }

    [ClientRpc]
    public void RpcDetonate()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.emissionRate = 0f;
        }
        rigidbody.velocity = Vector3.zero;
    }

    [Command]
    public void CmdDie()
    {
        RpcDie();
    }

    [ClientRpc]
    public void RpcDie()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.emissionRate = 0f;
        }
        rigidbody.velocity = Vector3.zero;
    }
}
