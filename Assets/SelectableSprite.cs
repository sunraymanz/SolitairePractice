using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableSprite : MonoBehaviour
{
    public bool isFaceUp = false;
    public bool top;
    public bool inDeckPile = false;
    public string suit;
    public int value;
    public int row;

    string valueString;
    // Start is called before the first frame update
    void Start()
    {
        if (CompareTag("Card"))
        {
            suit = transform.name[0].ToString();
            for (int i = 1; i < transform.name.Length; i++)
            {
                char c = transform.name[i];
                valueString = valueString + c.ToString();
            }
        }
        SetValueFrom(valueString);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetValueFrom(string str)
    {
        if (str == "A")
        {
            value = 1;
        }
        else if (str == "2")
        {
            value = 2;
        }
        else if (str == "3")
        {
            value = 3;
        }
        else if (str == "4")
        {
            value = 4;
        }
        else if (str == "5")
        {
            value = 5;
        }
        else if (str == "6")
        {
            value = 6;
        }
        else if (str == "7")
        {
            value = 7;
        }
        else if (str == "8")
        {
            value = 8;
        }
        else if (str == "9")
        {
            value = 9;
        }
        else if (str == "10")
        {
            value = 10;
        }
        else if (str == "J")
        {
            value = 11;
        }
        else if (str == "Q")
        {
            value = 12;
        }
        else if (str == "K")
        {
            value = 13;
        }

    }
}
