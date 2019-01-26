using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlootScript : MonoBehaviour
{



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
}
