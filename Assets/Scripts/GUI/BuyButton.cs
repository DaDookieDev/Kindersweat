using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyButton : MonoBehaviour
{
    public GameObject myColor;
    public TMP_Text nameText, worthText, costText;
    public bool isColor;
    public bool isClothing;

    private int cost;

    public Color currentColor;
    public Sprite currentSprite;

    public Image colorImage;

    public void Process(GameObject newColor)
    {
        if (isColor)
        {
        myColor = newColor;
        currentColor = myColor.GetComponent<ProductValue>().myColor;
        colorImage.color = currentColor;
        nameText.color = currentColor;
        worthText.text = "WORTH: " + myColor.GetComponent<ProductValue>().worth.ToString();
        costText.text = "COST: " + myColor.GetComponent<ProductValue>().cost.ToString();
        cost = myColor.GetComponent<ProductValue>().cost;

        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().money < myColor.GetComponent<ProductValue>().cost) costText.color = Color.red;
        else costText.color = Color.white;
        }
        else if (isClothing)
        {
            myColor = newColor;
            currentSprite = myColor.GetComponent<ProductValue>().myClothing;
            colorImage.sprite = currentSprite;
            nameText.color = currentColor;
            worthText.text = "WORTH: " + myColor.GetComponent<ProductValue>().worth.ToString();
            costText.text = "COST: " + myColor.GetComponent<ProductValue>().cost.ToString();
            cost = myColor.GetComponent<ProductValue>().cost;
            if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().money < myColor.GetComponent<ProductValue>().cost) costText.color = Color.red;
            else costText.color = Color.white;
        }
    }

    public void Buy()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().money >= cost)
        {
            GameObject.Find("PlayerManager").GetComponent<PlayerStats>().money -= cost;
            GameObject.Find("PlayerManager").GetComponent<PlayerStats>().buySound.Play();
            if (isColor)
            {
            int index = myColor.GetComponent<ProductValue>().id;
            GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedColors[index - 1] = myColor;
            Destroy(gameObject);
            }
            else if (isClothing)
            {
            int index = myColor.GetComponent<ProductValue>().id;
            GameObject.Find("PlayerManager").GetComponent<PlayerUnlocks>().ownedClothing[index - 1] = myColor;
            Destroy(gameObject);
            }
        }
        else GameObject.Find("PlayerManager").GetComponent<PlayerStats>().brokeSound.Play();
    }
}
