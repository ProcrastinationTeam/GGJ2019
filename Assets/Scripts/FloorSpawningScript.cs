using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FloorSpawningScript : MonoBehaviour
{
    [SerializeField]
    private AudioClip initBuildingSound;

    [SerializeField]
    private AudioClip FallBuildingSound;

    [SerializeField]
    private AudioSource audioS;

    [Header("Narrative")]
    [SerializeField]
    private TMPro.TextMeshProUGUI endText;
    [SerializeField]
    private TMPro.TextMeshProUGUI startText;
    [SerializeField]
    private Button goforward;
    [SerializeField]
    private Text goForwardText;


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

    [HideInInspector] public List<GameObject> initialFloors = new List<GameObject>();
    [HideInInspector] public List<GameObject> selectedFloors = new List<GameObject>();

    public bool retryPossible = false;

    // Start is called before the first frame update
    void Start()
    {
        StartGameUI();
        // Physics.gravity = new Vector3(0, -15.0f, 0);
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
            
            initialFloors.Add(prefab);
            yield return new WaitForSeconds(0.5f);
            audioS.PlayOneShot(FallBuildingSound);
            yield return new WaitForSeconds(3.5f);
        }

        Debug.Log("Floors: [" + String.Join(", ", initialFloors));
    }

    // Spawn au prochain truc ceux précédemment
    public IEnumerator SpawnSelectedFloors(int floor)
    {
        Transform container = Containers[floor];
        List<Transform> grounds = new List<Transform>(new Transform[] { container.GetChild(0), container.GetChild(1), container.GetChild(2), container.GetChild(3) });

        for (int i = 0; i < floor; i++)
        {
            audioS.PlayOneShot(FallBuildingSound);
            GameObject prefab = null;

            foreach (Transform ground in grounds)
            {
                foreach (GameObject tempObject in allPrefabs)
                {
                    if (tempObject.name == selectedFloors[i].name.Replace("(Clone", ""))
                    {
                        prefab = tempObject;
                        break;
                    }
                }

                GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                obj.transform.parent = ground;

                obj.transform.position = new Vector3(ground.position.x, ground.position.y + 5, ground.position.z);
                obj.transform.rotation = ground.rotation;
                // obj.transform.RotateAround(obj.GetComponent<BoxCollider>().bounds.center, Vector3.up, 90 * i);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // Spawn le nouveau floor
    public IEnumerator SpawnFloor(int floor)
    {
        Transform container = Containers[floor];
        List<Transform> grounds = new List<Transform>(new Transform[] { container.GetChild(0), container.GetChild(1), container.GetChild(2), container.GetChild(3) });

        GameObject[] prefabs = new GameObject[4];
        int indexRightOne = UnityEngine.Random.Range(0, 4);
        for (int i = 0; i < 4; i++)
        {
            // audio source list
            audioS.PlayOneShot(FallBuildingSound);
            if (floor != 0 && i == indexRightOne)
            {
                foreach (GameObject tempObject in allPrefabs)
                {
                    if (tempObject == initialFloors[floor])
                    {
                        prefabs[i] = tempObject;
                        break;
                    }
                }
            }
            else
            {
                if (floor == 0)
                {
                    prefabs[i] = RDC_Prefabs[UnityEngine.Random.Range(0, RDC_Prefabs.Count - 1)];
                }
                else if (floor == numberOfFloors - 1)
                {
                    prefabs[i] = Toit_Prefabs[UnityEngine.Random.Range(0, Toit_Prefabs.Count - 1)];
                }
                else
                {
                    prefabs[i] = Etage_Prefabs[UnityEngine.Random.Range(0, Etage_Prefabs.Count - 1)];
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            {
                Transform ground = grounds[i];
                GameObject prefab = prefabs[i];

                GameObject obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                obj.transform.parent = ground;

                obj.transform.position = new Vector3(ground.position.x, ground.position.y + 10, ground.position.z);
                obj.transform.rotation = ground.rotation;
                // obj.transform.RotateAround(obj.GetComponent<BoxCollider>().bounds.center, Vector3.up, 90 * i);

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void EndGameUI(int endId)
    {
        StartCoroutine(FadeIn(endText));

        switch (endId)
        {
            case 0:
                endText.text = "It seems your home is the result of discovery and originality";
                break;
            case 1:
                endText.text = "Your home is a mix of nostalgia and curiosity, you explored new horizon";
                break;
            case 2:
                endText.text = "Your home is filled of tradition and you seem to latch on the memories within you";
                break;
            default:
                break;
        }
    }

    public void StartGameUI()
    {
        StartCoroutine(Story());
    }

    IEnumerator Story()
    {

        
        yield return new WaitForSeconds(2.5f);

        startText.text = "My house is like a cocoon";
        StartCoroutine(FadeIn(startText));
        
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeOut(startText));
        
        yield return new WaitForSeconds(2.0f);

        startText.text = "I like it there";
        StartCoroutine(FadeIn(startText));
        
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeOut(startText));
        
        yield return new WaitForSeconds(2.0f);

        startText.text = "But sometimes, I have to leave";
        StartCoroutine(FadeIn(startText));
        
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeOut(startText));
        yield return new WaitForSeconds(2.0f);

        startText.text = "And when I do";
        StartCoroutine(FadeIn(startText));
        
        yield return new WaitForSeconds(2.0f);
        StartCoroutine(FadeOut(startText));
        yield return new WaitForSeconds(2.0f);

        startText.text = "I try and picture it in my mind";
        StartCoroutine(FadeIn(startText));        
        yield return new WaitForSeconds(4.0f);
        StartCoroutine(FadeOut(startText));
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(FadeInButton(goForwardText));
    }

    IEnumerator FadeInButton(Text text)
    {
        goforward.gameObject.SetActive(true);
        for (float f = 0f; f <= 1; f += 0.01f)
        {
            var color = text.color;
            color.a = f;
            text.color = color;
            yield return null;
        }
    }

    IEnumerator FadeIn(TMPro.TextMeshProUGUI texto)
    {
        for (float f = 0f; f <= 1; f += Time.deltaTime)
        {
            var color = texto.color;
            color.a = f;
            texto.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOut(TMPro.TextMeshProUGUI texto)
	{
        for (float f = 1f; f >= 0; f -= Time.deltaTime)
        {
            var color = texto.color;
            color.a = f;
            texto.color = color;
            yield return null;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(retryPossible && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))) {
            SceneManager.LoadScene(1);
        }
    }


}
