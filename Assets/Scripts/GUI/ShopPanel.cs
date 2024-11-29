using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopPanel : MonoBehaviour
{
    public GameObject openPanel;

    public GameObject colorButton;
    public GameObject clothingButton;

    public GameObject childPrefab;

    public TMP_Text chargeText, cooldownText, costText;

    public Transform mainShop;

    public TMP_Text titleText;

    private float cooldown;
    private int charge;
    private int cost;

    private bool opened;

    public PlayerUnlocks unlocks;

    public AudioSource buySound, brokeSound, badSound;

    void Update()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI != gameObject && opened) Close();
    }

    public void Open(GameObject openType)
    {
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        Reset();
        if (openPanel != null) openPanel.SetActive(false);
        openType.SetActive(true);
        openPanel = openType;

        if (GameObject.Find("PlayerManager").GetComponent<PlayerGrid>().placing) GameObject.Find("PlayerManager").GetComponent<PlayerGrid>().Deselect(true);

        if (openPanel.name == "GoodsPanel") titleText.text = "Goods";
        if (openPanel.name == "ColorsPanel")
        {
            CreateColor();
            titleText.text = "Colors";
        }
        if (openPanel.name == "ClothingPanel")
        {
            CreateClothing();
            titleText.text = "Clothing";
        }
        if (openPanel.name == "ChildrenPanel")
        {
            GenerateChild();
            titleText.text = "Kinder";
        }

        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = gameObject;
        opened = true;
    }

    public void CreateColor()
    {
        ValueList list = GameObject.Find("ProductValueList").GetComponent<ValueList>();
        PlayerUnlocks unlocks = GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>();

        for (int x = 0; x < list.colors.Length; x++)
        {
            if (unlocks.ownedColors[x] == null)
            {
                Transform newButton = Instantiate(colorButton, transform.position, Quaternion.identity).transform;
                newButton.parent = mainShop.Find("ColorsPanel").Find("Screen").Find("Buttons");
                newButton.GetComponent<BuyButton>().isColor = true;
                newButton.GetComponent<BuyButton>().Process(list.colors[x]);
            }
        }
    }

    public void GenerateChild()
    {
        cooldown = Mathf.Round(Random.Range(1.0f, 6.0f) * 10) / 10;
        charge = Random.Range(1, 10);
        int minimumCost = (int) (Mathf.Abs(7 - cooldown) * charge * 10);
        cost = Random.Range(minimumCost, (minimumCost * 3));

        cooldownText.text = "Cooldown: " + cooldown.ToString() + "s";
        chargeText.text = "Recharge: " + charge.ToString() + " per 5 seconds";
        costText.text = "Cost: " + cost.ToString();
    }

    public void BuyChild()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().money >= cost)
        {
            buySound.Play();
            GameObject.Find("PlayerManager").GetComponent<PlayerStats>().money -= cost;
            GameObject newChild = Instantiate(childPrefab, new Vector3(0, 0, 0), Quaternion.identity).gameObject;
            newChild.transform.parent = GameObject.Find("Children").transform;
            Child childScript = newChild.GetComponent<Child>();
            childScript.recharge = charge;
            childScript.cooldown = cooldown;
            childScript.used = false;
            bool placed = false;
            for (int i = 0; i < GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedChildren.Length; i++)
            {
                if (!placed)
                {
                    if (GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedChildren[i] == null)
                    {
                        placed = true;
                        GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedChildren[i] = newChild;
                    }
                }
            }
            GenerateChild();
        }
        else brokeSound.Play();
    }

    public void PassChild()
    {
        badSound.Play();
        GenerateChild();
    }

    public void CreateClothing()
    {
        ValueList list = GameObject.Find("ProductValueList").GetComponent<ValueList>();
        PlayerUnlocks unlocks = GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>();

        for (int x = 0; x < list.clothing.Length; x++)
        {
            if (unlocks.ownedClothing[x] == null)
            {
                Transform newButton = Instantiate(clothingButton, transform.position, Quaternion.identity).transform;
                newButton.parent = mainShop.Find("ClothingPanel").Find("Screen").Find("Buttons");
                newButton.GetComponent<BuyButton>().isClothing = true;
                newButton.GetComponent<BuyButton>().Process(list.clothing[x]);
            }
        }
    }

    private void Reset()
    {
        foreach(Transform child in mainShop.Find("ColorsPanel").Find("Screen").Find("Buttons"))
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in mainShop.Find("ClothingPanel").Find("Screen").Find("Buttons"))
        {
            Destroy(child.gameObject);
        }
    }

    public void Close()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI == gameObject) GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI = null;
        opened = false;
        gameObject.SetActive(false);
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
    }
}
