using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour
{
    [SerializeField] private  GameObject housePrefab;

    private Vector3 mousePosOnClick;
    private float moveX = 0.0f;
    private float speedH = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(0))
        {
            Debug.Log("CLICKED");
            moveX = speedH * Input.GetAxis("Mouse X");
            housePrefab.transform.Rotate(Vector3.down, moveX);
        }



    }
}
