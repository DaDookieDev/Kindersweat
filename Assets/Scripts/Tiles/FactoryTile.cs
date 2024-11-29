using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryTile : MonoBehaviour
{
    public Vector2 change;
    public GameObject attachment;
    public GameObject tile;

    public bool productHold;

    public FactoryTile previous;
    public FactoryTile next;

    public int x, y;

    public bool active;

    public float rot;
    public string type;

    public void Spawn()
    {
        rot = tile.transform.eulerAngles.z;

        if (type == "Conveyor") tile.GetComponent<ConveyorTile>().Placed(this);
        if (type == "Producer") tile.GetComponent<ProducerTile>().Placed(this);
        if (type == "Dyer") tile.GetComponent<DyerTile>().Placed(this);
        if (type == "Fabricator") tile.GetComponent<FabricatorTile>().Placed(this);
        if (type == "Packager") tile.GetComponent<PackagerTile>().Placed(this);


        tile.transform.Find("Arrows").gameObject.SetActive(false);

        GameObject previousObj = null;
        GameObject nextObj = null;
        if (rot == 0)
        {
            change = new Vector2(0, 1);
            previousObj = GameObject.Find("FactoryTile" + (x) + "-" + (y - 1));
            nextObj = GameObject.Find("FactoryTile" + (x) + "-" + (y + 1));
        }
        if (rot == 180)
        {
            change = new Vector2(0, -1);
            previousObj = GameObject.Find("FactoryTile" + (x) + "-" + (y + 1));
            nextObj = GameObject.Find("FactoryTile" + (x) + "-" + (y - 1));
        }
        if (rot == 90)
        {
            change = new Vector2(-1, 0);
            previousObj = GameObject.Find("FactoryTile" + (x + 1) + "-" + (y));
            nextObj = GameObject.Find("FactoryTile" + (x - 1) + "-" + (y));
        }
        if (rot == 270)
        {
            change = new Vector2(1, 0);
            previousObj = GameObject.Find("FactoryTile" + (x - 1) + "-" + (y));
            nextObj = GameObject.Find("FactoryTile" + (x + 1) + "-" + (y));
        }

        if (type == "Packager")
        {
            change = new Vector2(0, 0);
            nextObj = null;
        }

        if (previousObj) previous = previousObj.GetComponent<FactoryTile>();
        if (nextObj) next = nextObj.GetComponent<FactoryTile>();
    }

    void Update()
    {
        CheckActive();
    }

    private void CheckActive()
    {
        //if (attachment && productHold) active = false;
        if (next && next.active) active = true;
        else if (type == "Packager" && tile.GetComponent<PackagerTile>().accepting == true) active = true;
        else active = false;
    }

    public void AttachmentUse(Product product)
    {
        if (attachment && attachment.GetComponent<FactoryTile>().tile.GetComponent<DyerTile>()) attachment.GetComponent<FactoryTile>().tile.GetComponent<DyerTile>().GetProduct(product);
        if (attachment && attachment.GetComponent<FactoryTile>().tile.GetComponent<FabricatorTile>()) attachment.GetComponent<FactoryTile>().tile.GetComponent<FabricatorTile>().GetProduct(product);
    }

    void OnMouseEnter()
    {
        GameObject.Find("PlayerManager").GetComponent<PlayerGrid>().selected = gameObject;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && GameObject.Find("PlayerManager").GetComponent<PlayerGrid>().destroying == false && GameObject.Find("PlayerManager").GetComponent<PlayerGrid>().placing == false && GameObject.Find("PlayerManager").GetComponent<PlayerStats>().openedGUI == null)
        {
            if (type == "Dyer") tile.transform.Find("UI").GetComponent<DyePanel>().OpenMainPanel();
            if (type == "Fabricator") tile.transform.Find("UI").GetComponent<FabricatorPanel>().OpenMainPanel();
            if (type == "Packager") tile.transform.Find("UI").GetComponent<PackagerPanel>().OpenPanel();
        }
    }

    public void Reset()
    {
        if (tile != null)
        {
            if (type == "Dyer") tile.GetComponent<DyerTile>().DeselectChild(true);
            else if (type == "Fabricator") tile.GetComponent<FabricatorTile>().DeselectChild(true);
            Destroy(tile);
            tile = null;
        }
        type = null;
        rot = 0;
        active = false;
        if (previous && previous.attachment && previous.attachment == gameObject) previous.attachment = null;
        previous = null;
        next = null;
        productHold = false;
    }
}
