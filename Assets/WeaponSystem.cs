using UnityEngine;
using System.Collections;

public class WeaponSystem : GenericSystem {
    public GameObject myWeapon;
    public int[] fireTimes;
    public int coolDown;
    private int timer = 0;
	// Use this for initialization
	void Start () {
        timer = fireTimes[fireTimes.Length - 1] + coolDown + 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (timer <= fireTimes[fireTimes.Length-1]+coolDown)
        {
            foreach (int time in fireTimes)
            {
                if (time == timer)
                {
                    Instantiate(myWeapon, transform.position, transform.rotation);
                }

            }
            timer++;
        }
	}

    public override void Activate()
    {
        if (timer > fireTimes[fireTimes.Length - 1] + coolDown)
            timer = 0;
    }
}
