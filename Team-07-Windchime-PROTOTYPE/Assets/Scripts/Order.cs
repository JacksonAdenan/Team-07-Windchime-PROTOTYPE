using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
}
