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

    public Object[] dyingSpawns;
    public Object[] deathSpawns;

    [HideInInspector]
    public NetworkView myNetworkView;
    private NetworkManager myNetworkManager;

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
            }
            else
            {
                myHealth -= damage;
            }
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
