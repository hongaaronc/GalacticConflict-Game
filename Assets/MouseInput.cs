using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {

    public Sprite cursorIdle;
    public Sprite cursorHover;

    Vector3 cursorPosition;
    Vector3 mousePosition = Vector3.zero;
    Vector3 lastMousePosition = Vector3.zero;
    Vector3 initialMousePosition = Vector3.zero;

	// Use this for initialization
	void Start () {
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //cursorPosition.y = Camera.main.transform.position.y;
        lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;
        lastMousePosition.y = Camera.main.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        //Cursor.visible = false;
        //Cursor.SetCursor(cursorIdle, Vector2.zero, CursorMode.Auto);
        GetComponent<SpriteRenderer>().sprite = cursorIdle;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition)-Camera.main.transform.position;
        mousePosition.y = Camera.main.transform.position.y;
        transform.position += mousePosition - lastMousePosition;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 10f);
        Debug.DrawLine(transform.position, transform.position - 100f * Vector3.up, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100.0F))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            //Cursor.SetCursor(cursorHover, Vector2.zero, CursorMode.Auto);
            GetComponent<SpriteRenderer>().sprite = cursorHover;
            //print("hit");
        }
        lastMousePosition = mousePosition;
	}
}
