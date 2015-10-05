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

    void OnValidate()
    {
        foreach (Health childHealth in childHealths) {
            childHealth.parentHealth = this;
        }
        myHealth = Mathf.Clamp(myHealth, 0f, maxHealth);
    }

	// Use this for initialization
	void Start () {
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
                    foreach (Object spawn in deathSpawns)
                    {
                        Instantiate(spawn, transform.position, Quaternion.identity);
                    }
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
}
