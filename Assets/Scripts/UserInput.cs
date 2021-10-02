using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UserInput : MonoBehaviour
{
    public GameObject slot1;
    GameLogic logicToken;
    float timer;
    float doubleClickTime = 0.3f;
    int clickCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        logicToken = FindObjectOfType<GameLogic>();
        Initate();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseClick();
        CheckDoubleClick();
    }

    public void Initate()
    {
        slot1 = this.gameObject; ;
    }
    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount ++;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit) //Check if hit something
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    //Hit Deck
                    DeckPressed();
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    //Hit Card
                    CardPressed(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Top"))
                {
                    //Hit Top
                    TopPressed(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Bottom"))
                {
                    //Hit Bottom
                    BottomPressed(hit.collider.gameObject);
                }
                else { print("Nothing Here"); }
            }
        }
    }

    void DeckPressed() 
    {
        print("Deck Pressed");
        logicToken.DealFromDeck();
        slot1 = this.gameObject;
    }
    void CardPressed(GameObject selected)
    {
        print("Card Pressed");
        //Flip or not?
        if (!selected.GetComponent<SelectableSprite>().isFaceUp)
        {
            if (!IsBlocked(selected))
            {
                //Flip
                selected.GetComponent<SelectableSprite>().isFaceUp = true;
                slot1 = this.gameObject;
            }
        }
        //In the deck pile or not?
        else if (selected.GetComponent<SelectableSprite>().inDeckPile)
        {
            if (!IsBlocked(selected))
            {
                if (slot1 == selected)
                {
                    if (CheckDoubleClick())
                    {
                        AutoStack(selected);
                    }
                }
                else
                {
                    slot1 = selected;
                }
            }
        }
        else
        {
            //If the card is face up + no card currently selected
            if (slot1 == this.gameObject)
            {
                slot1 = selected;
            }
            //If there is already card selected (and it is not the same card)
            else if (slot1 != selected)
            {
                if (CheckStackable(selected))
                {
                    Stack(selected);
                }
                else
                {
                    slot1 = selected;
                }
            }
            else if(slot1 == selected)
            {
                if (CheckDoubleClick())
                {
                    AutoStack(selected);
                }
            }
        }
    }
    void TopPressed(GameObject selected)
    {
        print("Top Pressed");

        if (slot1.CompareTag("Card"))
        {
            //Check if it's ACE
            if (slot1.GetComponent<SelectableSprite>().value == 1)
            {
                Stack(selected);
            }
        }
    }
    void BottomPressed(GameObject selected)
    {
        print("Bottom Pressed");

        if (slot1.CompareTag("Card"))
        {
            //Check if it's K
            if (slot1.GetComponent<SelectableSprite>().value == 13)
            {
                Stack(selected);
            }
        }


    }

    bool CheckStackable(GameObject selected)
    {
        SelectableSprite s1 = slot1.GetComponent<SelectableSprite>();
        SelectableSprite s2 = selected.GetComponent<SelectableSprite>();

        if (!s2.inDeckPile)
        {
            //Top pile : Stack suit Ace to King
            if (s2.top)
            {
                //Same suit OR Ace to Empty slot
                if (s1.suit == s2.suit || (s1.value == 1 && s2.suit == null))
                {
                    if (s1.value == s2.value + 1)
                    {
                        print("Stackable : Top Rules");
                        return true;
                    }
                }
                else 
                {
                    print("Not Stackable : Not Same suit OR Not Ace to Empty slot");
                    return false;
                }
            }
            //Bottom pile : Stack King to Ace + Not same suit
            else
            {
                //Suit check
                if (s1.value == s2.value - 1)
                {
                    bool card1Red = true;
                    bool card2Red = true;
                    if (s1.suit == "C" || s1.suit == "S")
                    {
                        card1Red = false;
                    }
                    if (s2.suit == "C" || s2.suit == "S")
                    {
                        card2Red = false;
                    }
                    if (card1Red == card2Red)
                    {
                        print("Not Stackable : Same Suit");
                        return false;
                    }
                    else
                    {
                        print("Stackable : Bottom Ruls");
                        return true;
                    }
                }
                else
                {
                    print("Not Stackable : Value Incorrect");
                    return false;
                }
            }
        }
        print("Not Stackable");
        return false;
    }

    void Stack(GameObject selected) 
    {
        SelectableSprite s1 = slot1.GetComponent<SelectableSprite>();
        SelectableSprite s2 = selected.GetComponent<SelectableSprite>();
        float yOffset = 0.3f;
        //If on top or empty bottom stack the cards in place
        if (s2.top || (!s2.top && s1.value == 13))
        {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(selected.transform.position.x, selected.transform.position.y - yOffset, selected.transform.position.z-0.01f);
        slot1.transform.parent = selected.transform;

        if (s1.inDeckPile) //Removes the cards from the top pile to prevent duplicate cards
        {
            logicToken.tripsOnDisplay.Remove(slot1.name);
            //logicToken.deck.Remove(slot1.name);
        }
        else if (s1.top && s2.top && s1.value == 1) //Allow to Switch Top slots
        {
            logicToken.topPos[s1.row].GetComponent<SelectableSprite>().value = 0;
            logicToken.topPos[s1.row].GetComponent<SelectableSprite>().suit = null;
        }
        else if (s1.top)//Keeps track of the current value of the top decks as a card has been removed
        {
            logicToken.topPos[s1.row].GetComponent<SelectableSprite>().value = s1.value-1;
        }
        else //Removes the card string from the appropriate bottom list
        {
            logicToken.bottoms[s1.row].Remove(slot1.name);
        }
        s1.inDeckPile = false;
        s1.row = s2.row;

        if (s2.top)//Moves card to top and assigns the top's value and suit
        {
            logicToken.topPos[s1.row].GetComponent<SelectableSprite>().value = s1.value;
            logicToken.topPos[s1.row].GetComponent<SelectableSprite>().suit = s1.suit;
            s1.top = true;
        }
        else 
        {
            s1.top = false;
        }
        //Reset slot1 
        slot1 = this.gameObject;
    }

    bool IsBlocked(GameObject selected) 
    {
        SelectableSprite s2 = selected.GetComponent<SelectableSprite>();
        if (s2.inDeckPile == true)
        {
            //if (s2.name == logicToken.tripsOnDisplay[logicToken.tripsOnDisplay.Count-1])
            if (s2.name == logicToken.tripsOnDisplay.Last())
            {
                return false;
            }
            else
            {
                //print(s2.name + " is Blocked by " + logicToken.tripsOnDisplay[logicToken.tripsOnDisplay.Count - 1]);
                print(s2.name + " is Blocked by " + logicToken.tripsOnDisplay.Last());
                return true;
            }
        }
        else
        {
            //if (s2.name == logicToken.bottoms[s2.row][logicToken.bottoms[s2.row].Count-1])
            if (s2.name == logicToken.bottoms[s2.row].Last())
            {
                return false;
            }
            else
            {
                //print(s2.name + " is Blocked by " + logicToken.bottoms[s2.row][logicToken.bottoms[s2.row].Count - 1]);
                print(s2.name + " is Blocked by " + logicToken.bottoms[s2.row].Last());
                return true;
            }
        } 
    }

    bool CheckDoubleClick() 
    {
        if (clickCount == 1)
        {
            timer += Time.deltaTime;
        }
        if (clickCount == 2 && timer < doubleClickTime)
        {
            print("Done DoubleClick");
            timer = 0;
            clickCount = 0;
            return true;
        }
        else if (timer > doubleClickTime)
        {
            print("Time Out");
            timer = 0;
            clickCount = 0;
        }
        return false;
    }

    void AutoStack(GameObject selected)
    {
        for (int i = 0; i < logicToken.topPos.Length ; i++)
        {
            SelectableSprite stack = logicToken.topPos[i].GetComponent<SelectableSprite>();
            if (selected.GetComponent<SelectableSprite>().value == 1)//If it is an Ace
            {
                if (logicToken.topPos[i].GetComponent<SelectableSprite>().value == 0)//If top Empty
                {
                    slot1 = selected;
                    Stack(stack.gameObject);
                    break;
                }
            }
            else
            {
                if ((logicToken.topPos[i].GetComponent<SelectableSprite>().suit == slot1.GetComponent<SelectableSprite>().suit) &&(logicToken.topPos[i].GetComponent<SelectableSprite>().value == slot1.GetComponent<SelectableSprite>().value-1))
                {
                    if (HasNoChildren(slot1))
                    {
                        slot1 = selected;
                        //Find top slot that matches the conditions for auto stacking
                        string lastCardName = stack.suit + stack.value.ToString();
                        if (stack.value == 1)
                        {
                            lastCardName = stack.suit + "A";
                        }
                        if (stack.value == 11)
                        {
                            lastCardName = stack.suit + "J";
                        }
                        if (stack.value == 12)
                        {
                            lastCardName = stack.suit + "Q";
                        }
                        if (stack.value == 13)
                        {
                            lastCardName = stack.suit + "K";
                        }
                        GameObject lastCard = GameObject.Find(lastCardName);
                        Stack(lastCard);
                        break;
                    }
                }
            }
        }
    }

    bool HasNoChildren(GameObject card)
    {
        int i = 0;
        foreach (Transform child in card.transform)
        {
            i++;
        }
        if (i == 0)
        {
            return true;
        }
        else 
        { 
            return false; 
        }
    }
}
