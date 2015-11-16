using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour {
    public bool ready = false;
    public CheckPoint nextCheckpoint;
    public int numTimesReached = 0;

    void OnTriggerEnter(Collider other)
    {
        if (ready && other.gameObject.layer == 8)   //If ready and is local player
        {
            print("Test");
            numTimesReached++;
            nextCheckpoint.ready = true;
            ready = false;
        }
    }
}
