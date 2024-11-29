using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FabricatorTile : MonoBehaviour
{
    public GameObject attacher;

    public int fabric;

    public GameObject gui;

    public Child child;

    public GameObject childSprite;
    public GameObject sleepSprite;

    public Sprite[] cottons;

    public Product holdingProduct;

    public GameObject showProduct;

    public GameObject baseProduct;

    private int prodShape, prodColor;

    public int holdingCotton;
    public int neededCotton;

    public Sprite realShape;

    public bool isActive;

    private float currentWait;
    public float totalWait;

    public int currentWorkLoad;
    private int rechargeLoad;

    private float rechargeTime;

    private int myRot;

    public void Placed(FactoryTile myTile)
    {
        if (myTile.rot == 0) attacher = GameObject.Find("FactoryTile" + (myTile.x) + "-" + (myTile.y - 1));
        if (myTile.rot == 180) attacher = GameObject.Find("FactoryTile" + (myTile.x) + "-" + (myTile.y + 1));
        if (myTile.rot == 90) attacher = GameObject.Find("FactoryTile" + (myTile.x + 1) + "-" + (myTile.y));
        if (myTile.rot == 270) attacher = GameObject.Find("FactoryTile" + (myTile.x - 1) + "-" + (myTile.y));

        myRot = (int) myTile.rot;

        if (attacher) attacher.GetComponent<FactoryTile>().attachment = myTile.gameObject;

        gui.GetComponent<FabricatorPanel>().fabTile = this;
        currentWorkLoad = 100;
    }

    void Update()
    {
        if (holdingCotton == neededCotton)
        {
            if (child != null)
            {
            if (currentWorkLoad > 0)
            {
                if (holdingProduct != null)
                {
                    sleepSprite.SetActive(false);
                currentWait += Time.deltaTime;
                if (currentWait >= totalWait)
                {
                    showProduct.SetActive(false);
                    holdingProduct.gameObject.SetActive(true);
                    ChangeFabric(holdingProduct);
                    attacher.GetComponent<FactoryTile>().tile.GetComponent<ConveyorTile>().canPass = true;
                    holdingProduct = null;
                    currentWait = 0;
                    currentWorkLoad -= 1;
                }
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
            else if (attacher && attacher.GetComponent<FactoryTile>().tile) attacher.GetComponent<FactoryTile>().tile.GetComponent<ConveyorTile>().canPass = true;
        }
        else
        {
            if (currentWorkLoad == 0)
            {
                sleepSprite.SetActive(true);
                    if (attacher && !attacher.GetComponent<FactoryTile>().next.active)
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
            else
            {
                sleepSprite.SetActive(false);
            }
        }
    }

    public void ChangeFabric(Product product)
    {
        product.shape = fabric;
        product.productSprite = realShape;
        transform.parent.GetComponent<FactoryTile>().productHold = false;
        transform.parent.GetComponent<FactoryTile>().active = true;
        
        //product.dir = attacher.GetComponent<Conveyor>().transform.position
        holdingCotton = 0;
    }

    public void GetProduct(Product product)
    {
        if (child && neededCotton > 0)
        {
            if (product.shape == 0)
            {
            if (holdingCotton < neededCotton) holdingCotton += 1;
            else if (holdingCotton == neededCotton)
            {
                holdingProduct = product;
            }
            if (holdingCotton == neededCotton)
            {
                currentWait = 0;
                //Destroy(product.gameObject);
                product.gameObject.SetActive(false);
                holdingProduct = baseProduct.AddComponent<Product>();
                holdingProduct.color = prodColor;
                holdingProduct.shape = prodShape;
                holdingProduct.productColor = baseProduct.GetComponent<SpriteRenderer>().color;
                holdingProduct.productSprite = baseProduct.GetComponent<SpriteRenderer>().sprite;
                attacher.GetComponent<FactoryTile>().tile.GetComponent<ConveyorTile>().myProduct = holdingProduct;
                //attacher.GetComponent<FactoryTile>().previous.tile.GetComponent<ConveyorTile>().canPass = false;
                showProduct.GetComponent<SpriteRenderer>().sprite = cottons[holdingCotton - 1];
            }
            else if (holdingCotton == 1)
            {
                baseProduct = product.gameObject;
                prodColor = product.color;
                prodShape = product.shape;
                baseProduct.SetActive(false);
                showProduct.GetComponent<SpriteRenderer>().color = baseProduct.GetComponent<SpriteRenderer>().color;
                showProduct.SetActive(true);
                showProduct.GetComponent<SpriteRenderer>().sprite = cottons[0];
                Destroy(product);
            }
            else if (holdingCotton > 1 && holdingCotton < neededCotton)
            {
                transform.parent.GetComponent<FactoryTile>().active = true;
                transform.parent.GetComponent<FactoryTile>().productHold = false;
                attacher.GetComponent<FactoryTile>().tile.GetComponent<ConveyorTile>().canPass = true;
                showProduct.GetComponent<SpriteRenderer>().sprite = cottons[holdingCotton - 1];
                Destroy(product.gameObject);
            }
            }
            else
            {
                attacher.GetComponent<FactoryTile>().tile.GetComponent<ConveyorTile>().canPass = true;
            }
        }
        else
        {
            holdingCotton = 0;
            transform.parent.GetComponent<FactoryTile>().productHold = false;
            transform.parent.GetComponent<FactoryTile>().active = true;
            attacher.GetComponent<FactoryTile>().tile.GetComponent<ConveyorTile>().canPass = true;
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
        if (!delete) transform.Find("UI").GetComponent<FabricatorPanel>().OpenMainPanel();
    }
}
