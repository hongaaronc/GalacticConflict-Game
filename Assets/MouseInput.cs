using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = Camera.main.transform.position.y;
        Debug.DrawRay(mousePosition, -Vector3.up, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(mousePosition, -Vector3.up, out hit, 100.0F))
        {
            Debug.DrawLine(mousePosition, hit.point, Color.red);
        }       
	}
}
