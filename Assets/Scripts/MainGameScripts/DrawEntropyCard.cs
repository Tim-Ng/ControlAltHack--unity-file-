using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DrawEntropyCard : MonoBehaviourPunCallbacks
{
    public GameObject EntropycardTemplate;
    public EntropyCardDeck entropyCardDeck;
    public GameObject UserArea;
    public DrawCharacterCard drawCharacterCard;
    //inputmultiple cards
    private int x;
    private List<EntropyCardScript> entropycards = new List<EntropyCardScript>();
    private RaiseEventOptions AllOtherThanMePeopleOptions = new RaiseEventOptions()
    {
        CachingOption = EventCaching.DoNotCache,
        Receivers = ReceiverGroup.Others,
    };
    private RaiseEventOptions AllPeople = new RaiseEventOptions()
    {
        CachingOption = EventCaching.DoNotCache,
        Receivers = ReceiverGroup.All
    };
    public enum PhotonEventCode
    {
        removeEntropycardFromdeck = 7, 
    }
    private void OnEnable()
    {
        Debug.Log("Listen to event");
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        Debug.Log("Event heard");
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
        Debug.Log("Event Ended");
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)PhotonEventCode.removeEntropycardFromdeck)
        {
            object[] carddata = (object[])obj.CustomData;
            int datacode = (int)carddata[0];
            RemoveThisCard(datacode);
        }
        
    }
    private void Start()
    {
        //add all cards to a list here
        entropycards = entropyCardDeck.getentropyCards();
    }
    public void distribute_entropycard(int how_many)
    {
        Debug.Log("Drawing card");
        for (var i = 0; i < how_many; i++)
        {
            System.Random rand = new System.Random((int)DateTime.Now.Ticks);
            x = rand.Next(0, (entropycards.Count));
            GameObject entropyCard = Instantiate(EntropycardTemplate, transform.position, Quaternion.identity);
            entropyCard.GetComponent<EntropyCardDisplay>().entropyData = entropycards[x];
            entropyCard.GetComponent<EntropyCardDisplay>().FrontSide.SetActive(true);
            entropyCard.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
            entropyCard.transform.SetParent(UserArea.transform, false);
            object[] data = new object[] { entropycards[x].EntropyCardID };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.removeEntropycardFromdeck, data, AllOtherThanMePeopleOptions, SendOptions.SendReliable); // as this is not fast enough 
            RemoveThisCard(entropycards[x].EntropyCardID); //so this is used
            Thread.Sleep(175);
        }
    }
    public void RemoveThisCard(int cardID)
    {
        foreach (EntropyCardScript entropyscripts in entropycards)
        {
            if (cardID == entropyscripts.EntropyCardID)
            {
                entropycards.Remove(entropyscripts);
                Debug.Log("Entropy ID :" + cardID + " is removed");
                break;
            }
        }
        Debug.Log("Number of Entropy cards left in deck " + entropycards.Count);
    }

    //keep checking opponent amount

}
