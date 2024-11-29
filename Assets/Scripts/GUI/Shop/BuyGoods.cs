using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyGoods : MonoBehaviour
{
    public TMP_Text priceText, countText;
    public int price, count, cost;
    public PlayerStats stats;
    
    void Update()
    {
        if (stats.money < price) priceText.color = Color.red;
        else priceText.color = Color.white;
    }

    public void ValueChanged(Slider slide)
    {
        int newValue = (int) slide.value;
        count = newValue * 10;
        price = count * cost;
        countText.text = "COUNT: " + count.ToString();
        priceText.text = "PRICE:  " + price.ToString();
    }

    public void Buy(string type)
    {
        if (stats.money >= price)
        {
            stats.buySound.Play();
            stats.money -= price;
            if (type == "Cotton") stats.cotton += count;
            if (type == "White") stats.whiteDye += count;
            if (type == "Black") stats.blackDye += count;
            if (type == "Blue") stats.blueDye += count;
            if (type == "Red") stats.redDye += count;
            if (type == "Yellow") stats.yellowDye += count;
        }
        else stats.brokeSound.Play();
    }
}
