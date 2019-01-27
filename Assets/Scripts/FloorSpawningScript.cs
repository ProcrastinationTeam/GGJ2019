using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorSpawningScript : MonoBehaviour
{
    [Header("Narrative")]
    [SerializeField]
    private TMPro.TextMeshProUGUI endText;

    [Header("Logic")]
    [SerializeField] private List<GameObject> RDC_Prefabs;
    [SerializeField] private List<GameObject> Etage_Prefabs;
    [SerializeField] private List<GameObject> Toit_Prefabs;
    private List<GameObject> allPrefabs = new List<GameObject>();
    [SerializeField] public int numberOfFloors;

    [Header("Containers")]
    [SerializeField] public Transform FirstContainer;
    [HideInInspector] public List<Transform> Containers = new List<Transform>();
    [SerializeField] private GameObject ContainerNextPrefab;

    private float distanceBetweenContainers = 30;

    [HideInInspector] public List<string> floorNames = new List<string>();
    [HideInInspector] public List<GameObject> selectedFloors = new List<GameObject>();

    public bool retryPossible = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -15.0f, 0);
        StartCoroutine(SpawnTower());
        for(int i = 0; i < numberOfFloors + 1; i++)
        {
            GameObject container = Instantiate(ContainerNextPrefab) as GameObject;
            container.transform.position = new Vector3(FirstContainer.position.x + (distanceBetweenContainers * (i + 1)), 0, 5);
            container.transform.rotation = FirstContainer.rotation;
            Containers.Add(container.transform);
        }
        allPrefabs.AddRange(RDC_Prefabs);
        allPrefabs.AddRange(Etage_Prefabs);
        allPrefabs.AddRange(Toit_Prefabs);
    }

    IEnumerator SpawnTower()
    {
        Transform firstGround = FirstContainer.transform.GetChild(0).transform;

        for (int i = 0; i < numberOfFloors; i++)
        {
            GameObject prefab = null;

            if (i == 0)
            {
                prefab = RDC_Prefabs[UnityEngine.Random.Range(0, RDC_Prefabs.Count - 1)];
            }
            else if (i == numberOfFloors - 1)
            {
                prefab = Toit_Prefabs[UnityEngine.Random.Range(0, Toit_Prefabs.Count - 1)];
            }
            else
            {
                prefab = Etage_Prefabs[UnityEngine.Random.Range(0, Etage_Prefabs.Count - 1)];
            }

            GameObject obj = Instantiate(prefab) as GameObject;
            obj.transform.parent = FirstContainer;

            obj.transform.position = new Vector3(firstGround.position.x, firstGround.position.y + 20, firstGround.position.z);
            obj.transform.rotation = firstGround.rotation;
            //obj.transform.RotateAround(obj.GetComponent<BoxCollider>().bounds.center, Vector3.up, 90 * i);

            floorNames.Add(prefab.name);
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Floors: [" + String.Join(", ", floorNames));
    }

    // Spawn au prochain truc ceux précédemment
    public IEnumerator SpawnSelectedFloors(int floor)
    {
        Transform container = Containers[floor];
        List<Transform> grounds = new List<Transform>(new Transform[] { container.GetChild(0), container.GetChild(1), container.GetChild(2), container.GetChild(3) });

        for (int i = 0; i < floor; i++)
        {
            GameObject prefab = null;

            foreach (Transform ground in grounds)
            {
                foreach(GameObject tempObject in allPrefabs)
                {
                    if(tempObject.name == selectedFloors[i].name.Replace("(Clone)", ""))
                    {
                        prefab = tempObject;
                        break;
                    }
                }
 
                GameObject obj = Instantiate(prefab) as GameObject;
                obj.transform.parent = ground;

                obj.transform.position = new Vector3(ground.position.x, ground.position.y + 5, ground.position.z);
                obj.transform.rotation = ground.rotation;
                // obj.transform.RotateAround(obj.GetComponent<BoxCollider>().bounds.center, Vector3.up, 90 * i);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // Spawn le nouveau floor
    public IEnumerator SpawnFloor(int floor)
    {
        Transform container = Containers[floor];
        List<Transform> grounds = new List<Transform>(new Transform[] { container.GetChild(0), container.GetChild(1), container.GetChild(2), container.GetChild(3) });

        foreach (Transform ground in grounds)
        {
            GameObject prefab = null;

            if (floor == 0)
            {
                prefab = RDC_Prefabs[UnityEngine.Random.Range(0, RDC_Prefabs.Count - 1)];
            }
            else if (floor == numberOfFloors - 1)
            {
                prefab = Toit_Prefabs[UnityEngine.Random.Range(0, Toit_Prefabs.Count - 1)];
            }
            else
            {
                prefab = Etage_Prefabs[UnityEngine.Random.Range(0, Etage_Prefabs.Count - 1)];
            }

            GameObject obj = Instantiate(prefab) as GameObject;
            obj.transform.parent = ground;

            obj.transform.position = new Vector3(ground.position.x, ground.position.y + 20, ground.position.z);
            obj.transform.rotation = ground.rotation;
            // obj.transform.RotateAround(obj.GetComponent<BoxCollider>().bounds.center, Vector3.up, 90 * i);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void EndGameUI(int endId)
    {
        StartCoroutine(Fade());

        switch (endId)
        {
            case 0:
                endText.text = "Il semblerait que ton foyer soit le fruit de la découverte et de l'originalité...";
                break;
            case 1:
                endText.text = "Ton foyer est le mélange de tes souvenirs et de l'ouverture vers de nouveaux horizons / tu as exploré de nouveau horizons";
                break;
            case 2:
                endText.text = "Ton foyer est emprunt de tradition et tu semble attacher à tes souvenirs";
                break;
            default:
                break;
        }
    }

    IEnumerator Fade()
    {
        for (float f = 0f; f <= 1; f += 0.001f)
        {
            var color = endText.color;
            color.a += f;
            endText.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        // TODO: afficher "entrée pour recommencer"
        retryPossible = true;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(retryPossible && (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Space))) {
            SceneManager.LoadScene(1);
        }
    }

}
