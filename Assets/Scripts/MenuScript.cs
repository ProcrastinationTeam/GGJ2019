using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField]
    private AudioSource launchSound;
    [SerializeField]
    private AudioClip launchS;
    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame()
    {
        launchSound.PlayOneShot(launchS);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
