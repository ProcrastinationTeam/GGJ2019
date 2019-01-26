using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//[Serializable]
//public class Floooors
//{
//    public List<GameObject> listObject;
//}

public class FloorSpawningScript : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> floors;

    //[SerializeField]
    //public List<Floooors> flooooors;

    [SerializeField]
    public List<GameObject> RDC_Prefabs;

    [Header("LOGIC")]

    private GameObject prefab;


    [SerializeField]
    private GameObject container;
    [SerializeField]
    private GameObject ground;

    [SerializeField]
    private int numberOfFloors;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFloors());
    }

    IEnumerator SpawnFloorsTest()
    {
        for (int i = 0; i < numberOfFloors; i++)
        {
            GameObject prefab = floors[UnityEngine.Random.Range(0, floors.Count - 1)];
            var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            obj.transform.parent = container.transform;

            obj.transform.position = new Vector3(ground.transform.position.x, ground.transform.position.y + 20, ground.transform.position.z);
            obj.transform.rotation = ground.transform.rotation;

            StorageScript.GM.floorNames.Add(prefab.name);
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Floors: [" + String.Join(", ", StorageScript.GM.floorNames));
    }

    IEnumerator SpawnFloors()
    {
        for (int i = 0; i < numberOfFloors; i++)
        {

            if(i < 10)
            {
                Debug.Log("INSTANCIATE RDC");
                prefab = RDC_Prefabs[UnityEngine.Random.Range(0, RDC_Prefabs.Count - 1)];
            }
            else
            {
                prefab = floors[UnityEngine.Random.Range(0, floors.Count - 1)];
            }
            
            var obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            obj.transform.parent = container.transform;

            obj.transform.position = new Vector3(ground.transform.position.x, ground.transform.position.y + 20, ground.transform.position.z);
            obj.transform.rotation = ground.transform.rotation;
            obj.transform.RotateAround(obj.GetComponent<BoxCollider>().bounds.center, Vector3.up, 90 * i);


            StorageScript.GM.floorNames.Add(prefab.name);
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Floors: [" + String.Join(", ", StorageScript.GM.floorNames));
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
