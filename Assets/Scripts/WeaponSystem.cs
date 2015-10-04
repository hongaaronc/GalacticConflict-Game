using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponSystem : GenericSystem {
    public string[] myWeapons;
    public float inheritVelocity = 0f;
    public int[] fireTimes;
    public int coolDown;
    public bool triggerOnce;        //If true, key only needs to be tapped once for the whole system to trigger. If false, key needs to be held down to continue

    private bool myKeyDown = false;
    private int timer = 0;

    // Use this for initialization
	void Start () {
        timer = coolDown + 1;
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
                        foreach (string weapon in myWeapons) {
                            ((WeaponSystemController)systemController).CmdSpawnWeapon(weapon, transform.position, transform.rotation, inheritVelocity);
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
}
