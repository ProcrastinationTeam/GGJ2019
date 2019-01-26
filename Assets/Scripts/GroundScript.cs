using System.Collections;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
    private Renderer rendered;
    private Color defaultColor;
    private Color hightlightColor = Color.yellow;
    private ControlScript controlScript;

    void Start()
    {
        rendered = GetComponent<Renderer>();
        defaultColor = rendered.material.color;
        controlScript = GameObject.Find("Manager").GetComponent<ControlScript>();
    }

    void OnMouseOver()
    {
        if (controlScript.canSelectGround)
        {
            rendered.material.color = hightlightColor;
        }
    }

    void OnMouseExit()
    {
        if (controlScript.canSelectGround)
        {
            rendered.material.color = defaultColor;
        }
    }

    void OnMouseDown()
    {
        if(controlScript.canSelectGround)
        {
            defaultColor = hightlightColor;
            rendered.material.color = defaultColor;
            StartCoroutine(controlScript.SelectedGround(name));
        }
    }
}
