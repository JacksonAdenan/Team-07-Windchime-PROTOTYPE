using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soup
{
    public string soupName;
    public Ingridient core1;
    public Ingridient core2;
    public Ingridient core3;
    public bool isSpicy;
    public bool isChunky;
    public Colour colour;
    public Soup(string name, Ingridient core1, Ingridient core2, Ingridient core3)
    {
        soupName = name;
        this.core1 = core1;
        this.core2 = core2;
        this.core3 = core3;
    }
}
