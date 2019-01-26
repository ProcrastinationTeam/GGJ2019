using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageScript : MonoBehaviour
{
    public static StorageScript GM;

    public List<string> floorNames = new List<string>();

    void Awake()
    {
        if (GM != null)
            Destroy(GM);
        else
            GM = this;

        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
