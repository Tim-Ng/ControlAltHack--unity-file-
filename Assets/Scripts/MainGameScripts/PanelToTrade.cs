using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PanelToTrade : MonoBehaviour
{
    public Text opponent1Nick, opponent2Nick, opponent3Nick, opponent4Nick, opponent5Nick;
    [SerializeField] private GameObject userBoxOBJ,opponent1BoxOBJ, opponent2BoxOBJ, opponent3BoxOBJ, opponent4BoxOBJ, opponent5BoxOBJ;
    [SerializeField] private GameObject userBoxCardOBJ,opponent1BoxCardOBJ, opponent2BoxCardOBJ, opponent3BoxCardOBJ, opponent4BoxCardOBJ, opponent5BoxCardOBJ;
    private bool opponentAttend1=false, opponentAttend2 = false, opponentAttend3 = false, opponentAttend4 = false, opponentAttend5 = false;
    private Player opponent1player, opponent2player, opponent3player, opponent4player, opponent5player;
    private MissionCardScript userScript,oppenentScript1, oppenentScript2, oppenentScript3, oppenentScript4, oppenentScript5;
    private List<MissionCardScript> opponentScriptList = new List<MissionCardScript>();
    private List<GameObject> opponentBoxOBJ = new List<GameObject>();
    private List<GameObject> opponentBoxCardOBJ = new List<GameObject>();
    private List<Text> NickNameOtherList = new List<Text>();
    private List<Player> otherPlayerList = new List<Player>();
    private List<bool> otherPlayerAttend = new List<bool>();
    public MissionCardDeck missionCard;
    [SerializeField] private GameObject opponentSkipped1, opponentSkipped2, opponentSkipped3, opponentSkipped4, opponentSkipped5;
    private List<GameObject> otherPlayerSkippedOBJ = new List<GameObject>();
    [SerializeField] private DrawCharacterCard drawMissionCard; 
    [SerializeField] private popupcardwindowMission popupcardwindowMissionScript;
    [SerializeField] private Main_Game_before_start main_Game_Before_Start;
    [SerializeField] private GameObject panelTrade;
    private int everyoneIsDone = 0;
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
        whoDoneAttendOrNot = 11,
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
        if (obj.Code == (byte)PhotonEventCode.whoDoneAttendOrNot)
        {
            object[] receiveData = (object[])obj.CustomData;
            oneDone((string)receiveData[0], (bool)receiveData[1],(Player)receiveData[2]);
        }
    }
    private void Start()
    {
        userScript = drawMissionCard.getCurrentMissionScript();
        otherPlayerSkippedOBJ.Add(opponentSkipped1);
        otherPlayerSkippedOBJ.Add(opponentSkipped2);
        otherPlayerSkippedOBJ.Add(opponentSkipped3);
        otherPlayerSkippedOBJ.Add(opponentSkipped4);
        otherPlayerSkippedOBJ.Add(opponentSkipped5);
        otherPlayerAttend.Add(opponentAttend1);
        otherPlayerAttend.Add(opponentAttend2);
        otherPlayerAttend.Add(opponentAttend3);
        otherPlayerAttend.Add(opponentAttend4);
        otherPlayerAttend.Add(opponentAttend5);
        opponentBoxCardOBJ.Add(opponent1BoxCardOBJ);
        opponentBoxCardOBJ.Add(opponent2BoxCardOBJ);
        opponentBoxCardOBJ.Add(opponent3BoxCardOBJ);
        opponentBoxCardOBJ.Add(opponent4BoxCardOBJ);
        opponentBoxCardOBJ.Add(opponent5BoxCardOBJ);
        otherPlayerList.Add(opponent1player);
        otherPlayerList.Add(opponent2player);
        otherPlayerList.Add(opponent3player);
        otherPlayerList.Add(opponent4player);
        otherPlayerList.Add(opponent5player);
        opponentScriptList.Add(oppenentScript1);
        opponentScriptList.Add(oppenentScript2);
        opponentScriptList.Add(oppenentScript3);
        opponentScriptList.Add(oppenentScript4);
        opponentScriptList.Add(oppenentScript5);
        NickNameOtherList.Add(opponent1Nick);
        NickNameOtherList.Add(opponent2Nick);
        NickNameOtherList.Add(opponent3Nick);
        NickNameOtherList.Add(opponent4Nick);
        NickNameOtherList.Add(opponent5Nick);
        opponentBoxOBJ.Add(opponent1BoxOBJ);
        opponentBoxOBJ.Add(opponent2BoxOBJ);
        opponentBoxOBJ.Add(opponent3BoxOBJ);
        opponentBoxOBJ.Add(opponent4BoxOBJ);
        opponentBoxOBJ.Add(opponent5BoxOBJ);
        RestTrades();
    }
    public void clickOnAttend()
    {
        RestTrades();
        popupcardwindowMissionScript.closePopup();
        sendEveryOneDone(true);
    }
    public void clickOnNotAttend()
    {
        RestTrades();
        popupcardwindowMissionScript.closePopup();
        sendEveryOneDone(false);
    }
    public void sendEveryOneDone(bool AttendOrNot)
    {
        userScript = drawMissionCard.getCurrentMissionScript();
        object[] data = new object[] { userScript.Mission_code,AttendOrNot, PhotonNetwork.LocalPlayer};
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoDoneAttendOrNot, data, AllPeople, SendOptions.SendReliable);
    }
    public void oneDone(string sentMissionID, bool AttendOrNot,Player playerSent)
    {
        int j = 0;
        if (playerSent != PhotonNetwork.LocalPlayer) {
            foreach (Player listPlayer in PhotonNetwork.PlayerListOthers)
            {
                Debug.Log("Add player to list");
                if (listPlayer == playerSent)
                {
                    foreach (MissionCardScript checkMissionScript in missionCard.getMissionCardDeck())
                    {
                        if (checkMissionScript.Mission_code == sentMissionID)
                        {
                            opponentScriptList[j] = checkMissionScript;
                        }
                    }
                    otherPlayerList[j] = playerSent;
                    otherPlayerAttend[j] = AttendOrNot;
                    break;
                }
                j++;
            }
        }
        everyoneIsDone += 1;
        if (everyoneIsDone == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            setDataBoxes();
        }
    }
    public void setDataBoxes()
    {
        panelTrade.SetActive(true);
        userBoxOBJ.SetActive(true);
        userBoxCardOBJ.GetComponent<MissionCardDisplay>().mission_script = userScript;
        for (int k = 0; k < PhotonNetwork.CurrentRoom.PlayerCount -1  ; k++)
        {
            Debug.Log("Open other player Trade Box");
            NickNameOtherList[k].text = main_Game_Before_Start.oppententNameTextInfo[k];
            opponentBoxOBJ[k].SetActive(true);
            opponentBoxCardOBJ[k].GetComponent<MissionCardDisplay>().mission_script = opponentScriptList[k];
            if (otherPlayerAttend[k])
            {
                otherPlayerSkippedOBJ[k].SetActive(false);
            }
            else
            {
                otherPlayerSkippedOBJ[k].SetActive(true);
            }
        }
    }
    public void RestTrades()
    {
        for (int i = 0; i < otherPlayerSkippedOBJ.Count; i++)
        {
            otherPlayerSkippedOBJ[i].SetActive(false);
        }
        for (int i = 0; i < opponentBoxOBJ.Count; i++)
        {
            opponentBoxOBJ[i].SetActive(false);
        }
    }
}
