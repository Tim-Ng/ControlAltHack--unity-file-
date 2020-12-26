using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserAreas;
using rollmissions;
using System.Threading;

namespace DrawCards {
    public class drawEntropyCard : MonoBehaviour
    {
        public List<int> entropyCardID = new List<int>();
        public List<int> entropyCardIDUsed = new List<int>();
        [SerializeField] private UserAreaControlers userControler = null;
        [SerializeField] private GameObject cardArea = null, cardTemplate = null;
        [SerializeField] private EventHandeler EventManager = null;
        [SerializeField] private rollingMissionControl rollingMission = null;
        private void Start()
        {
            startDraw();
        }
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
        }
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
                    int x = rand.Next(0, entropyCardID.Count - 1);
                    Debug.Log("Card number is:" + entropyCardID[x]);
                    int entropyID = entropyCardID[x];
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
        public void addToPlayedDeck(int which)
        {
            Debug.Log("Added card to the used card : " + which);
            entropyCardIDUsed.Add(which);
        }
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
