using UnityEngine;
using System.Collections;

public class WeaponSystem : GenericSystem {
    public GameObject[] myWeapons;
    public GameObject[] myClientsideWeapons;
    public float inheritVelocity = 0f;
    public int[] fireTimes;
    public int coolDown;
    public bool triggerOnce;        //If true, key only needs to be tapped once for the whole system to trigger. If false, key needs to be held down to continue 

    private bool myKeyDown = false;
    private int timer = 0;

    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        timer = coolDown + 1;
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
	}
	
	
	void FixedUpdate () {
        if (timer <= coolDown)
        {
            if (triggerOnce || myKeyDown)
            {
                foreach (int time in fireTimes)
                {
                    if (time == timer)
                    {
                        GameObject newWeapon;
                        if (myNetworkManager.multiplayerEnabled)
                        {
                            foreach (GameObject weapon in myWeapons)
                            {
                                newWeapon = (GameObject)Network.Instantiate(weapon, transform.position, transform.rotation, 0);
                                if (newWeapon.GetComponent<Rigidbody>() != null && GetComponentInParent<Rigidbody>() != null)
                                {
                                    newWeapon.GetComponent<Rigidbody>().velocity = inheritVelocity * GetComponentInParent<Rigidbody>().velocity;
                                }
                            }
                            myNetworkView.RPC("spawnClientsideWeapons", RPCMode.All);
                        }
                        else
                        {
                            foreach (GameObject weapon in myWeapons)
                            {
                                newWeapon = (GameObject)Instantiate(weapon, transform.position, transform.rotation);
                                if (newWeapon.GetComponent<Rigidbody>() != null && GetComponentInParent<Rigidbody>() != null)
                                {
                                    newWeapon.GetComponent<Rigidbody>().velocity = inheritVelocity * GetComponentInParent<Rigidbody>().velocity;
                                }
                            }
                            spawnClientsideWeapons();
                        }
                    }

                }
            }
            timer++;
        }
        myKeyDown = false;
	}

    public override void Activate()
    {
        myKeyDown = true;
        if (timer > coolDown)
            timer = 0;
    }

    [RPC]
    private void spawnClientsideWeapons()
    {
        foreach (GameObject weapon in myClientsideWeapons)
        {
            GameObject newWeapon = (GameObject)Instantiate(weapon, transform.position, transform.rotation);
            if (newWeapon.GetComponent<Rigidbody>() != null && GetComponentInParent<Rigidbody>() != null)
            {
                newWeapon.GetComponent<Rigidbody>().velocity = inheritVelocity * GetComponentInParent<Rigidbody>().velocity;
            }
        }
    }
}
