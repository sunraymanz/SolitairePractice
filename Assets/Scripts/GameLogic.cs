using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameLogic : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFaces;
    public GameObject[] topPos;
    public GameObject[] bottomPos;
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
                if (card == bottoms[i][bottoms[i].Count-1])
                {
                    newCard.GetComponent<SelectableSprite>().isFaceUp = true;
                }
                else { newCard.GetComponent<SelectableSprite>().isFaceUp = false; }
                yOffset += 0.3f;
                zOffset += 0.03f;
            }
        }
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
}
