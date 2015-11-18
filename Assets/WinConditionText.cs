using UnityEngine;
using System.Collections;

public class WinConditionText : MonoBehaviour {
    WinCondition winCond;
    CheckPoint winCheckPoint;
    public TextMesh text;
	// Use this for initialization
	void Start () {
        winCond = GameObject.FindObjectOfType<WinCondition>();
        winCheckPoint = winCond.GetComponent<CheckPoint>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = winCheckPoint.numTimesReached + "/" + winCond.condition;
	}
}
