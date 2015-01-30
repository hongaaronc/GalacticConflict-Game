using UnityEngine;
using System.Collections;

public class SystemController : MonoBehaviour {
    public GenericSystem[] systems;
	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (GenericSystem system in systems)
            {
                system.Activate();
            }
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (GenericSystem system in systems)
            {
                system.Activate();
            }
        }
    }
}
