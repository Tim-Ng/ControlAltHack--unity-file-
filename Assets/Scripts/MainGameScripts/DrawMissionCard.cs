using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DrawMissionCard : MonoBehaviour
{
    public MissionCardDeck missionCardDeck;
    private List<MissionCardScript> missionCardToDraw = new List<MissionCardScript>();

    private MissionCardScript curruntMissoinScript;
    public GameObject MissioncardTemplate;
    public GameObject PlayerMissionArea;
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
        removeEntropycardFromdeck = 8,
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
            object[] data = (object[])obj.CustomData;
            RemoveThisCard((string)data[0]);
        }
    }
    private void Start()
    {
        missionCardToDraw = missionCardDeck.getMissionCardDeck();
    }
    public void whoDrawMissionCard(int CharacterCardId)
    {
        Debug.Log("Checking Who draw");
        if(CharacterCardId == 9 )
        {
            drawMissionCard(2);
        }
        else
        {
            drawMissionCard(1);
        }
    }
    public void drawMissionCard(int how_many)
    {
        for (var i = 0; i < how_many; i++)
        {
            System.Random rand = new System.Random((int)DateTime.Now.Ticks);
            int x = rand.Next(0, (missionCardToDraw.Count));
            GameObject missionCard = Instantiate(MissioncardTemplate, transform.position, Quaternion.identity);
            missionCard.GetComponent<MissionCardDisplay>().mission_script = missionCardToDraw[x];
            missionCard.GetComponent<MissionCardDisplay>().FrontSide.SetActive(true);
            missionCard.gameObject.transform.localScale += new Vector3(-0.75f, -0.75f, 0);
            missionCard.transform.SetParent(PlayerMissionArea.transform, false);
            object[] data = new object[] { missionCardToDraw[x].Mission_code };
            RemoveThisCard(missionCardToDraw[x].Mission_code);
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.removeEntropycardFromdeck, data, AllOtherThanMePeopleOptions, SendOptions.SendReliable); // as this is not fast enough
            Thread.Sleep(175);
        }
    }
    public void RemoveThisCard(string cardID)
    {
        foreach (MissionCardScript missionscripts in missionCardToDraw)
        {
            if (cardID == missionscripts.Mission_code)
            {
                missionCardToDraw.Remove(missionscripts);
                Debug.Log("Entropy ID :" + cardID + " is removed");
                break;
            }
        }
        if (missionCardToDraw.Count == 0)
        {
            missionCardToDraw = missionCardDeck.getMissionCardDeck();
        }
        Debug.Log("Number of Mission cards left in deck " + missionCardToDraw.Count);
    }
}
