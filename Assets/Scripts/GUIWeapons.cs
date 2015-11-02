using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIWeapons : MonoBehaviour {
    public Slider overheat;
	// Use this for initialization
	void Start () {
        overheat.minValue = 0f;
        overheat.maxValue = 100f;
        overheat.value = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("space"))
        {
            overheat.value += 25f/60f;
        }
        overheat.value -= 10f/60f;
	}
}
