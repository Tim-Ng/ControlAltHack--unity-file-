using System.Collections;
using UserAreas;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

namespace DrawCards
{
    public class drawCharacterCard : MonoBehaviour
    {
        private List<int> characterCardID = new List<int>();
        [SerializeField] private GameObject cardArea,cardTemplate;
        [SerializeField]
        private CharCardScript
            card1,
            card2,
            card3,
            card4,
            card5,
            card6,
            card7,
            card8,
            card9,
            card10,
            card11,
            card12,
            card13,
            card14,
            card15,
            card16;
        public static List<CharCardScript> cardDeck = new List<CharCardScript>(16);
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
                int x = rand.Next(1, characterCardID.Count);
                Debug.Log("Card number is:" + characterCardID[x]);
                GameObject characterPlayerCard1 = Instantiate(cardTemplate, transform.position, Quaternion.identity);
                characterPlayerCard1.GetComponent<characterCardDisplay>().setID(characterCardID[x]);
                characterPlayerCard1.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
                characterPlayerCard1.transform.SetParent(cardArea.transform, false);
                characterCardID.Remove(characterCardID[x]);
                UserAreaControlers.pv.RPC("removeFormDeck", RpcTarget.Others, characterCardID[x]);
            }
        }
        [PunRPC]
        private void removeFormDeck(int which)
        {
            characterCardID.Remove(which);
        }
    }
}