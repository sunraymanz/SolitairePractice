using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    GameLogic logicToken;
    public GameObject slot1;
    // Start is called before the first frame update
    void Start()
    {
        logicToken = FindObjectOfType<GameLogic>();
        slot1 = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseClick();
    }

    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    //Click Deck
                    DeckPressed();
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    //Click Card
                    CardPressed(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Top"))
                {
                    //Click Top
                    TopPressed();
                }
                else if (hit.collider.CompareTag("Bottom"))
                {
                    //Click Bottom
                    BottomPressed();
                }
                else { print("Nothing Here"); }
            }
        }
    }

    void DeckPressed() 
    {
        print("Deck Pressed");
        logicToken.DealFromDeck();
    }
    void CardPressed(GameObject selected)
    {
        print("Card Pressed");
        if (slot1 == this.gameObject)
        {
            slot1 = selected;
        }
        else if (slot1 != selected)
        {
            if (CheckStackable(selected))
            {

            }
            else
            {
                slot1 = selected;
            }
        }

    }
    void TopPressed()
    {
        print("Top Pressed");
    }
    void BottomPressed()
    {
        print("Bottom Pressed");
    }

    bool CheckStackable(GameObject selected)
    {
        SelectableSprite s1 = slot1.GetComponent<SelectableSprite>();
        SelectableSprite s2 = selected.GetComponent<SelectableSprite>();
        if (s2.top)
        {
            if (s1.suit == s2.suit || s1.value == 1 && s2.suit == null)
            {
                if (s1.value == s2.value + 1)
                {
                    return true;
                }
            }
            else { return false; }
        }
        else 
        {
            if (s1.value == s2.value -1)
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
                    print("Not Stackable");
                    return false;
                }
                else 
                { 
                    print("Stackable");
                    return true;
                }
            }
        }
        return false;
    }
}
