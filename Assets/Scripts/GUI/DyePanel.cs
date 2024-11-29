using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DyePanel : MonoBehaviour
{
    public Color color;
    public int col;
    private GameObject values;

    public GameObject button;
    public GameObject childButton;
    public GameObject buttonFolder;
    public GameObject childButtonFolder;

    public GameObject sleepSlider;

    public TMP_Text sleepText;

    public GameObject colorPanel, mainPanel, childPanel;

    public DyerTile dyeTile;

    public Image colorLogo;

    public GameObject childLogo;

    private bool opened;

    void Start()
    {
        col = 1;
    }

    public void Build()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int y = 0; y < 4; y++)
            {
                int currentID = (i * 4) + (y + 1);
                ValueList list = GameObject.Find("ProductValueList").GetComponent<ValueList>();
                Color newColor = new Color(0, 0, 0);
                bool hasUnlocked = false;
                int white = 0, black = 0, blue = 0, red = 0, yellow = 0;
                for (int z = 0; z < list.colors.Length; z++)
                {
                    if (list.colors[z].GetComponent<ProductValue>().id == currentID)
                    {
                            if (GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedColors[z] != null && GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedColors[z].GetComponent<ProductValue>().id == currentID)
                            {
                                hasUnlocked = true;
                                newColor = list.colors[z].GetComponent<ProductValue>().myColor;
                                white = list.colors[z].GetComponent<ProductValue>().white;
                                black = list.colors[z].GetComponent<ProductValue>().black;
                                blue = list.colors[z].GetComponent<ProductValue>().blue;
                                red = list.colors[z].GetComponent<ProductValue>().red;
                                yellow = list.colors[z].GetComponent<ProductValue>().yellow;
                            }
                    }
                }
                if (hasUnlocked)
                {
                GameObject newColorButton = Instantiate(button, new Vector3((y * 55) - 165, 270 - (i * 60), 0), Quaternion.identity);
                newColorButton.transform.parent = buttonFolder.transform;
                newColorButton.transform.Find("Color").GetComponent<Image>().color = newColor;
                newColorButton.transform.Find("WhiteInd").GetChild(0).GetComponent<TMP_Text>().text = white.ToString();
                newColorButton.transform.Find("BlackInd").GetChild(0).GetComponent<TMP_Text>().text = black.ToString();
                newColorButton.transform.Find("BlueInd").GetChild(0).GetComponent<TMP_Text>().text = blue.ToString();
                newColorButton.transform.Find("RedInd").GetChild(0).GetComponent<TMP_Text>().text = red.ToString();
                newColorButton.transform.Find("YellowInd").GetChild(0).GetComponent<TMP_Text>().text = yellow.ToString();
                

                newColorButton.SetActive(true);
                }
            }
        }
    }

    public void BuildChild()
    {
        for (int i = 0; i < 25; i++)
        {
            GameObject getChild = GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedChildren[i];
            if (getChild != null && getChild.GetComponent<Child>().used == false)
            {
                GameObject newChild = Instantiate(childButton, new Vector3(0, 0, 0), Quaternion.identity);
                newChild.transform.Find("CooldownText").GetComponent<TMP_Text>().text = "Cooldown: " + getChild.GetComponent<Child>().cooldown;
                newChild.transform.Find("RechargeText").GetComponent<TMP_Text>().text = "Recharge: " + getChild.GetComponent<Child>().recharge;
                newChild.transform.parent = childButtonFolder.transform;
                newChild.GetComponent<Button>().onClick.AddListener(() => NewChild(getChild));
                newChild.SetActive(true);
            }
        }
    }

    public void CloseChildPanel()
    {
        foreach(Transform child in childButtonFolder.transform)
        {
            if (child.name != "DeselectButton") Destroy(child.gameObject);
        }
        childPanel.SetActive(false);
    }

    public void CloseColorPanel()
    {
        foreach(Transform child in buttonFolder.transform)
        {
            Destroy(child.gameObject);
        }
        colorPanel.SetActive(false);
        CloseChildPanel();
    }

    public void OpenColorPanel()
    {
        Build();
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        colorPanel.SetActive(true);
        mainPanel.SetActive(false);
        //CloseChildPanel();
    }

    public void OpenChildPanel()
    {
        BuildChild();
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        childPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void OpenMainPanel()
    {
        mainPanel.SetActive(true);
        gameObject.SetActive(true);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = gameObject;
        opened = true;
        CloseColorPanel();
        CloseChildPanel();
    }

    public void CloseMainPanel()
    {
        mainPanel.SetActive(false);
        gameObject.SetActive(false);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        opened = false;
        CloseColorPanel();
        CloseChildPanel();
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI == gameObject) GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = null;
    }

    public void NewColor(Image myButton)
    {
        color = myButton.color;
        ValueList list = GameObject.Find("ProductValueList").GetComponent<ValueList>();
        for (int z = 0; z < list.colors.Length; z++)
                {
                    if (list.colors[z].GetComponent<ProductValue>().myColor == color)
                    {
                        dyeTile.color = z + 1;
                        dyeTile.colorValue = list.colors[z].GetComponent<ProductValue>();
                        dyeTile.realColor = color;
                    }
                }
        colorLogo.color = color;
        OpenMainPanel();
    }

    public void NewChild(GameObject myChild)
    {
        dyeTile.PlaceChild(myChild.GetComponent<Child>());
        childLogo.SetActive(true);
        OpenMainPanel();
    }

    void Update()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI != gameObject && opened) CloseMainPanel();
        if (dyeTile.child)
        {
            sleepSlider.SetActive(true);
            sleepText.gameObject.SetActive(true);
            sleepSlider.GetComponent<Slider>().value = dyeTile.currentWorkLoad;
            sleepText.text = "Remaining Work Load: " + dyeTile.currentWorkLoad;
            childLogo.SetActive(true);
        }
        else
        {
            childLogo.SetActive(false);
            sleepSlider.SetActive(false);
            sleepText.gameObject.SetActive(false);
        }
    }
}
