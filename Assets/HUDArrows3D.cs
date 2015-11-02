using UnityEngine;
using System.Collections;

public class HUDArrows3D : MonoBehaviour
{
    public GameObject arrowPrefab;

    public float arrowRadius;
    public float arrowVisibleRange = 10f;

    public MouseInput cursor;

    private GameObject[] ships;
    private ArrayList arrows;
    private GameObject playerShip;
    // Use this for initialization
    void Start()
    {
        arrows = new ArrayList();
        playerShip = Camera.main.GetComponent<CameraFollow>().myTargets[0].gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerShip == null)
        {
            playerShip = Camera.main.GetComponent<CameraFollow>().myTargets[0].gameObject;
            return;
        }
        ships = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject ship in ships)
        {
            if ((ship.transform.position - playerShip.transform.position).magnitude < arrowVisibleRange)
                continue;
            if (ship == playerShip)
                continue;
            bool arrowFound = false;
            foreach (GUIHudArrow arrow in arrows)
            {
                if (arrow.targetShip == ship)
                {
                    if (ship.transform == cursor.lockedTarget)
                    {
                        arrow.transform.position = playerShip.transform.position + 1.0f * arrowRadius * (ship.transform.position - playerShip.transform.position).normalized;
                        arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, arrow.transform.localPosition.y, 0f);
                    }
                    else
                    {
                        arrow.transform.position = playerShip.transform.position + arrowRadius * (ship.transform.position - playerShip.transform.position).normalized;
                        arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, arrow.transform.localPosition.y, 0f);
                    }
                    arrowFound = true;
                }
            }
            if (!arrowFound)
            {
                GameObject newArrow = (GameObject)Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                newArrow.transform.parent = transform;

                newArrow.GetComponent<GUIHudArrow>().playerShip = playerShip;
                newArrow.GetComponent<GUIHudArrow>().targetShip = ship;
                if (ship.transform == cursor.lockedTarget)
                {
                    newArrow.transform.position = playerShip.transform.position + 1.0f * arrowRadius * (ship.transform.position - playerShip.transform.position).normalized;
                    newArrow.transform.localPosition = new Vector3(newArrow.transform.localPosition.x, newArrow.transform.localPosition.y, 0f);
                }
                else
                {
                    newArrow.transform.position = playerShip.transform.position + arrowRadius * (ship.transform.position - playerShip.transform.position).normalized;
                    newArrow.transform.localPosition = new Vector3(newArrow.transform.localPosition.x, newArrow.transform.localPosition.y, 0f);
                }
                arrows.Add(newArrow.GetComponent<GUIHudArrow>());
            }
        }
        ArrayList arrowsCopy = (ArrayList)arrows.Clone();
        foreach (GUIHudArrow arrow in arrowsCopy)
        {
            bool shipFound = false;
            foreach (GameObject ship in ships)
            {
                if (ship == arrow.targetShip && (ship.transform.position - playerShip.transform.position).magnitude >= arrowVisibleRange)
                    shipFound = true;
            }
            if (!shipFound)
            {
                Destroy(arrow.gameObject);
                arrows.Remove(arrow);
            }
        }
    }
}
