using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlScript : MonoBehaviour
{

    [SerializeField] private  GameObject housePrefab;

    private Vector3 mousePosOnClick;
    private float moveX = 0.0f;
    private float speedH = 4.0f;


    [Header("Narrative")]
    [SerializeField]
    private Button goforward;

    public bool translateCamera = false;

    Vector3 nextCameraPos;
    private Vector3 velocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
       goforward.onClick.AddListener(GoForward);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            moveX = speedH * Input.GetAxis("Mouse X");
            housePrefab.transform.Rotate(Vector3.down, moveX);
        }

        if(translateCamera)
        {

            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, nextCameraPos, ref velocity, 1.0f);
            if(Mathf.Abs(Camera.main.transform.position.x - nextCameraPos.x) < 0.1)
            {
                translateCamera = false;
            }
        }
    }

    private void GoForward()
    {
        if(translateCamera == false)
        {
            nextCameraPos = new Vector3(Camera.main.transform.position.x + 20, Camera.main.transform.position.y, Camera.main.transform.position.z);
            translateCamera = true;
        }
       
    }
}
