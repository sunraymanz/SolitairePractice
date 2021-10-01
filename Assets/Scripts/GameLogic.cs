using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameLogic : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFaces;
    public GameObject[] topPos;
    public GameObject[] bottomPos;
    public GameObject deckPos;
    public List<string>[] tops ;
    public List<string>[] bottoms ;

    public List<string> bottom0 = new List<string>() ;
    public List<string> bottom1 = new List<string>() ;
    public List<string> bottom2 = new List<string>() ;
    public List<string> bottom3 = new List<string>() ;
    public List<string> bottom4 = new List<string>() ;
    public List<string> bottom5 = new List<string>() ;
    public List<string> bottom6 = new List<string>() ;

    public static string[] suits = new string[]{"C","D","H","S" };
    public static string[] numbers = new string[]{"A","2","3","4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string> deck ;
    public List<string> tripsOnDisplay = new List<string>() ;
    public List<List<string>> deckTrips = new List<List<string>>();
    public List<string> discardPile = new List<string>();
    int trips;
    int tripsRemainder;
    int deckLocation;

    // Start is called before the first frame update
    void Start()
    {
        bottoms = new List<string>[] {bottom0,bottom1, bottom2, bottom3, bottom4, bottom5, bottom6 };
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
        SortCard();
        StartCoroutine(SpawnCard());
        SortDeckIntoTrips();
    }

    public static List<string> GenerateDeck()
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

    IEnumerator SpawnCard()
    {
        for (int i = 0; i < 7; i++)
        {
            float yOffset = 0f;
            float zOffset = 0.03f;
            foreach (string card in bottoms[i])
            {
                yield return new WaitForSeconds(0.03f);
                GameObject newCard = Instantiate(cardPrefab, new Vector3(bottomPos[i].transform.position.x, bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset), Quaternion.identity, bottomPos[i].transform);
                newCard.name = card;
                newCard.GetComponent<SelectableSprite>().row = i;
                if (card == bottoms[i][bottoms[i].Count-1])
                {
                    newCard.GetComponent<SelectableSprite>().isFaceUp = true;
                }
                yOffset += 0.3f;
                zOffset += 0.03f;
                discardPile.Add(card);
            }
        }
        foreach (string card in discardPile)
        {
            if (deck.Contains(card))
            {
                deck.Remove(card);
            }
        }
        discardPile.Clear();
    }

    void SortCard()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = i; j < 7; j++)
            {
                bottoms[j].Add(deck[deck.Count-1]);
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }

    void SortDeckIntoTrips()
    {
        trips = deck.Count / 3;
        tripsRemainder = deck.Count % 3;
        deckTrips.Clear();
        int modifiers = 0;
        for (int i = 0; i < trips; i++)
        {
            List<string> myTrips = new List<string>();
            for (int j = 0; j < 3; j++)
            {
                myTrips.Add(deck[j+modifiers]);
            }
            deckTrips.Add(myTrips);
            modifiers += 3;
        }
        if (tripsRemainder != 0)
        {
            List<string> myRemainder = new List<string>();
            for (int k = 0; k < tripsRemainder; k++)
            {
                myRemainder.Add(deck[deck.Count-tripsRemainder+modifiers]);
                modifiers++;
            }
            deckTrips.Add(myRemainder);
            trips++;
        }
        deckLocation = 0;
    }

    public void DealFromDeck()
    {
        foreach (Transform child in deckPos.transform)
        {
            if (child.CompareTag("Card"))
            {
                deck.Remove(child.name);
                discardPile.Add(child.name);
                Destroy(child.gameObject);
            }
        }
        if (deckLocation < trips)
        {
            tripsOnDisplay.Clear();
            float xOffset = 2.5f;
            float zOffset = -0.2f;
            foreach (string card in deckTrips[deckLocation])
            {
                GameObject newCard = Instantiate(cardPrefab, new Vector3(deckPos.transform.position.x+xOffset, deckPos.transform.position.y, deckPos.transform.position.z + zOffset), Quaternion.identity,deckPos.transform);
                newCard.name = card;
                xOffset += 0.5f;
                zOffset -= 0.2f;
                tripsOnDisplay.Add(card);
                newCard.GetComponent<SelectableSprite>().isFaceUp = true;
                newCard.GetComponent<SelectableSprite>().inDeckPile = true;
            }
            deckLocation++;
        }
        else
        {
            RestackTopDeck();
        }
    }

    void RestackTopDeck() 
    {
        foreach (string card in discardPile)
        {
            deck.Add(card);
        }
        discardPile.Clear();
        SortDeckIntoTrips();
    }
}
