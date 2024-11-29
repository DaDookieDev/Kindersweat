using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FabricatorPanel : MonoBehaviour
{
    public Sprite sprite;
    public int shape;
    private GameObject values;

    public GameObject button;
    public GameObject buttonFolder;
    public GameObject childButton;
    public GameObject childButtonFolder;

    public GameObject clothingLogo;

    public GameObject fabPanel, mainPanel, childPanel;

    public FabricatorTile fabTile;

    public GameObject childLogo;

    public GameObject sleepSlider;

    public TMP_Text sleepText;

    private bool opened;

    void Start()
    {
        shape = 0;
    }

    public void Build()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int y = 0; y < 2; y++)
            {
                int currentID = (i * 2) + (y + 1);
                ValueList list = GameObject.Find("ProductValueList").GetComponent<ValueList>();
                Sprite newSprite = null;
                int usage = 0;
                bool hasUnlocked = false;
                for (int z = 0; z < list.clothing.Length; z++)
                {
                    if (list.clothing[z].GetComponent<ProductValue>().id == currentID)
                    {
                            if (GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedClothing[z] != null && GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedClothing[z].GetComponent<ProductValue>().id == currentID)
                            {
                                hasUnlocked = true;
                                newSprite = list.clothing[z].GetComponent<ProductValue>().myClothing;
                                usage = list.clothing[z].GetComponent<ProductValue>().cotton;
                            }
                    }
                }
                if (hasUnlocked)
                {
                GameObject newFabButton = Instantiate(button, new Vector3((y * 55) - 165, 270 - (i * 60), 0), Quaternion.identity);
                newFabButton.transform.parent = buttonFolder.transform;
                newFabButton.transform.Find("Clothing").GetComponent<Image>().sprite = newSprite;
                newFabButton.transform.Find("CottonUsage").Find("Text").GetComponent<TMP_Text>().text = usage.ToString();
                newFabButton.SetActive(true);
                }
            }
        }
    }

    public void CloseClothingPanel()
    {
        foreach(Transform child in buttonFolder.transform)
        {
            Destroy(child.gameObject);
        }
        fabPanel.SetActive(false);
    }

    public void OpenClothingPanel()
    {
        Build();
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        fabPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void OpenMainPanel()
    {
        mainPanel.SetActive(true);
        gameObject.SetActive(true);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = gameObject;
        opened = true;
        CloseClothingPanel();
        CloseChildPanel();
    }

    public void CloseMainPanel()
    {
        mainPanel.SetActive(false);
        gameObject.SetActive(false);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        opened = false;
        CloseClothingPanel();
        CloseChildPanel();
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI == gameObject) GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = null;
    }

    public void NewClothing(Image myButton)
    {
        sprite = myButton.sprite;
        ValueList list = GameObject.Find("ProductValueList").GetComponent<ValueList>();
        for (int z = 0; z < list.clothing.Length; z++)
                {
                    if (list.clothing[z].GetComponent<ProductValue>().myClothing == sprite)
                    {
                        fabTile.fabric = z + 1;
                        fabTile.realShape = sprite;
                        fabTile.neededCotton = list.clothing[z].GetComponent<ProductValue>().cotton;
                    }
                }
        clothingLogo.SetActive(true);
        clothingLogo.GetComponent<Image>().sprite = sprite;
        OpenMainPanel();
    }

    public void NewChild(GameObject myChild)
    {
        fabTile.PlaceChild(myChild.GetComponent<Child>());
        childLogo.SetActive(true);
        OpenMainPanel();
    }

    public void CloseChildPanel()
    {
        foreach(Transform child in childButtonFolder.transform)
        {
            if (child.name != "DeselectButton") Destroy(child.gameObject);
        }
        childPanel.SetActive(false);
    }

    public void OpenChildPanel()
    {
        BuildChild();
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        childPanel.SetActive(true);
        mainPanel.SetActive(false);
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

    void Update()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI != gameObject && opened) CloseMainPanel();
        if (fabTile.child)
        {
            sleepSlider.SetActive(true);
            sleepText.gameObject.SetActive(true);
            sleepSlider.GetComponent<Slider>().value = fabTile.currentWorkLoad;
            sleepText.text = "Remaining Work Load: " + fabTile.currentWorkLoad;
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
