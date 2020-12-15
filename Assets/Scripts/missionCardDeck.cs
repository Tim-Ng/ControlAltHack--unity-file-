using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DrawCards
{
    public class missionCardDeck : MonoBehaviour
    {
        [SerializeField]
        private MissionCardScript
                card1 = null,
                card2 = null,
                card3 = null,
                card4 = null,
                card5 = null,
                card6 = null,
                card7 = null,
                card8 = null,
                card9 = null,
                card10 = null,
                card11 = null,
                card12 = null,
                card13 = null,
                card14 = null,
                card15 = null,
                card16 = null,
                card17 = null,
                card18 = null,
                card19 = null,
                card20 = null,
                card21 = null,
                card22 = null,
                card23 = null,
                card24 = null,
                card25 = null,
                card26 = null,
                card27 = null,
                card28 = null,
                card29 = null,
                card30 = null,
                card31 = null,
                card32 = null,
                card33 = null,
                card34 = null,
                card35 = null,
                card36 = null,
                card37 = null;
        public static List<MissionCardScript> cardDeck = new List<MissionCardScript>(37);
        private void Start()
        {
            cardDeck.Add(card1);
            cardDeck.Add(card2);
            cardDeck.Add(card3);
            cardDeck.Add(card4);
            cardDeck.Add(card5);
            cardDeck.Add(card6);
            cardDeck.Add(card7);
            cardDeck.Add(card8);
            cardDeck.Add(card9);
            cardDeck.Add(card10);
            cardDeck.Add(card11);
            cardDeck.Add(card12);
            cardDeck.Add(card13);
            cardDeck.Add(card14);
            cardDeck.Add(card15);
            cardDeck.Add(card16);
            cardDeck.Add(card17);
            cardDeck.Add(card18);
            cardDeck.Add(card19);
            cardDeck.Add(card20);
            cardDeck.Add(card21);
            cardDeck.Add(card22);
            cardDeck.Add(card23);
            cardDeck.Add(card24);
            cardDeck.Add(card25);
            cardDeck.Add(card26);
            cardDeck.Add(card27);
            cardDeck.Add(card28);
            cardDeck.Add(card29);
            cardDeck.Add(card30);
            cardDeck.Add(card31);
            cardDeck.Add(card32);
            cardDeck.Add(card33);
            cardDeck.Add(card34);
            cardDeck.Add(card35);
            cardDeck.Add(card36);
            cardDeck.Add(card37);
        }
    }
}
