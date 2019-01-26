using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlootScript : MonoBehaviour
{
    private Renderer rend;
    private Color color;

    private Color hightlightColor = Color.yellow;

    void Start()
    {
        rend = GetComponent<Renderer>();
        color = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //void OnTriggerEnter(Collider collider)
    //{
    //    StartCoroutine(Stuck());
    //}

    void OnCollisionEnter(Collision collision)
    {
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
        rend.material.color = hightlightColor;
    }

    void OnMouseExit()
    {
        rend.material.color = color;
    }

    void OnMouseDown()
    {
        color = hightlightColor;
        rend.material.color = color;
        Debug.Log(name);
    }
}
