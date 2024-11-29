using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float maxX;
    public float maxY;
    public float minX;
    public float minY;

    private int x;
    private int y;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && transform.position.y < maxY)
        {
            y = 1;
        }
        else if (Input.GetKey(KeyCode.S) && transform.position.y > minY)
        {
            y = -1;
        }
        else
        {
            y = 0;
        }

        if (Input.GetKey(KeyCode.D) && transform.position.x < maxX)
        {
            x = 1;
        }
        else if (Input.GetKey(KeyCode.A) && transform.position.x > minX)
        {
            x = -1;
        }
        else
        {
            x = 0;
        }

        transform.position += new Vector3(x, y, 0) * Time.deltaTime * 5;
    }
}
