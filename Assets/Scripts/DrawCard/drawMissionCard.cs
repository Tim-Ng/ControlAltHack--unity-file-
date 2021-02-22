using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UserAreas;
namespace DrawCards {
    /// <summary>
    /// This is to draw the Mission card
    /// </summary>
    public class drawMissionCard : MonoBehaviour
    {
        /// <summary>
        /// This holds the list of the mission card ID 
        /// </summary>
        private List<int> missionCardID = new List<int>();
        /// <summary>
        /// This is the list of the used mission card ID 
        /// </summary>
        private List<int> missionCardIDUsed = new List<int>();
        /// <summary>
        /// The gameobject this script is attatched to 
        /// </summary>
        private GameObject ScriptOBJ = null;
        /// <summary>
        /// This is to hold the script of UserAreaControlers
        /// </summary>
        private UserAreaControlers userControler = null;
        /// <summary>
        /// This is to hold the script of EventHandeler
        /// </summary>
        private EventHandeler EventManager = null;
        /// <summary>
        /// This is the gameobject of the area the card will be placed/initiated
        /// </summary>
        [SerializeField] private GameObject cardArea = null;
        /// <summary>
        /// This is the gameobject of the misssion card template
        /// </summary>
        [SerializeField] private GameObject cardTemplateMission = null;
        /// <summary>
        /// This function will run the this script is rendered.
        /// </summary>
        /// <remarks>
        /// This will also set the varaible of scripts 
        /// The function startDraw() will also be called.
        /// </remarks>
        private void Start()
        {
            ScriptOBJ = gameObject;
            userControler = ScriptOBJ.GetComponent<UserAreaControlers>();
            EventManager = ScriptOBJ.GetComponent<EventHandeler>();
            startDraw();
        }
        /// <summary>
        /// This will setup the missionCardID list and clear the missionCardIDUsed list
        /// </summary>
        /// <remarks>
        /// After the list is setup the list will be randomized
        /// </remarks>
        public void startDraw()
        {
            missionCardIDUsed.Clear();
            missionCardID.Clear();
            missionCardID.Add(1);
            missionCardID.Add(2);
            missionCardID.Add(3);
            missionCardID.Add(4);
            missionCardID.Add(5);
            missionCardID.Add(6);
            missionCardID.Add(7);
            missionCardID.Add(8);
            missionCardID.Add(9);
            missionCardID.Add(10);
            missionCardID.Add(11);
            missionCardID.Add(12);
            missionCardID.Add(13);
            missionCardID.Add(14);
            missionCardID.Add(15);
            missionCardID.Add(16);
            missionCardID.Add(17);
            missionCardID.Add(18);
            missionCardID.Add(19);
            missionCardID.Add(20);
            missionCardID.Add(21);
            missionCardID.Add(22);
            missionCardID.Add(23);
            missionCardID.Add(24);
            missionCardID.Add(25);
            missionCardID.Add(26);
            missionCardID.Add(27);
            missionCardID.Add(28);
            missionCardID.Add(29);
            missionCardID.Add(30);
            missionCardID.Add(31);
            missionCardID.Add(32);
            missionCardID.Add(33);
            missionCardID.Add(34);
            missionCardID.Add(35);
            missionCardID.Add(36);
            missionCardID.Add(37);
            missionCardID = missionCardID.OrderBy(i => Guid.NewGuid()).ToList();
        }
        /// <summary>
        /// This is the function to draw the mission cards
        /// </summary>
        /// <param name="howmuch">The amount of cards to be drawn</param>
        public void drawMissionCards(int howmuch)
        {
            Debug.Log("Drawing Mission cards ");
            for (int i = 0; i < howmuch; i++)
            {
                if (missionCardID.Count == 0)
                {
                    missionCardID.AddRange( missionCardIDUsed);
                    missionCardIDUsed.Clear();
                    Debug.LogWarning("Reimport deck mission");
                }
                System.Random rand = new System.Random((int)DateTime.Now.Ticks);
                int x = rand.Next(0, missionCardID.Count - 1);
                Debug.Log("Card number is:" + missionCardID[x]);
                GameObject characterPlayerCard1 = Instantiate(cardTemplateMission, transform.position, Quaternion.identity);
                characterPlayerCard1.GetComponent<missionDisplay>().setID(missionCardID[x]);
                characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
                characterPlayerCard1.transform.SetParent(cardArea.transform, false);
                object[] cardID = new object[] { missionCardID[x] };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawMissionRemove, cardID, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                missionCardID.Remove(missionCardID[x]);
                userControler.users[0].MissionCards += 1;
            }
        }
        /// <summary>
        /// To remove the Card that had been drawn by you or the other player from the list as well as adding it to the used list [which is the function addToPlayedDeck]
        /// </summary>
        /// <param name="which">The card ID</param>
        public void removeFormDeck(int which)
        {
            missionCardID.Remove(which);
            if (missionCardID.Count == 0)
            {
                missionCardID.AddRange(missionCardIDUsed);
                missionCardIDUsed.Clear();
                Debug.LogWarning("Reimport deck mission");
            }
        }
        /// <summary>
        /// To add a card ID into the used mission card deck list
        /// </summary>
        /// <param name="which">The card ID to be added</param>
        public void addToPlayedDeck(int which)
        {
            Debug.Log("Added card to the used card : " + which);
            missionCardIDUsed.Add(which);
        }
        /// <summary>
        /// This is to remove all misssion cards the player have
        /// </summary>
        public void removeAllCard()
        {
            foreach (Transform child in cardArea.transform)
            {
                GameObject gameItem = child.gameObject;
                object[] whichCard = new object[] { gameItem.GetComponent<missionDisplay>().getInfo().Mission_code };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawMissionUsed, whichCard, EventManager.AllPeople, SendOptions.SendReliable);
                GameObject.Destroy(gameItem);
                userControler.users[0].MissionCards -= 1;
            }
        }
        /// <summary>
        /// This is to remove a misssion cards from the player
        /// </summary>
        /// <param name="whichScript">The script to be removed</param>
        public void removeAnMissionCard(MissionCardScript whichScript)
        {
            foreach (Transform child in cardArea.transform)
            {
                if (child.gameObject.GetComponent<missionDisplay>().getInfo() == whichScript)
                {
                    GameObject.Destroy(child.gameObject);
                    userControler.users[0].MissionCards -= 1;
                    break;
                }
            }
            object[] whichCard = new object[] { whichScript.Mission_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawMissionUsed, whichCard, EventManager.AllPeople, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is to remove all mission card but one from the player
        /// </summary>
        /// <param name="whichScript">The mission card not to be deleted</param>
        public void removeOtherThanMissionCard(MissionCardScript whichScript)
        {
            foreach (Transform child in cardArea.transform)
            {
                if (child.gameObject.GetComponent<missionDisplay>().getInfo() != whichScript)
                {
                    GameObject.Destroy(child.gameObject);
                    userControler.users[0].MissionCards -= 1;
                    object[] whichCard = new object[] { child.gameObject.GetComponent<missionDisplay>().getInfo().Mission_code };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawMissionUsed, whichCard, EventManager.AllPeople, SendOptions.SendReliable);
                    break;
                }
            }
        }
    }
}
