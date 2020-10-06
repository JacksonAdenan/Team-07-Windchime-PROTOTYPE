using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static List<Order> currentOrders;

    void Start()
    {
        currentOrders = new List<Order>();
    }

    public static void AddOrder(Order orderToAdd)
    {
        currentOrders.Clear();
        currentOrders.Add(orderToAdd);
    }
}
