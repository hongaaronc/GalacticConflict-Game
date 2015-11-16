 using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
    public float maxHealth = 100f;
    [HideInInspector]public float myHealth;
    public float maxShield = 100f;
    [HideInInspector]public float myShield;
    public float shieldRechargeDelay = 3f;
    private float shieldRechargeTimer = 0f;
    public float shieldRechargeRate = 50.0f;
    public float parentBaseDamage = 0f;
    public float parentMultDamage = 0f;
    public bool necessaryToLive = false;
    [HideInInspector] public Health parentHealth;
    public Health[] childHealths;
    private bool alive = true;
    private float myLastHealth = 100f;
    private float myLastShield = 100f;

    public GameObject[] dyingSpawns;
    public GameObject[] deathSpawns;

    [HideInInspector]
    public NetworkView myNetworkView;
    private NetworkManager myNetworkManager;

    public MeshRenderer[] shieldMeshRenderers;
    public ParticleSystem[] shieldParticleSystems;
    public float shieldFadeRate;
    public float shieldHitAlphaIncrease;
    public int hitParticles = 100;

    void OnValidate()
    {
        foreach (Health childHealth in childHealths) {
            childHealth.parentHealth = this;
        }
        myShield = Mathf.Clamp(myShield, 0f, maxShield);
    }

	// Use this for initialization
	void Start () {
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();

        myHealth = maxHealth;
        myLastHealth = myHealth;
        myShield = maxShield;
        myLastShield = myShield;

        //foreach (MeshRenderer shield in shieldMeshRenderers)
        //{
        //    if (shield.additionalVertexStreams == null)
        //        continue;
        //    Vector3[] originalVertices = shield.additionalVertexStreams.vertices;
        //    Vector3[] newVertices = new Vector3[originalVertices.Length];
        //    for (int i = 0; i < originalVertices.Length; i++)
        //    {
        //        newVertices[i] = originalVertices[i];
        //        newVertices[i].x += 2f;
        //        print("test");
        //    }
        //    shield.additionalVertexStreams.vertices = newVertices;
        //}
	}
	
	// Update is called once per frame
	void Update () {
        if (alive)
        {
            if (myHealth <= 0f)
            {
                if (parentHealth != null)
                {
                    parentHealth.myHealth -= parentBaseDamage;
                }
                if (necessaryToLive)
                {
                    //Ship death
                    if (myNetworkManager.multiplayerEnabled)
                    {
                        myNetworkView.RPC("explode", RPCMode.All);
                    }
                    else
                    {
                        explode();
                    }
                    if (myNetworkManager.multiplayerEnabled && myNetworkView.isMine)
                        Network.Destroy(gameObject);
                    else
                        Destroy(gameObject);
                    Camera.main.GetComponent<NetworkManager>().spawnShip();
                }
                alive = false;
            }
            if (myHealth != myLastHealth)
            {
                if (parentHealth != null)
                {
                    parentHealth.myHealth += parentMultDamage * (myHealth - myLastHealth);
                }
            }
            myHealth = Mathf.Clamp(myHealth, 0f, maxHealth);
            myLastHealth = myHealth;
        }

        foreach (MeshRenderer shield in shieldMeshRenderers)
        {
            Color newColor = shield.material.color;
            newColor = new Color(newColor.r, newColor.g, newColor.b, Mathf.Clamp01(newColor.a - shieldFadeRate * Time.deltaTime));
            shield.material.color = newColor;
        }

        if (shieldRechargeTimer > 0)
            shieldRechargeTimer -= Time.deltaTime;
        else if (myShield < maxShield)
        {
            myShield = Mathf.Clamp(myShield + Time.deltaTime * shieldRechargeRate, 0f, maxShield);
        }
	}

    [RPC]
    public void takeDamage(float damage)
    {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
        {
            if (myShield > 0)
            {
                myShield = Mathf.Clamp(myShield - damage, 0f, maxShield);

                foreach (MeshRenderer shield in shieldMeshRenderers)
                {
                    Color newColor = shield.material.color;
                    newColor = new Color(newColor.r, newColor.g, newColor.b, Mathf.Clamp01(newColor.a - shieldHitAlphaIncrease));
                    shield.material.color = newColor;
                }
                foreach (ParticleSystem ps in shieldParticleSystems)
                {
                    ps.Emit(hitParticles);
                }
            }
            else
            {
                myHealth -= damage;
            }
            FindObjectOfType<HUDChromaticAbberation>().distort(0.5f * damage);
            shieldRechargeTimer = shieldRechargeDelay;
        }
    }

    [RPC]
    private void explode()
    {
        foreach (GameObject spawn in deathSpawns)
        {
            GameObject newWeapon = (GameObject)Instantiate(spawn, transform.position, transform.rotation);
        }
    }
}
