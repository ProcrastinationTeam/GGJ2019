using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlScript : MonoBehaviour
{
    [SerializeField]
    private AudioClip goforwardSound;
    [SerializeField]
    private AudioClip selectionSound;
    [SerializeField]
    private AudioClip CameraSound;


    [SerializeField]
    private AudioSource audioS;


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

    bool lastTranslateCamera = false;

    [SerializeField] Image Circle;
    [SerializeField] Image Top;
    [SerializeField] Image Right;
    [SerializeField] Image Bottom;
    [SerializeField] Image Left;
    [SerializeField] Image AllBlack;

    bool goToBlack = false;

    bool rotateAuto = false;

    // Start is called before the first frame update
    void Start()
    {
       goforward.onClick.AddListener(FirstGoForward);
       floorSpawningScript = GetComponent<FloorSpawningScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && !rotateAuto)
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
            audioS.PlayOneShot(CameraSound);

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

        if (lastTranslateCamera)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, nextCameraPos, ref velocity, 2.0f);
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, nextCameraSize, ref velocitySize, 2.0f);

            rotateAuto = true;

            if (Mathf.Abs(Camera.main.transform.position.x - nextCameraPos.x) < 0.1)
            {
                lastTranslateCamera = false;
                StartCoroutine(OnceYouGoBlack());
            }
        }

        if(goToBlack)
        {
            float stepScale = Time.deltaTime * 0.7f;
            Circle.transform.localScale = new Vector3(Circle.transform.localScale.x - stepScale, Circle.transform.localScale.y - stepScale, 1);

            float alphaScale = Time.deltaTime / 3;
            

            float stepY = Time.deltaTime * 50;
            float stepX = Time.deltaTime * 100;
            if (Circle.transform.localScale.x < 4)
            {
                AllBlack.color = new Color(AllBlack.color.r, AllBlack.color.g, AllBlack.color.g, AllBlack.color.a + alphaScale);

                Left.transform.position = new Vector3(Left.transform.position.x + stepX, Left.transform.position.y, Left.transform.position.z);
                Right.transform.position = new Vector3(Right.transform.position.x - stepX, Right.transform.position.y, Right.transform.position.z);
            }
            if (Circle.transform.localScale.y < 3)
            {
                Bottom.transform.position = new Vector3(Bottom.transform.position.x, Bottom.transform.position.y + stepY, Bottom.transform.position.z);
                Top.transform.position = new Vector3(Top.transform.position.x, Top.transform.position.y - stepY, Top.transform.position.z);
            }
        }

        if(rotateAuto)
        {
            moveX = Time.deltaTime * 10;

            //floorSpawningScript.FirstContainer.RotateAround(floorSpawningScript.FirstContainer.position, Vector3.up, moveX);
            foreach (Transform container in floorSpawningScript.Containers)
            {
                List<Transform> grounds = new List<Transform>(new Transform[] { container.GetChild(0), container.GetChild(1), container.GetChild(2), container.GetChild(3) });
                foreach (Transform ground in grounds)
                {
                    ground.RotateAround(ground.position, Vector3.up, moveX);
                }
            }
        }
    }

    private IEnumerator OnceYouGoBlack()
    {
        yield return new WaitForSeconds(2);
        goToBlack = true;
        yield return new WaitForSeconds(13);
        SceneManager.LoadScene(0);
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
        Debug.Log("CLICK");
        audioS.PlayOneShot(goforwardSound);
        goforward.gameObject.SetActive(false);
        firstTranslateCamera = true;
        nextCameraPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + 3, Camera.main.transform.position.z);
    }

    public IEnumerator SelectedGround(string groundName)
    {
        audioS.PlayOneShot(selectionSound);
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

            lastTranslateCamera = true;
            nextCameraSize = 2;
            if(groundName == "Ground0")
            {
                nextCameraPos = new Vector3((30 * floorSpawningScript.numberOfFloors) - 1 - 6.5f, 11, -2.5f);
            }
            else if (groundName == "Ground1")
            {
                nextCameraPos = new Vector3((30 * floorSpawningScript.numberOfFloors) - 1 - 3.5f, 11, -5.5f);
            }
            else if (groundName == "Ground2")
            {
                nextCameraPos = new Vector3((30 * floorSpawningScript.numberOfFloors) - 1 - 0.5f, 11, -8.5f);
            }
            else
            {
                nextCameraPos = new Vector3((30 * floorSpawningScript.numberOfFloors) - 1 + 2.5f, 11, -11.5f);
            }
            // 89, 11, -5
            // Gauche => 82.5, 11, -2.5
            // Milieu gauche => 85.5, 11, -5.5
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
