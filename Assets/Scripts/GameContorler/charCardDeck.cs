using System.Collections.Generic;
using UnityEngine;

namespace DrawCards {
    /// <summary>
    /// This is a class to hold all the data of the character cards 
    /// </summary>
    public class charCardDeck : MonoBehaviour
    {
        /// <summary>
        /// This is where the card data is placed
        /// </summary>
        [SerializeField] private CharCardScript
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
                card16 = null;
        /// <summary>
        /// This is the list to hold all the data of the deck where it can be accessed anywhere
        /// </summary>
        public static List<CharCardScript> cardDeck = new List<CharCardScript>(16);
        /// <summary>
        /// This is to input all the data in to the list when the script is loaded
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
        }
    }
}
