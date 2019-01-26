using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FloorSpawningScript : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> floors;

    [SerializeField]
    private GameObject container;
    [SerializeField]
    private GameObject ground;

    [SerializeField]
    private int numberOfFloors;

    private List<string> floorNames = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfFloors; i++)
        {
            GameObject prefab = floors[UnityEngine.Random.Range(0, floors.Count - 1)];
            var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            obj.transform.parent = container.transform;

            obj.transform.position = new Vector3(ground.transform.position.x, ground.transform.position.y + (20 * (i + 1)), ground.transform.position.z);

            floorNames.Add(prefab.name);
        }

        Debug.Log("Floors: [" + String.Join(", ", floorNames));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
