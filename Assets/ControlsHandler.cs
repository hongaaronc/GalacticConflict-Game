using UnityEngine;
using System.Collections;

public class ControlsHandler : MonoBehaviour {
    public Transform myPlayer;
    public Transform GUIcursor;
    public Vector3 mousePosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        mousePosition = Camera.main.ScreenToWorldPoint(GUIcursor.position);
        mousePosition.y = 0f;
	}
}
