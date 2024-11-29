using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    public AudioSource pressSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        pressSound.Play();
        SceneManager.LoadScene("MainGame");
    }

    public void QuitGame()
    {
        pressSound.Play();
        Application.Quit();
    }
}
