using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Order
{
    public Soup mainSoup;
    public Colour colourPreference;
    public bool isSpicy;
    public bool isChunky;
    public bool noMeat;
    public bool noVeg;
    public Order(Soup mainSoup, Colour colourPreference, bool isSpicy, bool isChunky, bool meatPreference, bool vegPreference)
    {
        this.mainSoup = mainSoup;
        this.colourPreference = colourPreference;
        this.isSpicy = isSpicy;
        this.isChunky = isChunky;
    }
    public Order()
    { 
    }

    static Soup GetSoupFromDropdown(int selected, TMP_Dropdown soupDropdown)
    {
        for (int i = 0; i < CookingManager.allSoups.Count; i++)
        {
            if (soupDropdown.options[selected].text == CookingManager.allSoups[i].soupName)
            {
                return CookingManager.allSoups[i];
            }
        }
        return null;
    }

    public static Order CreateOrder(TMP_Dropdown soup, TMP_Dropdown colourPreference, TMP_Dropdown meatVegPref, Toggle spicy, Toggle chunky)
    {
        Order newOrder = new Order();

        newOrder.mainSoup = GetSoupFromDropdown(soup.value, soup);
        newOrder.colourPreference = new Colour("none");

        if (spicy.isOn)
        {
            newOrder.isSpicy = true;
        }
        else
        {
            newOrder.isSpicy = false;
        }

        if (chunky.isOn)
        {
            newOrder.isChunky = true;
        }
        else
        {
            newOrder.isChunky = false;
        }

        if (meatVegPref.value == 0)
        {
            newOrder.noMeat = false;
            newOrder.noVeg = false;
        }
        else if (meatVegPref.value == 1)
        {
            newOrder.noMeat = true;
            newOrder.noVeg = false;
        }
        else if (meatVegPref.value == 2)
        {
            newOrder.noVeg = true;
            newOrder.noMeat = false;
        }

        return newOrder;
    }
}
