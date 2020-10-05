using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public Soup mainSoup;
    public Colour colourPreference;
    public bool isSpicy;
    public bool isChunky;
    public Order(Soup mainSoup, Colour colourPreference, bool isSpicy, bool isChunky)
    {
        this.mainSoup = mainSoup;
        this.colourPreference = colourPreference;
        this.isSpicy = isSpicy;
        this.isChunky = isChunky;
    }
    
}
