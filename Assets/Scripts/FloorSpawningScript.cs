using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FloorSpawningScript : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] private List<GameObject> RDC_Prefabs;
    [SerializeField] private List<GameObject> Etage_Prefabs;
    [SerializeField] private List<GameObject> Toit_Prefabs;
    [SerializeField] private int numberOfMiddleFloors;

    [Header("Containers")]
    [SerializeField] public Transform FirstContainer;
    [HideInInspector] public List<Transform> Containers = new List<Transform>();
    [SerializeField] private GameObject ContainerNextPrefab;

    private float distanceBetweenContainers = 30;

    [HideInInspector] public List<string> floorNames = new List<string>();
    [HideInInspector] public List<GameObject> selectedFloors = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTower());
        for(int i = 0; i < numberOfMiddleFloors + 1; i++)
        {
            GameObject container = PrefabUtility.InstantiatePrefab(ContainerNextPrefab) as GameObject;
            container.transform.position = new Vector3(FirstContainer.position.x + (distanceBetweenContainers * (i + 1)), 0, 5);
            container.transform.rotation = FirstContainer.rotation;
            Containers.Add(container.transform);
        }
    }

    IEnumerator SpawnTower()
    {
        Transform firstGround = FirstContainer.transform.GetChild(0).transform;

        // Spawn RDC

        // Spawn others

        // Spawn toit

        for (int i = 0; i < numberOfMiddleFloors + 2; i++)
        {
            GameObject prefab = RDC_Prefabs[UnityEngine.Random.Range(0, RDC_Prefabs.Count - 1)];

            GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
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
                if (i == 0)
                {
                    foreach(GameObject tempObject in RDC_Prefabs)
                    {
                        if(tempObject.name == selectedFloors[i].name)
                        {
                            prefab = tempObject;
                            break;
                        }
                    }
                } else if(i < numberOfMiddleFloors + 1)
                {
                    foreach (GameObject tempObject in RDC_Prefabs) // TODO: remettre RDC_Prefabs
                    {
                        if (tempObject.name == selectedFloors[i].name)
                        {
                            prefab = tempObject;
                            break;
                        }
                    }
                } else
                {
                    foreach (GameObject tempObject in RDC_Prefabs) // TODO: remettre Toit_Prefabs
                    {
                        if (tempObject.name == selectedFloors[i].name)
                        {
                            prefab = tempObject;
                            break;
                        }
                    }
                }

                GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                obj.transform.parent = ground;

                obj.transform.position = new Vector3(ground.position.x, ground.position.y + 5, ground.position.z);
                obj.transform.rotation = ground.rotation;
                // obj.transform.RotateAround(obj.GetComponent<BoxCollider>().bounds.center, Vector3.up, 90 * i);
            }

            floorNames.Add(prefab.name);
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

            if(floor == 0)
            {
                // Debug.Log("INSTANCIATE RDC");
                prefab = RDC_Prefabs[UnityEngine.Random.Range(0, RDC_Prefabs.Count - 1)];
            } else if (floor < numberOfMiddleFloors + 1)
            {
                // Debug.Log("INSTANCIATE ETAGES " + (i + 1));
                prefab = Etage_Prefabs[UnityEngine.Random.Range(0, RDC_Prefabs.Count - 1)];
            } else
            {
                // Debug.Log("INSTANCIATE TOIT");
                prefab = Toit_Prefabs[UnityEngine.Random.Range(0, RDC_Prefabs.Count - 1)];
            }

            GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            obj.transform.parent = ground;

            obj.transform.position = new Vector3(ground.position.x, ground.position.y + 10, ground.position.z);
            obj.transform.rotation = ground.rotation;
            // obj.transform.RotateAround(obj.GetComponent<BoxCollider>().bounds.center, Vector3.up, 90 * i);

            yield return new WaitForSeconds(0.1f);
        }
}
