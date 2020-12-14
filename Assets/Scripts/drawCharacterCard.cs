using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;
using main;

namespace DrawCards
{
    public class drawCharacterCard : MonoBehaviour
    {
        private List<int> characterCardID = new List<int>();
        [SerializeField] private GameObject cardArea = null, cardTemplate = null;
        [SerializeField] private EventHandeler EventManager = null;
        private void Start()
        {
            startDraw();
        }
        private void startDraw()
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
        }
        public void drawCharCards(int howmuch)
        {
            Debug.Log("Drawing Character cards ");
            for (var i = 0; i < howmuch; i++)
            {
                System.Random rand = new System.Random((int)DateTime.Now.Ticks);
                int x = rand.Next(0, characterCardID.Count -1 );
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
        public void removeFormDeck(int which)
        {
            characterCardID.Remove(which);
        }
    }
}