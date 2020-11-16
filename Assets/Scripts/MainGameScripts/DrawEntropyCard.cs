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
    [SerializeField] private MoneyAndPoints moneyAndPointScripts;
    [SerializeField] private rollTime RollTime;
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
    public void restEntropyCard()
    {
        entropycards.Clear();
        Start();
    }
    public void distribute_entropycard(int how_many)
    {
        Debug.Log("Drawing card");
        if (how_many > (5 - moneyAndPointScripts.getMyAmountOfEntropyCards()) )
        {
            how_many = (5 -moneyAndPointScripts.getMyAmountOfEntropyCards());
        }
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
            if (entropycards[x].IsExtensiveExperience)
            {
                int HowMuchAdd;
                if (drawCharacterCard.getMyCharScript().find_which(entropycards[x].Title) >= 12)
                {
                    HowMuchAdd = 1;
                }
                else
                {
                    HowMuchAdd = (12 - drawCharacterCard.getMyCharScript().find_which(entropycards[x].Title));
                }
                RollTime.addSkillChanger(entropycards[x].Title, HowMuchAdd, "All");
            }
            RemoveThisCard(entropycards[x].EntropyCardID);
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.removeEntropycardFromdeck, data, AllOtherThanMePeopleOptions, SendOptions.SendReliable); // as this is not fast enough 
        }
        moneyAndPointScripts.countMyNumOfEntropyCards();
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
        if (entropycards.Count == 0)
        {
            entropycards = entropyCardDeck.getentropyCards();
        }
        Debug.Log("Number of Entropy cards left in deck " + entropycards.Count);
    }
    public EntropyCardScript FindWhichEntropyWithEntroID(int ID)
    {
        foreach (EntropyCardScript whichScript in entropyCardDeck.getentropyCards())
        {
            if (whichScript.EntropyCardID == ID)
            {
                return whichScript;
            }
        }
        return null;
    }
    //keep checking opponent amount

}
