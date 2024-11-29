using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyerTile : MonoBehaviour
{
    public GameObject attacher;

    public GameObject gui;

    public Child child;

    public GameObject childSprite;
    public GameObject sleepSprite;

    public Product holdingProduct;

    public ProductValue colorValue;

    private int myRot;

    private float currentWait;
    public float totalWait;

    public int currentWorkLoad;
    private int rechargeLoad;

    private float rechargeTime;

    public int color;
    public Color realColor;

    public bool isActive;

    public void Placed(FactoryTile myTile)
    {
        if (myTile.rot == 0) attacher = GameObject.Find("FactoryTile" + (myTile.x) + "-" + (myTile.y - 1));
        if (myTile.rot == 180) attacher = GameObject.Find("FactoryTile" + (myTile.x) + "-" + (myTile.y + 1));
        if (myTile.rot == 90) attacher = GameObject.Find("FactoryTile" + (myTile.x + 1) + "-" + (myTile.y));
        if (myTile.rot == 270) attacher = GameObject.Find("FactoryTile" + (myTile.x - 1) + "-" + (myTile.y));

        myRot = (int) myTile.rot;

        if (attacher) attacher.GetComponent<FactoryTile>().attachment = myTile.gameObject;

        gui.GetComponent<DyePanel>().dyeTile = this;
        currentWorkLoad = 100;
        //gui.SetActive(false);
    }

    void Update()
    {
        if (holdingProduct != null)
        {
            if (child != null)
            {
            if (currentWorkLoad > 0)
            {
                currentWait += Time.deltaTime;
                if (currentWait >= totalWait)
                {
                    ChangeColor(holdingProduct);
                 attacher.GetComponent<FactoryTile>().tile.GetComponent<ConveyorTile>().canPass = true;
                    holdingProduct = null;
                    currentWait = 0;
                    currentWorkLoad -= 1;
                }
            }
            else
                {
                    sleepSprite.SetActive(true);
                    if (!attacher.GetComponent<FactoryTile>().next.active)
                    {
                        rechargeTime += Time.deltaTime;
                        if (rechargeTime >= 5)
                        {
                            currentWorkLoad += rechargeLoad;
                            rechargeTime = 0;
                        }
                    }
                    else if (rechargeTime > 0) rechargeTime = 0;
                }

            }
            else if (attacher) attacher.GetComponent<FactoryTile>().tile.GetComponent<ConveyorTile>().canPass = true;
        }
    }

    private void ChangeColor(Product product)
    {
        PlayerStats stats = GameObject.Find("PlayerManager").GetComponent<PlayerStats>();
        if (colorValue && stats.whiteDye >= colorValue.white && stats.blackDye >= colorValue.black && stats.blueDye >= colorValue.blue && stats.redDye >= colorValue.red && stats.yellowDye >= colorValue.yellow)
        {
            stats.whiteDye -= colorValue.white;
            stats.blackDye -= colorValue.black;
            stats.blueDye -= colorValue.blue;
            stats.redDye -= colorValue.red;
            stats.yellowDye -= colorValue.yellow;
        product.color = color;
        product.productColor = realColor;
        }
        transform.parent.GetComponent<FactoryTile>().productHold = false;
        transform.parent.GetComponent<FactoryTile>().active = true;
    }

    public void GetProduct(Product product)
    {
        holdingProduct = product;
        if (child)
        {
            currentWait = 0;
        }
        else
        {
            transform.parent.GetComponent<FactoryTile>().productHold = false;
            transform.parent.GetComponent<FactoryTile>().active = true;
        }
    }

    public void PlaceChild(Child addChild)
    {
        if (child) child.used = false;
        child = addChild;
        totalWait = child.cooldown;
        rechargeLoad = child.recharge;
        child.used = true;

        if (myRot == 0)
        {
            childSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            childSprite.transform.position = transform.position + new Vector3(0, 1.1f, -4);
        }
        if (myRot == 180)
        {
            childSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            childSprite.transform.position = transform.position + new Vector3(0, -0.2f, -4);
        }if (myRot == 90)
        {
            childSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            childSprite.transform.position = transform.position + new Vector3(-0.5f, 0.1f, -4);
        }
        if (myRot == 270)
        {
            childSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            childSprite.transform.position = transform.position + new Vector3(0.5f, 0.1f, -4);
        }

        childSprite.SetActive(true);
    }

    public void DeselectChild(bool delete)
    {
        if (child)
        {
            child.used = false;
            child = null;
        }
        childSprite.SetActive(false);
        sleepSprite.SetActive(false);
        if (!delete) transform.Find("UI").GetComponent<DyePanel>().OpenMainPanel();
    }
}
