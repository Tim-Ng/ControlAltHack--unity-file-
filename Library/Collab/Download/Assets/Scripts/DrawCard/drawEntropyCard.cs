using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UserAreas;
using rollmissions;
using System.Linq;

namespace DrawCards {
    /// <summary>
    /// This script is used to draw entropy cards. It also receives updates on which cards were drawn/used by other players. So that the player wont 
    /// draw the same cards as other players. Furthermore, with the data of the used cards they can reshuffle the deck when all the cards in the deck are used.
    /// </summary>
    public class drawEntropyCard : MonoBehaviour
    {
        /// <summary>
        /// The gameobject this script is attatched to 
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// The list of entropy card ID 
        /// </summary>
        private List<int> entropyCardID = new List<int>();
        /// <summary>
        /// This is the list of the used entropy card ID 
        /// </summary>
        private List<int> entropyCardIDUsed = new List<int>();
        /// <summary>
        /// This is the gameobject of the area the card will be placed/initiated
        /// </summary>
        [SerializeField] private GameObject cardArea = null;
        /// <summary>
        /// This is the gameobject of the entropy card template
        /// </summary>
        [SerializeField] private GameObject cardTemplate = null;
        /// <summary>
        /// This is to hold the script of UserAreaControlers
        /// </summary>
        private UserAreaControlers userControler = null;
        /// <summary>
        /// This is to hold the script of EventHandeler
        /// </summary>
        private EventHandeler EventManager = null;
        /// <summary>
        /// This is to hold the script of rollingMissionControl
        /// </summary>
        private rollingMissionControl rollingMission = null;
        /// <summary>
        /// This function will run the this script is rendered.
        /// </summary>
        /// <remarks>
        /// This will also set the varaible of scripts 
        /// The function startDraw() will also be called.
        /// </remarks>
        private void Start()
        {
            ScriptsODJ = gameObject;
            userControler = ScriptsODJ.GetComponent<UserAreaControlers>();
            EventManager = ScriptsODJ.GetComponent<EventHandeler>();
            rollingMission = ScriptsODJ.GetComponent<rollingMissionControl>();
            startDraw();
        }
        /// <summary>
        /// This will setup the entropyCardID list and clear the entropyCardIDUsed list
        /// </summary>
        /// <remarks>
        /// After the list is setup the list will be randomized
        /// </remarks>
        public void startDraw()
        {
            entropyCardIDUsed.Clear();
            entropyCardID.Clear();
            entropyCardID.Add(1);
            entropyCardID.Add(2);
            entropyCardID.Add(3);
            entropyCardID.Add(4);
            entropyCardID.Add(5);
            entropyCardID.Add(6);
            entropyCardID.Add(7);
            entropyCardID.Add(8);
            entropyCardID.Add(9);
            entropyCardID.Add(10);
            entropyCardID.Add(11);
            entropyCardID.Add(12);
            entropyCardID.Add(13);
            entropyCardID.Add(14);
            entropyCardID.Add(15);
            entropyCardID.Add(16);
            entropyCardID.Add(17);
            entropyCardID.Add(18);
            entropyCardID.Add(19);
            entropyCardID.Add(20);
            entropyCardID.Add(21);
            entropyCardID.Add(22);
            entropyCardID.Add(23);
            entropyCardID.Add(24);
            entropyCardID.Add(25);
            entropyCardID.Add(26);
            entropyCardID.Add(27);
            entropyCardID.Add(28);
            entropyCardID.Add(29);
            entropyCardID.Add(30);
            entropyCardID.Add(31);
            entropyCardID = entropyCardID.OrderBy(i => Guid.NewGuid()).ToList();
        }
        /// <summary>
        /// This is the function to draw the entropy cards
        /// </summary>
        /// <param name="howmuch">The amount of cards to be drawn</param>
        /// <remarks>
        /// If the amount of Entropy cards of the player is more than 5 then the draw will stop as a player cannot have more than 5 entropy cards
        /// </remarks>
        public void drawEntropyCards(int howmuch)
        {
            Debug.Log("Drawing Character cards ");
            for (int i = 0; i < howmuch; i++)
            {
                if (userControler.users[0].NumberOfCards <= 4)
                {
                    if (entropyCardID.Count == 0)
                    {
                        entropyCardID.AddRange(entropyCardIDUsed);
                        entropyCardIDUsed.Clear();
                        Debug.LogWarning("Reimport deck entropy");
                    }
                    System.Random rand = new System.Random((int)DateTime.Now.Ticks);
                    int entropyID = entropyCardID[rand.Next(0, entropyCardID.Count - 1)];
                    GameObject characterPlayerCard1 = Instantiate(cardTemplate, transform.position, Quaternion.identity);
                    characterPlayerCard1.GetComponent<entropyCardDisplay>().setID(entropyID);
                    characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
                    characterPlayerCard1.transform.SetParent(cardArea.transform, false);
                    object[] cardID = new object[] { entropyID };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawEntropyRemove, cardID, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                    entropyCardID.Remove(entropyID);
                    userControler.users[0].NumberOfCards += 1;
                }
                else 
                    break;
            }
            userControler.sendAmountOfCards();
        }
        /// <summary>
        /// To remove the Card that had been drawn by you or the other player from the list as well as adding it to the used list [which is the function addToPlayedDeck]
        /// </summary>
        /// <param name="which">The card ID</param>
        public void removeFormDeck(int which)
        {
            entropyCardID.Remove(which);
            if (entropyCardID.Count == 0)
            {
                entropyCardID.AddRange(entropyCardIDUsed);
                entropyCardIDUsed.Clear();
                Debug.LogWarning("Reimport deck entropy");
            }
        }
        /// <summary>
        /// To add a card ID into the used entropy card deck list
        /// </summary>
        /// <param name="which">The card ID to be added</param>
        public void addToPlayedDeck(int which)
        {
            Debug.Log("Added card to the used card : " + which);
            if (!entropyCardIDUsed.Contains(which))
            {
                entropyCardIDUsed.Add(which);
            }
        }
        /// <summary>
        /// This function is used to remove of a card from the entropy card area
        /// </summary>
        /// <param name="whichScript">The entropy card to be deleted</param>
        /// <param name="discarding">Only true if it is click on the dicard button on the entropy card popup,if you dont want this to be counted in the rollingMission</param>
        public void removeAnEntropyCard(EntropyCardScript whichScript,bool discarding)
        {
            foreach (Transform child in cardArea.transform)
            {
                if (child.gameObject.GetComponent<entropyCardDisplay>().getInfo() == whichScript)
                {
                    object[] whichCard = new object[] { child.gameObject.GetComponent<entropyCardDisplay>().getInfo().EntropyCardID };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawEntropyUsed, whichCard, EventManager.AllPeople, SendOptions.SendReliable);
                    GameObject.Destroy(child.gameObject);
                    userControler.users[0].NumberOfCards -= 1;
                    break;
                }
            }
            userControler.sendAmountOfCards();
            if (discarding)
                rollingMission.removedAnEntropy();
        }
        /// <summary>
        /// This is to remove all entropy cards the player have
        /// </summary>
        public void removeAllEntropyCard()
        {
            foreach (Transform child in cardArea.transform)
            {
                object[] whichCard = new object[] { child.gameObject.GetComponent<entropyCardDisplay>().getInfo().EntropyCardID };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawEntropyUsed, whichCard, EventManager.AllPeople, SendOptions.SendReliable);
                GameObject.Destroy(child.gameObject);
                userControler.users[0].NumberOfCards -= 1;
            }
            userControler.sendAmountOfCards();
        }
    }
}
