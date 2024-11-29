using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PackagerPanel : MonoBehaviour
{
    public PackagerTile myTile;
    private bool canAccept;

    public GameObject acceptButton;

    private bool opened;

    void Start()
    {
        canAccept = true;
    }

    public void ChangeAccept()
    {
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        if (canAccept)
        {
            canAccept = false;
            acceptButton.GetComponent<Image>().color = Color.green;
            acceptButton.transform.Find("AcceptText").GetComponent<TMP_Text>().text = "START CONVEYOR";
        }
        else
        {
            canAccept = true;
            acceptButton.GetComponent<Image>().color = Color.red;
            acceptButton.transform.Find("AcceptText").GetComponent<TMP_Text>().text = "STOP CONVEYOR";
        }

        myTile.accepting = canAccept;
    }

    public void OpenPanel()
    {
        transform.Find("Screen").gameObject.SetActive(true);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = gameObject;
        opened = true;
    }

    public void ClosePanel()
    {
        transform.Find("Screen").gameObject.SetActive(false);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI == gameObject) GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = null;
        opened = false;
    }

    void Update()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI != gameObject && opened) ClosePanel();
    }
}
