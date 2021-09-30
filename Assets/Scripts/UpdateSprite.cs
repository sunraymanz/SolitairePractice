using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cardFace;
    public Sprite cardBack;
    private SpriteRenderer sprToken;
    private SelectableSprite selectableToken;
    private GameLogic logicToken;
    // Start is called before the first frame update
    void Start()
    {
        List<string> deck = GameLogic.GenerateDeck();
        logicToken = FindObjectOfType<GameLogic>();
        int i = 0;
        foreach (string card in deck)
        {
            if (this.name == card)
            {
                cardFace = logicToken.cardFaces[i];
                break;
            }
            i++;
        }
        sprToken = GetComponent<SpriteRenderer>();
        selectableToken = GetComponent<SelectableSprite>();

    }

    // Update is called once per frame
    void Update()
    {
        if (selectableToken.isFaceUp)
        {
            sprToken.sprite = cardFace;
        }
        else { sprToken.sprite = cardBack; }
    }
}
