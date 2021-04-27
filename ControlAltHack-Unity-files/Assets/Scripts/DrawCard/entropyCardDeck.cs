﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DrawCards
{
    /// <summary>
    /// This is a class to hold all the data for all the entropy cards.
    /// </summary>
    public class entropyCardDeck : MonoBehaviour
    {
        /// <summary>
        /// This is where the entropy card data is placed
        /// </summary>
        [SerializeField]
        private EntropyCardScript
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
                card31 = null;
        /// <summary>
        /// This is the list to hold all the entropy data of the deck where it can be accessed anywhere
        /// </summary>
        public static List<EntropyCardScript> cardDeck = new List<EntropyCardScript>(31);
        /// <summary>
        /// This is to input all the entropy data in to the list when the script is loaded
        /// </summary>
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
        }
    }
}