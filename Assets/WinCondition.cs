using UnityEngine;
using System.Collections;

public class WinCondition : MonoBehaviour {
    public int condition = 3;
    public string winLevel;
    public CheckPoint checkpointToCheck;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (checkpointToCheck.numTimesReached >= condition)
        {
            Application.LoadLevel(winLevel);
        }
	}
}
