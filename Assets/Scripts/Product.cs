using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public int color;
    public Color productColor;
    public int shape;
    public Sprite productSprite;
    public Vector2 dir;
    //public FactoryTile currentTile;
    //public bool passed;

    private float idleCount;

    //public int value;

    void Update()
    {
        Vector2 productMove = Vector2.MoveTowards(transform.position, dir, Time.deltaTime);
        transform.position = new Vector3(productMove.x, productMove.y, -3);
        GetComponent<SpriteRenderer>().color = productColor;
        GetComponent<SpriteRenderer>().sprite = productSprite;

        if (transform.position.x == dir.x && transform.position.y == dir.y)
        {
            idleCount += Time.deltaTime;
            if (idleCount > 11) Destroy(gameObject);
        }
        else idleCount = 0;
    }
}
