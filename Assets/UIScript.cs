using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAgain()
    {
        panel.SetActive(false);
        ResetScene();
    }

    public void ResetScene()
    {
        UpdateSprite[] cards = FindObjectsOfType<UpdateSprite>();
        foreach (UpdateSprite card in cards)
        {
            Destroy(card.gameObject);
        }
        ClearTopValues();
        FindObjectOfType<UserInput>().Initate();
        FindObjectOfType<GameLogic>().InitCard();
    }

    void ClearTopValues() 
    {
        SelectableSprite[] selectables = FindObjectsOfType<SelectableSprite>();
        foreach (SelectableSprite selectable in selectables)
        {
            if (selectable.CompareTag("Top"))
            {
                selectable.suit = null;
                selectable.value = 0;
            }
        }
    }
}
