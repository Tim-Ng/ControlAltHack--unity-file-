using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;
using main;
using System.Linq;

namespace DrawCards
{
    /// <summary>
    /// This script is used to draw character cards. It also receives updates on which cards were drawn/used by other players. So that the 
    /// player wont draw the same cards as other players. Furthermore, with the data of the used cards they can 
    /// reshuffle the deck when all the cards in the deck are used.
    /// </summary>
    public class drawCharacterCard : MonoBehaviour
    {
        /// <summary>
        /// This holds the list of the charater card ID 
        /// </summary>
        private List<int> characterCardID = new List<int>();
        /// <summary>
        /// This is the gameObject where the character card will be place at.
        /// </summary>
        [SerializeField] private GameObject cardArea = null;
        /// <summary>
        /// This is the gameObject of the character card template
        /// </summary>
        [SerializeField] private GameObject cardTemplate = null;
        /// <summary>
        /// This is the script for the EventManager
        /// </summary>
        [SerializeField] private EventHandeler EventManager = null;
        /// <summary>
        /// When the script starts the function startDraw will be called
        /// </summary>
        private void Start()
        {
            startDraw();
        }
        /// <summary>
        /// This will add the all the character card ID into the list 
        /// </summary>
        /// <remarks>
        /// After adding into the list, it will be randomized
        /// </remarks>
        public void startDraw()
        {
            characterCardID.Clear();
            characterCardID.Add(1);
            characterCardID.Add(2);
            characterCardID.Add(3);
            characterCardID.Add(4);
            characterCardID.Add(5);
            characterCardID.Add(6);
            characterCardID.Add(7);
            characterCardID.Add(8);
            characterCardID.Add(9);
            characterCardID.Add(10);
            characterCardID.Add(11);
            characterCardID.Add(12);
            characterCardID.Add(13);
            characterCardID.Add(14);
            characterCardID.Add(15);
            characterCardID.Add(16);
            characterCardID = characterCardID.OrderBy(i => Guid.NewGuid()).ToList();
        }
        /// <summary>
        /// This function is used to draw cards 
        /// </summary>
        /// <param name="howmuch">How much card to be drawn</param>
        public void drawCharCards(int howmuch)
        {
            Debug.Log("Drawing Character cards ");
            for (int i = 0; i < howmuch; i++)
            {
                System.Random rand = new System.Random((int)DateTime.Now.Ticks);
                int x = rand.Next(0, characterCardID.Count - 1);
                Debug.Log("Card number is:" + characterCardID[x]);
                GameObject characterPlayerCard1 = Instantiate(cardTemplate, transform.position, Quaternion.identity);
                characterPlayerCard1.GetComponent<characterCardDisplay>().setID(characterCardID[x]);
                characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
                characterPlayerCard1.transform.SetParent(cardArea.transform, false);
                object[] cardID = new object[] { characterCardID[x] };
                characterCardID.Remove(characterCardID[x]);
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawCharacterRemove, cardID, EventManager.AllPeople, SendOptions.SendReliable);
            }
        }
        /// <summary>
        /// This is to take out the character card from the deck if it is drawn
        /// </summary>
        /// <param name="which"> this is the card ID that has been drawn</param>
        public void removeFormDeck(int which)
        {
            characterCardID.Remove(which);
        }
    }
}