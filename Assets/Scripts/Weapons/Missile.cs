using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Missile : NetworkBehaviour
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
    public GameObject explosion;
    public GameObject deathParticle;
    public ParticleSystem[] particleSystems;

    public float topSpeed = 9.0f;
    public float topAngularSpeed = 10f;

    public float handling;

    private bool dead = false;

    private GameObject myTarget;

    private Rigidbody rigidbody;
    private NetworkIdentity networkIdentity;

    void OnValidate()
    {
        handling = Mathf.Clamp(handling, 0f, 1f);
    }

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        networkIdentity = GetComponent<NetworkIdentity>();

        rigidbody.maxAngularVelocity = topAngularSpeed;
        rigidbody.AddRelativeForce(fireForce * Vector3.forward);
        rigidbody.AddRelativeForce(Random.Range(-fireRandomForce, fireRandomForce) * Vector3.right);
        rigidbody.AddTorque(Random.Range(-fireRandomTorque, fireRandomTorque) * Vector3.up);
        transform.Rotate(Vector3.up, Random.Range(-fireRandomAngle, fireRandomAngle));

        myTarget = new GameObject("Target");
        myTarget.transform.position = Camera.main.GetComponent<ControlsHandler>().mousePosition;
        myTarget.transform.parent = Camera.main.GetComponent<ControlsHandler>().target;
    }

    // Update is called once per frame
    void Update()
    {
        if (networkIdentity.hasAuthority)
        {
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
                home();
                rigidbody.AddRelativeForce(thrust * Vector3.forward);
                glide();
                rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, topSpeed);
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

    private void glide()
    {
        //Add speeds due to handling
        Vector3 newVelocity = rigidbody.velocity;
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
        rigidbody.velocity = newVelocity;
    }

    private void home()
    {
        float targetAngle = Mathf.Atan2(transform.position.z - myTarget.transform.position.z, myTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg + 90f;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnRate), transform.eulerAngles.z);
    }
}
