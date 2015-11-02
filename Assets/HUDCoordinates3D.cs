using UnityEngine;
using System.Collections;

public class HUDCoordinates3D : MonoBehaviour
{
    public TextMesh text;
    public bool updating = true;
    public float refreshRate = 1.0f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(updateText());
    }


    IEnumerator updateText()
    {
        text.text = "X:" + ((int)Camera.main.transform.position.x).ToString() + "  Y:" + ((int)Camera.main.transform.position.z).ToString();
        yield return new WaitForSeconds(refreshRate);
        if (updating)
            StartCoroutine(updateText());
    }
}
