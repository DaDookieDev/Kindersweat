using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptorTile : MonoBehaviour
{
    private PlayerStats stat;
    public void Accept(Product product)
    {
        stat.money += 10;
        Destroy(product.gameObject);
    }

    public void Placed(FactoryTile myTile)
    {
        myTile.active = true;
        stat = GameObject.Find("PlayerManager").GetComponent<PlayerStats>();
        
    }
}
