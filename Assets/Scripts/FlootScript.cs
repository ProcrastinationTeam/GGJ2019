using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlootScript : MonoBehaviour
{
    private Renderer rendered;
    private Color defaultColor;
    private Color hightlightColor = Color.yellow;

    [SerializeField]
    private AudioClip initBuildingSound;

    [SerializeField]
    private AudioSource audioS;

    void Start()
    {
        rendered = GetComponent<Renderer>();
        defaultColor = rendered.material.color;
    }

    void OnCollisionEnter(Collision collision)
    {
        audioS.PlayOneShot(initBuildingSound);
        StartCoroutine(Stuck());
    }

    IEnumerator Stuck()
    {
        if(true)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            yield return new WaitForSeconds(2);
            rb.isKinematic = true;
        } else
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            yield return new WaitForSeconds(1);
        }
        
    }

    void OnMouseOver()
    {
        rendered.material.color = hightlightColor;
    }

    void OnMouseExit()
    {
        rendered.material.color = defaultColor;
    }

    void OnMouseDown()
    {
        defaultColor = hightlightColor;
        rendered.material.color = defaultColor;
        Debug.Log(name);
    }
}
