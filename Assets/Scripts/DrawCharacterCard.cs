using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class DrawCharacterCard : MonoBehaviour
{
    public GameObject PlayerArea;
    public GameObject OponentArea1;
    public GameObject OponentArea2;
    public GameObject OponentArea3;
    public GameObject OponentArea4;
    public GameObject OponentArea5;
    public GameObject CharCard1;
    public GameObject CharCard2;
    public GameObject CharCard3;
    public GameObject CharCard4;
    public GameObject CharCard5;
    public GameObject CharCard6;
    public GameObject CharCard7;
    public GameObject CharCard8;
    public GameObject CharCard9;
    public GameObject CharCard10;
    public GameObject CharCard11;
    public GameObject CharCard12;
    public GameObject button;
    private int x;

    List<GameObject> cards = new List<GameObject>();

    void Start()
    {
        cards.Add(CharCard1);
        cards.Add(CharCard2);
        cards.Add(CharCard3);
        cards.Add(CharCard4);
        cards.Add(CharCard5);
        cards.Add(CharCard6);
        cards.Add(CharCard7);
        cards.Add(CharCard8);
        cards.Add(CharCard9);
        cards.Add(CharCard10);
        cards.Add(CharCard11);
        cards.Add(CharCard12);
    }

    public void OnClick()
    {
        for (var i = 0; i < 3; i++)
        {
            x =Random.Range(0, cards.Count);
            GameObject characterPlayerCard1 = Instantiate(cards[x], transform.position, Quaternion.identity);
            characterPlayerCard1.transform.SetParent(PlayerArea.transform, false);
            cards.Remove(cards[x]);
        }
        button.SetActive(false);

    }
}
