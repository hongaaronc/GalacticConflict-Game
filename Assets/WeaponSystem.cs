﻿using UnityEngine;
using System.Collections;

public class WeaponSystem : GenericSystem {
    public GameObject myWeapon;
    public int[] fireTimes;
    public int coolDown;
    public bool triggerOnce;        //If true, key only needs to be tapped once for the whole system to trigger. If false, key needs to be held down to continue 

    private bool myKeyDown = false;
    private int timer = 0;

    private NetworkManager myNetworkManager;
	// Use this for initialization
	void Start () {
        timer = fireTimes[fireTimes.Length - 1] + coolDown + 1;
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (timer <= fireTimes[fireTimes.Length - 1] + coolDown)
        {
            if (triggerOnce || myKeyDown)
            {
                foreach (int time in fireTimes)
                {
                    if (time == timer)
                    {
                        if (myNetworkManager.multiplayerEnabled)
                            Network.Instantiate(myWeapon, transform.position, transform.rotation, 0);
                        else
                            Instantiate(myWeapon, transform.position, transform.rotation);
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
        if (timer > fireTimes[fireTimes.Length - 1] + coolDown)
            timer = 0;
    }
}
