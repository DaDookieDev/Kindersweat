using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenArrows : MonoBehaviour
{
    public GameObject[] myArrows;

    public void CloseArrows()
    {
        for (int i = 0; i < myArrows.Length; i++)
        {
            myArrows[i].SetActive(false);
        }
    }

    public void OpenArrows()
    {
        for (int i = 0; i < myArrows.Length; i++)
        {
            myArrows[i].SetActive(true);
        }
    }
}
