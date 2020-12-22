using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserAreas;
namespace DrawCards {
    public class drawMissionCard : MonoBehaviour
    {
        private List<int> missionCardID = new List<int>();
        private List<int> missionCardIDUsed = new List<int>();
        [SerializeField] private UserAreaControlers userControler = null;
        [SerializeField] private GameObject cardArea = null, cardTemplateMission = null;
        [SerializeField] private EventHandeler EventManager = null;
        private void Start()
        {
            startDraw();
        }
        private void startDraw()
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
        }
        public void drawMissionCards(int howmuch)
        {
            Debug.Log("Drawing Character cards ");
            for (int i = 0; i < howmuch; i++)
            {
                System.Random rand = new System.Random((int)DateTime.Now.Ticks);
                int x = rand.Next(0, missionCardID.Count - 1);
                Debug.Log("Card number is:" + missionCardID[x]);
                GameObject characterPlayerCard1 = Instantiate(cardTemplateMission, transform.position, Quaternion.identity);
                characterPlayerCard1.GetComponent<missionDisplay>().setID(missionCardID[x]);
                characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
                characterPlayerCard1.transform.SetParent(cardArea.transform, false);
                object[] cardID = new object[] { missionCardID[x] };
                missionCardID.Remove(missionCardID[x]);
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawMissionRemove, cardID, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                userControler.users[0].MissionCards += 1;
            }
            userControler.sendAmountOfCards();
        }
        public void removeFormDeck(int which)
        {
            missionCardID.Remove(which);
            if (missionCardID.Count == 0)
            {
                missionCardID = missionCardIDUsed;
                missionCardIDUsed.Clear();
            }
        }
        public void addToPlayedDeck(int which)
        {
            Debug.Log("Added card to the used card : " + which);
            missionCardIDUsed.Add(which);
        }
        public void removeAllCard()
        {
            foreach (Transform child in cardArea.transform)
            {
                object[] whichCard = new object[] { child.gameObject.GetComponent<MissionCardScript>().Mission_code };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.drawMissionUsed, whichCard, EventManager.AllPeople, SendOptions.SendReliable);
                GameObject.Destroy(child.gameObject);
                userControler.users[0].MissionCards -= 1;
            }
        }
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
