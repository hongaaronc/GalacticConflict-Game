using UnityEngine;
using System.Collections;

public class ParticleControllerSystem : GenericSystem
{
    public ParticleSystem[] myParticleSystems;
    public int[] triggerTimes;
    public int coolDown;
    public bool triggerOnce;        //If true, key only needs to be tapped once for the whole system to trigger. If false, key needs to be held down to continue 

    private bool myKeyDown = false;
    private int timer = 0;

    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
    // Use this for initialization
    void Start()
    {
        timer = coolDown + 1;
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
    }


    void FixedUpdate()
    {
        if (timer <= coolDown)
        {
            if (triggerOnce || myKeyDown)
            {
                foreach (int time in triggerTimes)
                {
                    if (time == timer)
                    {
                        if (myNetworkManager.multiplayerEnabled)
                            myNetworkView.RPC("playParticleSystems", RPCMode.All);
                        else
                            playParticleSystems();
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
    private void playParticleSystems()
    {
        foreach (ParticleSystem particleSystem in myParticleSystems)
        {
            particleSystem.Play();
        }
    }
}
