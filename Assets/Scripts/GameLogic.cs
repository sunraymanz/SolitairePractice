using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public string[] suits = new string[]{"C","D","H","S" };
    public string[] numbers = new string[]{"A","2","3","4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public Sprite[] cardFaces;
    public List<string> deck ;
    // Start is called before the first frame update
    void Start()
    {
        InitCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitCard()
    {
        deck = GenerateDeck();
        Shuffle(deck);
        foreach (string card in deck)
        {
            print(card);
        }
    }

    public List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string suit in suits)
        {
            foreach (string number in numbers)
            {
                newDeck.Add(suit+number);              
            }
        }
        return newDeck;
    }

    

    void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = rng.Next(n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
