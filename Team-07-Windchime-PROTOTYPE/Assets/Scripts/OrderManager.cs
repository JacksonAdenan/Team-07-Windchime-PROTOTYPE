using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static List<Order> currentOrder;

    void Start()
    {
        currentOrder = new List<Order>();
    }
}
