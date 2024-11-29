using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProducerTile : MonoBehaviour
{
    public GameObject cotton;
    public FactoryTile attachTile;

    public GameObject myCotton;

    public void Placed(FactoryTile myTile)
    {
        attachTile = myTile;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == myCotton)
        {
            myCotton = null;
        }
    }

    void Update()
    {
        if (attachTile && attachTile.next.tile)
        {
        if (attachTile.next.tile.GetComponent<ConveyorTile>().myProduct == null)
        {
            if (myCotton)
        {
                if (myCotton && myCotton.transform.position.x == transform.position.x && myCotton.transform.position.y == transform.position.y) myCotton.GetComponent<Product>().dir = new Vector2(attachTile.transform.position.x + attachTile.change.x, attachTile.transform.position.y + attachTile.change.y);
        }
            else
            {
                if (attachTile.active) CreateCotton();
            }
        }
        }
    }

    private void CreateCotton()
    {
        if (GameObject.Find("PlayerManager").GetComponent<PlayerStats>().cotton >= 1)
        {
            myCotton = Instantiate(cotton, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity).gameObject;
            myCotton.transform.parent = GameObject.Find("Products").transform;
            GameObject.Find("PlayerManager").GetComponent<PlayerStats>().cotton -= 1;
        }
    }
}
