using UnityEngine;
using System.Collections;

public class ControlsHandler : MonoBehaviour {
    public Transform myPlayer;
    public Transform GUIcursor;
    public MouseInput mouseInput;
    public Vector3 mousePosition;
    [HideInInspector]
    public Transform target = null;

	// Use this for initialization
	void Start () {
        target = mouseInput.lockedTarget;
	}
	
	// Update is called once per frame
	void Update () {
        mousePosition = Camera.main.ScreenToWorldPoint(GUIcursor.position);
        mousePosition.y = 0f;
        target = mouseInput.lockedTarget;
	}
}
