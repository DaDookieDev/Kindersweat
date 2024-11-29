using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerGrid : MonoBehaviour
{
    public GameObject selected;

    public GameObject toPlace;

    private bool willDestroy;

    public GameObject qText;

    public GameObject shopPanel;

    private string type;
    private int rotating;

    public bool placing;
    public bool destroying;

    public GameObject conveyorTile, cottonProducerTile, cottonPackagerTile, cottonDyerTile, cottonFabricatorTile;

    void Update()
    {
        if (placing)
        {
            if (selected) toPlace.transform.position = selected.transform.position + new Vector3(0, 0, -2);
            if (Input.GetKeyDown(KeyCode.R)) rotating += 90;
            if (selected.GetComponent<FactoryTile>().tile == null) toPlace.transform.Find("Tile").GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            else toPlace.transform.Find("Tile").GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;

            if (rotating > 270) rotating = 0;
            toPlace.transform.eulerAngles = new Vector3(0, 0, rotating);
        }
        Vector3 mousePos = Input.mousePosition;
        bool canSelect = false;
        if (mousePos.x > 300 && mousePos.x < 1800 && mousePos.y < 950) canSelect = true;

        if (Input.GetMouseButtonDown(0) && canSelect)
            {
                if (selected && selected.GetComponent<FactoryTile>().tile == null)
                {
                    if (placing)
                    {
                        if (GetComponent<PlayerStats>().money >= 10)
                        {
                            GetComponent<PlayerStats>().money -= 10;
                            toPlace.transform.parent = selected.transform;
                        toPlace.name = type;
                        selected.GetComponent<FactoryTile>().type = type;
                        selected.GetComponent<FactoryTile>().tile = toPlace;
                        selected.GetComponent<FactoryTile>().Spawn();
                        Deselect(false);
                        StartPlace(type);
                        }
                        else GetComponent<PlayerStats>().brokeSound.Play();
                    }
                }
                else if (selected && destroying)
                    {
                        GetComponent<PlayerStats>().badSound.Play();
                        GetComponent<PlayerStats>().money += 8;
                        selected.GetComponent<FactoryTile>().Reset();
                    }
            }
                if (Input.GetKey(KeyCode.Q))
                    {
                        Deselect(true);
                        DisableDestroy();
                    }
    }

    public void StartPlace(string name)
    {
        destroying = false;
        Deselect(true);
        if (name == "Conveyor") toPlace = Instantiate(conveyorTile, selected.transform.position + new Vector3(0, 0, -1), Quaternion.identity).gameObject;
        if (name == "Producer") toPlace = Instantiate(cottonProducerTile, selected.transform.position + new Vector3(0, 0, -1), Quaternion.identity).gameObject;
        if (name == "Packager") toPlace = Instantiate(cottonPackagerTile, selected.transform.position + new Vector3(0, 0, -1), Quaternion.identity).gameObject;
        if (name == "Dyer") toPlace = Instantiate(cottonDyerTile, selected.transform.position + new Vector3(0, 0, -1), Quaternion.identity).gameObject;
        if (name == "Fabricator") toPlace = Instantiate(cottonFabricatorTile, selected.transform.position + new Vector3(0, 0, -1), Quaternion.identity).gameObject;

        GetComponent<PlayerStats>().openedGUI = null;
        type = name;
        placing = true;
        shopPanel.GetComponent<ShopPanel>().Close();
        qText.GetComponent<TMP_Text>().text = "PLACING: PRESS Q TO STOP";
        qText.SetActive(true);
    }

    public void Deselect(bool destroy)
    {
        placing = false;
        qText.SetActive(false);
        if (toPlace && destroy) Destroy(toPlace);
        toPlace = null;
    }

    public void EnableDestroy()
    {
        Deselect(true);
        destroying = true;
        GameObject.Find("PlayerManager").GetComponent<PlayerStats>().selectSound.Play();
        qText.GetComponent<TMP_Text>().text = "DESTROYING: PRESS Q TO STOP";
        qText.SetActive(true);
    }

    public void DisableDestroy()
    {
        Deselect(true);
        destroying = false;
    }
}
