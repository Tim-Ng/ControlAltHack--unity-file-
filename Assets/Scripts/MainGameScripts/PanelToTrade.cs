using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
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
    [SerializeField] private TMP_InputField bribeOpponent1, bribeOpponent2, bribeOpponent3, bribeOpponent4, bribeOpponent5;
    [SerializeField] private Text WaitingReply1Text, WaitingReply2Text, WaitingReply3Text, WaitingReply4Text, WaitingReply5Text;
    private string MoneyBribedStr;
    private int MoneyBribedint;
    [SerializeField] private Button askTradeButton1, askTradeButton2, askTradeButton3, askTradeButton4, askTradeButton5;
    [SerializeField] private Text getAskText1, getAskText2, getAskText3, getAskText4, getAskText5;
    private List<Text> getAskTextList = new List<Text>();
    private int amountWantedToBribe1, amountWantedToBribe2, amountWantedToBribe3, amountWantedToBribe4, amountWantedToBribe5;
    private List<int> amountWantedToBribeList = new List<int>();
    private List<Button> allotherTradeButtonList = new List<Button>();
    [SerializeField] private MoneyAndPoints moneyAndPoints;
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
        sendToAsk = 12,
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
        else if (obj.Code == (byte)PhotonEventCode.sendToAsk)
        {
            object[] receiveData = (object[])obj.CustomData;
            if (PhotonNetwork.LocalPlayer.ActorNumber == (int)receiveData[1])
            {
                whoSentInfo((int)receiveData[0], (int)receiveData[2]);
            }
        }
    }
    private void Start()
    {
        amountWantedToBribeList.Add(amountWantedToBribe1);
        amountWantedToBribeList.Add(amountWantedToBribe2);
        amountWantedToBribeList.Add(amountWantedToBribe3);
        amountWantedToBribeList.Add(amountWantedToBribe4);
        amountWantedToBribeList.Add(amountWantedToBribe5);
        getAskTextList.Add(getAskText1);
        getAskTextList.Add(getAskText2);
        getAskTextList.Add(getAskText3);
        getAskTextList.Add(getAskText4);
        getAskTextList.Add(getAskText5);
        allotherTradeButtonList.Add(askTradeButton1);
        allotherTradeButtonList.Add(askTradeButton2);
        allotherTradeButtonList.Add(askTradeButton3);
        allotherTradeButtonList.Add(askTradeButton4);
        allotherTradeButtonList.Add(askTradeButton5);
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
        popupcardwindowMissionScript.setONLYLOOK(true);
        popupcardwindowMissionScript.closePopup();
        sendEveryOneDone(true);
    }
    public void clickOnNotAttend()
    {
        RestTrades();
        popupcardwindowMissionScript.setONLYLOOK(true);
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
        bribeOpponent1.text = "0";
        bribeOpponent2.text = "0";
        bribeOpponent3.text = "0"; 
        bribeOpponent4.text = "0";
        bribeOpponent5.text = "0";
        for (int i = 0; i < amountWantedToBribeList.Count; i++)
        {
            amountWantedToBribeList[i] = 0;
        }
    }
    public void clickOnToTradeOpponent1()
    {
        MoneyBribedStr = bribeOpponent1.text;
        Debug.Log("Trying to bribe with " + MoneyBribedStr);
        if (int.TryParse(MoneyBribedStr, out MoneyBribedint))
        {
            if (MoneyBribedint > moneyAndPoints.getMyMoneyAmount())
            {
                WaitingReply1Text.text = "You cant Bribe that much";
            }
            else if (MoneyBribedint < 0)
            {
                WaitingReply1Text.text = "You cant have negative Bribe";
            }
            else
            {
                WaitingReply1Text.text = "Waiting for other player responce...";
                toggleButtonToTrade(false);
                bribeOpponent1.interactable=false;
                object[] data = new object[] {PhotonNetwork.LocalPlayer.ActorNumber , otherPlayerList[0].ActorNumber , MoneyBribedint };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendToAsk, data, AllPeople, SendOptions.SendReliable);
            }
        }
        else
        {
            WaitingReply1Text.text = "This is not int " + MoneyBribedStr;
        }

    }
    public void clickOnToTradeOpponent2()
    {

    }
    public void clickOnToTradeOpponent3()
    {

    }
    public void clickOnToTradeOpponent4()
    {

    }
    public void clickOnToTradeOpponent5()
    {

    }
    public void toggleButtonToTrade(bool onOrOff)
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            allotherTradeButtonList[i].interactable = onOrOff;
        }
    }
    public void whoSentInfo(int SenderID, int Amount)
    {
        int BoxNum = 0;
        foreach (Player CheckPlayer in PhotonNetwork.PlayerListOthers)
        {
            if (CheckPlayer.ActorNumber == SenderID)
            {
                amountWantedToBribeList[BoxNum] = Amount;
                getAskTextList[BoxNum].text = "This player " + CheckPlayer.NickName + " wants to trade with the amount of $" + Amount;
            }
            BoxNum++;
        }
    }
}
