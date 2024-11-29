using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTile : MonoBehaviour
{
    public Product myProduct;

    public bool passed;

    public bool canPass;

    private bool passedStarted;
    public bool useAttach;

    private FactoryTile tile;

    public void Placed(FactoryTile myTile)
    {
        myProduct = null;
        tile = myTile;
        useAttach = false;
    }

    void Update()
    {
        if (tile && tile.next.type == "Conveyor")
        {
            if (!tile.next || !tile.next.tile.GetComponent<ConveyorTile>().myProduct)
            {
                if (myProduct && myProduct.transform.position.x == transform.position.x && myProduct.transform.position.y == transform.position.y)
                {
                    if (tile.active && myProduct.dir.x == transform.position.x && myProduct.dir.y == transform.position.y)
                    {
                        if (tile.attachment == null) PassProduct();
                        else if (!useAttach)
                        {
                            tile.AttachmentUse(myProduct);
                            useAttach = true;
                        }
                        else if (canPass) PassProduct();
                    }
                }
            }
            else if (tile.next.tile.GetComponent<ConveyorTile>().myProduct)
            {
                if (myProduct && myProduct.transform.position.x == transform.position.x && myProduct.transform.position.y == transform.position.y)
                {
                    if (tile.active && myProduct.dir.x == transform.position.x && myProduct.dir.y == transform.position.y)
                    {
                        if (tile.next.attachment != null && tile.next.attachment.GetComponent<FactoryTile>().type == "Fabricator")
                        {
                            if (canPass == true) PassProduct();
                        }
                    }
                }
            }
        }
        else if (tile && tile.next.type == "Packager")
        {
            if (myProduct && myProduct.transform.position.x == transform.position.x && myProduct.transform.position.y == transform.position.y)
            {
                myProduct.dir = new Vector2(tile.next.transform.position.x, tile.next.transform.position.y);
                tile.next.tile.GetComponent<PackagerTile>().myProduct = myProduct;
                LoseProduct(myProduct);
            }
        }
    }

    public void PassProduct()
    {
        if (myProduct) myProduct.dir = new Vector2(tile.transform.position.x + tile.change.x, tile.transform.position.y + tile.change.y);
        canPass = false;
        passedStarted = false;
    }

    public void GainProduct(Product newProduct)
    {
        if (myProduct == null) myProduct = newProduct;
        if (tile.previous.type == "Producer" && tile.previous.tile.GetComponent<ProducerTile>().myCotton != null) tile.previous.tile.GetComponent<ProducerTile>().myCotton = null;
        if (tile.previous.type == "Conveyor")
        {
            tile.previous.tile.GetComponent<ConveyorTile>().LoseProduct(myProduct);
        }
        myProduct.dir = transform.position;
        canPass = false;
        passed = false;
        useAttach = false;
    }

    public void LoseProduct(Product goneProduct)
    {
        if (goneProduct == myProduct) myProduct = null;
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Product"))
        {
        GainProduct(col.GetComponent<Product>());
        }
    }
}
