using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackagerTile : MonoBehaviour
{
    private PlayerStats stat;
    public GameObject[] flaps;
    public bool front, back, left, right;

    public Product myProduct;

    public bool accepting;

    public void Accept(Product product)
    {
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().packagedSound.Play();
        ValueList valueList = GameObject.Find("ProductValueList").GetComponent<ValueList>();

        int colorValue = 0;
        int clothingValue = 0;

        bool isTrend = false;

        for (int i = 0; i < valueList.colors.Length; i++)
        {
            if (valueList.colors[i].GetComponent<ProductValue>().id == product.color)
            {
                colorValue = valueList.colors[i].GetComponent<ProductValue>().worth;
                if (i == GameObject.Find("PlayerManager").GetComponent<PlayerStats>().currentColorTrend) isTrend = true;
            }
        }

        for (int y = 0; y < valueList.clothing.Length; y++)
        {
            if (valueList.clothing[y].GetComponent<ProductValue>().id == product.shape)
            {
                clothingValue = valueList.clothing[y].GetComponent<ProductValue>().worth;
                if (y == GameObject.Find("PlayerManager").GetComponent<PlayerStats>().currentClothingTrend && isTrend == true) isTrend = true;
                else isTrend = false;
            }
        }

        int multiply = 1;
        if (isTrend) multiply = 2;
        stat.money += ((5 + colorValue + clothingValue) * multiply);
        Destroy(product.gameObject);
    }

    public void Placed(FactoryTile myTile)
    {
        myTile.active = true;
        stat = GameObject.Find("PlayerManager").GetComponent<PlayerStats>();
        transform.Find("Arrows").GetComponent<GreenArrows>().CloseArrows();

        for (int i = 0; i < flaps.Length; i++)
        {
            flaps[i].GetComponent<Animation>().Play("PackagerFlapExtension");
        }
    }

    void Update()
    {
        if (myProduct && myProduct.transform.position.x == transform.position.x && myProduct.transform.position.y == transform.position.y) Accept(myProduct);
    }

}
