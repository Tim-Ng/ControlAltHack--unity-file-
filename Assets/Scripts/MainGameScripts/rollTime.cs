using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rollTime : MonoBehaviour
{
    [SerializeField] private GameObject rollMissionTime;
    [SerializeField] private GameObject missionCardOBJ;
    [SerializeField] private Text rollingPlayerNickName, DiceNumber,
        ChancesLeft, WhichTask, WhichSkill, RollMustBeMoreThan;
    [SerializeField] GameObject RollButton;
    [SerializeField] DrawCharacterCard drawCharacterCard;

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
        whoRolling = 15,
    }
    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)PhotonEventCode.whoRolling)
        {
            object[] receiveData = (object[])obj.CustomData;
           
        }
    }
    public void startRollTurn()
    {
        if (true)
        {
            Debug.LogError("Problem its not Roll Time");
        }
        else
        {
            if (drawCharacterCard.IsMyTurn)
            {
                missionCardOBJ.GetComponentInChildren<MissionCardDisplay>().mission_script = drawCharacterCard.getCurrentMissionScript();
            }
        }
    }
}
