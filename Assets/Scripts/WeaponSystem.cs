using UnityEngine;
using System.Collections;

public class WeaponSystem : GenericSystem {
    struct overheatManager{
        public float minValue;
        public float maxValue;
        public float value;
    }

    public GameObject[] myWeapons;
    public GameObject[] myClientsideWeapons;
    public float inheritVelocity = 0f;
    public int[] fireTimes;
    public int coolDown;
    public bool triggerOnce;        //If true, key only needs to be tapped once for the whole system to trigger. If false, key needs to be held down to continue 

    private bool myKeyDown = false;
    private int timer = 0;
    private overheatManager overheat;

    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        overheat.value = 0;
        overheat.minValue = 0;
        overheat.maxValue = 100;
        timer = coolDown + 1;
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
	}

    void Update ()
    {
        overheat.value -= 10f / 60f;
    }
	
	
	void FixedUpdate () {
        if (timer <= coolDown && overheat.value < 100f)
        {
            if (triggerOnce || myKeyDown)
            {
                foreach (int time in fireTimes)
                {
                    if (time == timer)
                    {
                        GameObject newWeapon;
                        overheat.value += 6f;
                        foreach (GameObject weapon in myWeapons) {
                            if (myNetworkManager.multiplayerEnabled)
                            {
                                newWeapon = (GameObject)Network.Instantiate(weapon, transform.position, transform.rotation, 0);
                                myNetworkView.RPC("spawnClientsideWeapons", RPCMode.All);
                            }
                            else
                            {
                                newWeapon = (GameObject)Instantiate(weapon, transform.position, transform.rotation);
                                spawnClientsideWeapons();
                            }
                            if (newWeapon.GetComponent<Rigidbody>() != null && GetComponentInParent<Rigidbody>() != null)
                                newWeapon.GetComponent<Rigidbody>().velocity = inheritVelocity * GetComponentInParent<Rigidbody>().velocity;
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
                newWeapon.GetComponent<Rigidbody>().velocity = inheritVelocity * GetComponentInParent<Rigidbody>().velocity;
        }
    }
}
