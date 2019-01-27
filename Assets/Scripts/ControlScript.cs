﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlScript : MonoBehaviour
{
    private Vector3 mousePosOnClick;
    private float moveX = 0.0f;
    private float speedH = 4.0f;

    [Header("Narrative")]
    [SerializeField] private Button goforward;

    public bool translateCamera = false;

    Vector3 nextCameraPos;
    private Vector3 velocity = Vector3.zero;
    private float velocitySize = 0;

    private float translationX = 30;
    private bool firstTime = true;

    private FloorSpawningScript floorSpawningScript;

    public bool canSelectGround = true;

    // -1 = base
    //  0 = RDC
    //  1 = RDC + 1er étage
    // etc
    private int currentCameraPosition = -1;

    bool firstTranslateCamera = false;
    float nextCameraSize = 5;

    // Start is called before the first frame update
    void Start()
    {
       goforward.onClick.AddListener(FirstGoForward);
       floorSpawningScript = GetComponent<FloorSpawningScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {

            moveX = speedH * Input.GetAxis("Mouse X");

            floorSpawningScript.FirstContainer.RotateAround(floorSpawningScript.FirstContainer.position, Vector3.up, moveX);
            foreach (Transform container in floorSpawningScript.Containers)
            {
                List<Transform> grounds = new List<Transform>(new Transform[] { container.GetChild(0), container.GetChild(1), container.GetChild(2), container.GetChild(3) });
                foreach(Transform ground in grounds)
                {
                    ground.RotateAround(ground.position, Vector3.up, moveX);
                }
            }
        }

        if(firstTranslateCamera)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, nextCameraPos, ref velocity, 1.0f);
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, nextCameraSize, ref velocitySize, 0.8f);

            if (Mathf.Abs(Camera.main.transform.position.y - nextCameraPos.y) < 0.1)
            {
                firstTranslateCamera = false;
                goforward.gameObject.SetActive(false);
                StartCoroutine(TempoAndGoForward());
            }
        }

        if(translateCamera)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, nextCameraPos, ref velocity, 0.5f);
            if(Mathf.Abs(Camera.main.transform.position.x - nextCameraPos.x) < 0.1)
            {
                translateCamera = false;
                currentCameraPosition++;
                canSelectGround = true; // TODO: attendre que tous les blocs soient tombés
                StartCoroutine(floorSpawningScript.SpawnFloor(currentCameraPosition));
            }
        }
    }

    private IEnumerator TempoAndGoForward()
    {
        // Faire tomber
        Transform container = floorSpawningScript.FirstContainer;
        Transform ground = container.GetChild(0);

        Rigidbody rbGround = ground.gameObject.GetComponent<Rigidbody>();
        rbGround.isKinematic = false;
        rbGround.constraints = RigidbodyConstraints.None;
        rbGround.AddForce(new Vector3(0, 1000, 0));
        for (int i = 0; i < ground.gameObject.transform.childCount; i++)
        {
            Rigidbody rb = ground.gameObject.transform.GetChild(i).GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
        }

        StartCoroutine(CleanStep(container));
        //

        yield return new WaitForSeconds(2.0f);
        GoForward();
    }

    private void GoForward()
    {
        if (!translateCamera) {
            // Le firstTime c'est parce que la première fois faut un peu plus translater
            nextCameraPos = new Vector3(Camera.main.transform.position.x + translationX + (firstTime ? 4 : 0) + 0.1f, Camera.main.transform.position.y, Camera.main.transform.position.z);
            translateCamera = true;
            firstTime = false;
        }
    }

    private void FirstGoForward()
    {
        goforward.gameObject.SetActive(false);
        firstTranslateCamera = true;
        nextCameraPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 3, Camera.main.transform.position.z);
    }

    public IEnumerator SelectedGround(string groundName)
    {
        canSelectGround = false;

        yield return new WaitForSeconds(0.5f);

        // Faire tomber le sol
        Transform container = floorSpawningScript.Containers[currentCameraPosition];
        List<Transform> grounds = new List<Transform>(new Transform[] { container.GetChild(0), container.GetChild(1), container.GetChild(2), container.GetChild(3) });
        foreach(Transform ground in grounds)
        {
            if(ground.name != groundName)
            {
                Rigidbody rbGround = ground.gameObject.GetComponent<Rigidbody>();
                rbGround.isKinematic = false;
                rbGround.constraints = RigidbodyConstraints.None;
                rbGround.AddForce(new Vector3(0, 2000 / 8 * (currentCameraPosition + 2), 0));
                for(int i = 0; i < ground.gameObject.transform.childCount; i++)
                {
                    Rigidbody rb = ground.gameObject.transform.GetChild(i).GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.constraints = RigidbodyConstraints.None;
                }
            } else
            {
                GameObject selectedFloor = ground.GetChild(ground.childCount - 1).gameObject;
                floorSpawningScript.selectedFloors.Add(selectedFloor);
            }
        }

       if (currentCameraPosition != floorSpawningScript.numberOfFloors - 1) {
            StartCoroutine(CleanStep(container));
            StartCoroutine(floorSpawningScript.SpawnSelectedFloors(currentCameraPosition + 1));
        }

        yield return new WaitForSeconds(2.0f);

        if(currentCameraPosition == floorSpawningScript.numberOfFloors - 1)
        {
            floorSpawningScript.EndGameUI(UnityEngine.Random.Range(0, 3));
        } else
        {
            GoForward();
        }
    }

    IEnumerator CleanStep(Transform container)
    {
        yield return new WaitForSeconds(3.0f);
        container.gameObject.SetActive(false);
    }
}
