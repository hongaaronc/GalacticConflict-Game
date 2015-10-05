using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
    public float maxHealth = 100f;
    public float myHealth;
    public float parentBaseDamage = 0f;
    public float parentMultDamage = 0f;
    public bool necessaryToLive = false;
    [HideInInspector] public Health parentHealth;
    public Health[] childHealths;
    private bool alive = true;
    private float myLastHealth = 100f;

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
        myHealth = Mathf.Clamp(myHealth, 0f, maxHealth);
    }

	// Use this for initialization
	void Start () {
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();

        myHealth = maxHealth;
        myLastHealth = myHealth;
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
	}

    [RPC]
    public void takeDamage(float damage)
    {
        if (!myNetworkManager.multiplayerEnabled || myNetworkView.isMine)
            myHealth -= damage;
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
