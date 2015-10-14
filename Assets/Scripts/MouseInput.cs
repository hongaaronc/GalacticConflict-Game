using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {

    public float sensitivity = 10f;
    public bool trackRotation = true;
    public bool adjustingAllowed = true;
    public bool lockMouse = true;

    public Sprite cursorIdle;
    public Sprite cursorHover;

    private RaycastHit hit;

    private bool isHovered;
    private bool isLocked = false;
    private Vector3 targetRelativePosToCam = Vector3.zero;
    private Vector3 targetLastRelativePosToCam = Vector3.zero;
    private float targetLastEulerAngle;
    private Vector3 targetRelativePosToCursor = Vector3.zero;
    private float targetRelativeAngToCam;
    private Vector3 targetPosOffset = Vector3.zero;
    private float targetAngOffset;

    private UnityEngine.UI.Image myImage;

    public Transform lockedTarget = null;

	// Use this for initialization
	void Start () {
        myImage = GetComponent<UnityEngine.UI.Image>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Camera.main.GetComponent<CameraFollow>().myTargets[0] != null)
        {
            Vector3 forward = Camera.main.GetComponent<CameraFollow>().myTargets[0].transform.forward;
            transform.localPosition = 1500f * new Vector3(forward.x, forward.z, 0f);
        }
        //myImage.sprite = cursorIdle;
        //if (!isLocked)
        //    transform.position += sensitivity * new Vector3(Input.GetAxisRaw("CursorX"), Input.GetAxisRaw("CursorY"), 0f);
        //hoverHandler();
        //lockHandler();
        //constrain();
        //if (Input.GetMouseButtonDown(0) && lockMouse)
        //{
        //    if (Cursor.lockState != CursorLockMode.Locked)
        //    {
        //        Cursor.lockState = CursorLockMode.Locked;
        //        Cursor.visible = false;
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
	}

    private void constrain()
    {
        if (transform.position.x < 0f)
            transform.position = new Vector3(0f, transform.position.y, transform.position.z);
        if (transform.position.x > Screen.width)
            transform.position = new Vector3(Screen.width, transform.position.y, transform.position.z);
        if (transform.position.y < 0f)
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        if (transform.position.y > Screen.height)
            transform.position = new Vector3(transform.position.x, Screen.height, transform.position.z);
    }

    private void hoverHandler()
    {
        Vector3 myWorldPoint = Camera.main.ScreenToWorldPoint(transform.position);
        Debug.DrawLine(myWorldPoint, myWorldPoint - 100f * Vector3.up, Color.green);
        if (Physics.Raycast(myWorldPoint, -Vector3.up, out hit, 100.0F))
        {
            isHovered = true;
            Debug.DrawLine(myWorldPoint, hit.point, Color.red);
            myImage.sprite = cursorHover;
        }
        else
        {
            isHovered = false;
        }
    }

    private void lockHandler()
    {
        if (Input.GetAxisRaw("CursorLock") == 1.0f && isHovered & !isLocked) {
            isLocked = true;
            lockedTarget = hit.transform;
            targetRelativePosToCam = Camera.main.WorldToScreenPoint(lockedTarget.position);
            targetRelativePosToCam.z = 0f;
            targetLastRelativePosToCam = targetRelativePosToCam;
            targetLastEulerAngle = lockedTarget.eulerAngles.y;
            targetRelativePosToCursor = transform.position - targetRelativePosToCam;
            targetRelativePosToCursor.z = 0f;
            targetRelativeAngToCam = Mathf.Atan2(targetRelativePosToCursor.y, targetRelativePosToCursor.x);
            targetPosOffset = Vector3.zero;
            targetAngOffset = 0f;
        }
        else if (Input.GetAxisRaw("CursorLock") == 0.0f && isLocked)
        {
            lockedTarget = null;
            isLocked = false;
        }
        if (isLocked)
        {
            if (!trackRotation)
                targetLastEulerAngle = lockedTarget.eulerAngles.y;
            if (adjustingAllowed)
                targetPosOffset = sensitivity * new Vector3(Input.GetAxisRaw("CursorX"), Input.GetAxisRaw("CursorY"), 0f);

            targetRelativePosToCam = Camera.main.WorldToScreenPoint(lockedTarget.position);
            targetRelativePosToCam.z = 0f;
            targetRelativePosToCursor = transform.position + targetPosOffset - targetRelativePosToCam;
            targetRelativePosToCursor.z = 0f;
            targetRelativeAngToCam = Mathf.Atan2(targetRelativePosToCursor.y, targetRelativePosToCursor.x);

            targetAngOffset = (lockedTarget.eulerAngles.y - targetLastEulerAngle) * Mathf.Deg2Rad;
            transform.position = 2f * targetRelativePosToCam - targetLastRelativePosToCam + (targetRelativePosToCursor.magnitude * new Vector3(Mathf.Cos(targetRelativeAngToCam - targetAngOffset), Mathf.Sin(targetRelativeAngToCam - targetAngOffset), 0f));

            myImage.sprite = cursorHover;

            targetLastRelativePosToCam = targetRelativePosToCam;
            targetLastEulerAngle = lockedTarget.eulerAngles.y;
        }
    }
}
