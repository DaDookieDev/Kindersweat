using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsPanel : MonoBehaviour
{
    public GameObject mainPanel, settingsPanel, tutorialPanel;

    private bool opened;

    public void Resume()
    {
        opened = false;
        gameObject.SetActive(false);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI == gameObject) GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = null;
    }

    public void MainMenu()
    {
        opened = true;
        gameObject.SetActive(true);
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = gameObject;
    }

    public void Settings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
    }

    public void Tutorial()
    {
        mainPanel.SetActive(false);
        tutorialPanel.SetActive(true);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
    }

    public void Quit()
    {
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI != gameObject && opened) Resume();
    }
}
