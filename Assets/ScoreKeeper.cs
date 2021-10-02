using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public SelectableSprite[] topStacks;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HasWon();
    }

    public void HasWon()
    {
        int i = 0;
        foreach (SelectableSprite topStack in topStacks)
        {
            i += topStack.value;
        }
        if (i >= 52)
        {
            Win();
        }
    }

    void Win()
    {
        panel.SetActive(true);
        print("You Have Won");
    }

}
