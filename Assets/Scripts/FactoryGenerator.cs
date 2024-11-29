using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryGenerator : MonoBehaviour
{
    private Object factoryTile;

    public Vector3 startPos;
    public int xSize;
    public int ySize;

    void Start()
    {
        factoryTile = Resources.Load("Factory/FactoryTile");

        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {
                GameObject newTile = (GameObject) Instantiate(factoryTile, startPos + new Vector3(i * 2, j * 2, 2), Quaternion.identity);
                newTile.name = "FactoryTile" + (i + 1) + "-" + (j + 1);
                newTile.GetComponent<FactoryTile>().x = (i + 1);
                newTile.GetComponent<FactoryTile>().y = (j + 1);
                newTile.transform.parent = transform;
            }
        }
    }
}
