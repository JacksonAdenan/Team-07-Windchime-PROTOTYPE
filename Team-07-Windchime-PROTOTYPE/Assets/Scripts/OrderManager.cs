using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private List<Soup> allSoups = new List<Soup>();

    void ReadAllSoups(GameObject soupContainerObject)
    {
        for (int i = 0; i < soupContainerObject.transform.childCount; i++)
        { 
            //allSoups.Add(soupContainerObject.transform.GetChild()
        }
    }
}
