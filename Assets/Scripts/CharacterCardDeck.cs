using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCardDeck : MonoBehaviour
{
    private List<CharCardScript> cardsInfo = new List<CharCardScript>();
    public CharCardScript CharCard1, CharCard2, CharCard3, CharCard4, CharCard5, CharCard6, CharCard7,
         CharCard8, CharCard9, CharCard10, CharCard11,CharCard12, CharCard13, CharCard14, CharCard15, CharCard16 ;
    void Start()
    {
        cardsInfo.Add(CharCard1);
        cardsInfo.Add(CharCard2);
        cardsInfo.Add(CharCard3);
        cardsInfo.Add(CharCard4);
        cardsInfo.Add(CharCard5);
        cardsInfo.Add(CharCard6);
        cardsInfo.Add(CharCard7);
        cardsInfo.Add(CharCard8);
        cardsInfo.Add(CharCard9);
        cardsInfo.Add(CharCard10);
        cardsInfo.Add(CharCard11);
        cardsInfo.Add(CharCard12);
        cardsInfo.Add(CharCard13);
        cardsInfo.Add(CharCard14);
        cardsInfo.Add(CharCard15);
        cardsInfo.Add(CharCard16);
    }
    public List<CharCardScript> getCharDeck()
    { 
        return cardsInfo;
    }
}
