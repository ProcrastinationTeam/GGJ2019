﻿using System;
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

    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFloors());
    }

    IEnumerator SpawnFloors()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}