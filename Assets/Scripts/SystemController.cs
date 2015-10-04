using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SystemController : NetworkBehaviour {
    public int abilityNum;
    public GenericSystem[] systems;

    [HideInInspector]
    public NetworkIdentity networkIdentity;

    void OnValidate()
    {
        abilityNum = Mathf.Clamp(abilityNum, 0, 4);
        foreach (GenericSystem system in systems)
        {
            system.systemController = this;
        }
    }
	// Use this for initialization
	void Start () {
        networkIdentity = GetComponent<NetworkIdentity>();
	}

    // Update is called once per frame
    void Update()
    {
        if (networkIdentity.hasAuthority)
        {
            if (Input.GetAxisRaw("Ability"+abilityNum) == 1.0f)
            {
                foreach (GenericSystem system in systems)
                {
                    system.Activate();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (networkIdentity.hasAuthority)
        {
            if (Input.GetAxisRaw("Ability" + abilityNum) == 1.0f)
            {
                foreach (GenericSystem system in systems)
                {
                    system.Activate();
                }
            }
        }
    }
}
