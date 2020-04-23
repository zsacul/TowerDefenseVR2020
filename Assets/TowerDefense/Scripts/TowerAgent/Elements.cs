using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType
{
    fire = 0,
    stone = 1,
    ice = 2,
    electricity = 3,
    poison = 4,
    earth = 5,
    wind = 6,
}

public enum Resistance
{
    none,
    low,
    normal,
    high,
}

public class Elements : MonoBehaviour
{
    static Resistance[,] resistanceArray = new Resistance[7, 7]; 
    
    void Start()
    {
        fillResistanceArray();
    }

    void fillResistanceArray()
    {
        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
            {
                if (i != j)
                    resistanceArray[i, j] = Resistance.normal;
                else
                    resistanceArray[i, j] = Resistance.high;
            }

        resistanceArray[(int)ElementType.fire, (int)ElementType.ice] = Resistance.low;
        resistanceArray[(int)ElementType.fire, (int)ElementType.wind] = Resistance.low;

        resistanceArray[(int)ElementType.stone, (int)ElementType.fire] = Resistance.high;
        resistanceArray[(int)ElementType.stone, (int)ElementType.wind] = Resistance.high;
        resistanceArray[(int)ElementType.stone, (int)ElementType.electricity] = Resistance.high;

        resistanceArray[(int)ElementType.ice, (int)ElementType.fire] = Resistance.low;

        resistanceArray[(int)ElementType.earth, (int)ElementType.stone] = Resistance.high;
        resistanceArray[(int)ElementType.earth, (int)ElementType.fire] = Resistance.low;
    }

    public Resistance GetResistence(ElementType e1, ElementType e2)
    {
        return resistanceArray[(int)e1, (int)e2];
    }
}


