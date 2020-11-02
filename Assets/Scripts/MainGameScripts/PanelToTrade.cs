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
    private List<TMP_InputField> bribeOpponentList = new List<TMP_InputField>();

    [SerializeField] private Text WaitingReply1Text, WaitingReply2Text, WaitingReply3Text, WaitingReply4Text, WaitingReply5Text;
    private List<Text> waitingRelpyTextList = new List<Text>();

    private string MoneyBribedStr;
    private int MoneyBribedint;
    [SerializeField] private Button askTradeButton1, askTradeButton2, askTradeButton3, askTradeButton4, askTradeButton5;
    [SerializeField] private Text getAskText1, getAskText2, getAskText3, getAskText4, getAskText5;

    [SerializeField] private Button delineTradeButton1, delineTradeButton2, delineTradeButton3, delineTradeButton4, delineTradeButton5;
    private List<Button> delineTradeButtonList = new List<Button>();
    [SerializeField] private Button confirmTradeButton1, confirmTradeButton2, confirmTradeButton3, confirmTradeButton4, confirmTradeButton5;
    private List<Button> confirmTradeButtonList = new List<Button>();

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
                whoSentInfo((int)receiveData[0], (int)receiveData[2] , (bool)receiveData[3] , (byte)receiveData[4]);
            }
        }
    }
    private void Start()
    {
        bribeOpponentList.Add(bribeOpponent1);
        bribeOpponentList.Add(bribeOpponent2);
        bribeOpponentList.Add(bribeOpponent3);
        bribeOpponentList.Add(bribeOpponent4);
        bribeOpponentList.Add(bribeOpponent5);
        waitingRelpyTextList.Add(WaitingReply1Text);
        waitingRelpyTextList.Add(WaitingReply2Text);
        waitingRelpyTextList.Add(WaitingReply3Text);
        waitingRelpyTextList.Add(WaitingReply4Text);
        waitingRelpyTextList.Add(WaitingReply5Text);
        delineTradeButtonList.Add(delineTradeButton1);
        delineTradeButtonList.Add(delineTradeButton2);
        delineTradeButtonList.Add(delineTradeButton3);
        delineTradeButtonList.Add(delineTradeButton4);
        delineTradeButtonList.Add(delineTradeButton5);
        confirmTradeButtonList.Add(confirmTradeButton1);
        confirmTradeButtonList.Add(confirmTradeButton2);
        confirmTradeButtonList.Add(confirmTradeButton3);
        confirmTradeButtonList.Add(confirmTradeButton4);
        confirmTradeButtonList.Add(confirmTradeButton5);
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
        for (int i = 0; i < bribeOpponentList.Count; i++)
        {
            bribeOpponentList[i].text = "0";
        }
        for (int i = 0; i < amountWantedToBribeList.Count; i++)
        {
            amountWantedToBribeList[i] = 0;
        }
        for (int i = 0; i < delineTradeButtonList.Count;i++)
        {
            delineTradeButtonList[i].interactable = false;
        }
        for (int i = 0; i < confirmTradeButtonList.Count; i++)
        {
            confirmTradeButtonList[i].interactable = false;
        }
    }
    public void clickOnToTrade(int WhichClick)
    {
        MoneyBribedStr = bribeOpponentList[WhichClick].text;
        Debug.Log("Trying to bribe with " + MoneyBribedStr);
        if (int.TryParse(MoneyBribedStr, out MoneyBribedint))
        {
            if (MoneyBribedint > moneyAndPoints.getMyMoneyAmount())
            {
                waitingRelpyTextList[WhichClick].text = "You cant Bribe that much";
            }
            else if (MoneyBribedint < 0)
            {
                waitingRelpyTextList[WhichClick].text = "You cant have negative Bribe";
            }
            else
            {
                waitingRelpyTextList[WhichClick].text = "Waiting for other player responce...";
                toggleButtonToTrade(false);
                bribeOpponentList[WhichClick].interactable=false;
                object[] data = new object[] {PhotonNetwork.LocalPlayer.ActorNumber , otherPlayerList[0].ActorNumber , MoneyBribedint , drawMissionCard.getCurrentMissionScript().Newb_job, moneyAndPoints.getMyPoints()};
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendToAsk, data, AllPeople, SendOptions.SendReliable);
            }
        }
        else
        {
            waitingRelpyTextList[WhichClick].text = "This is not int " + MoneyBribedStr;
        }
    }
    public void clickOnToTradeOpponent1()
    {
        clickOnToTrade(0);
    }
    public void clickOnToTradeOpponent2()
    {
        clickOnToTrade(1);
    }
    public void clickOnToTradeOpponent3()
    {
        clickOnToTrade(2);
    }
    public void clickOnToTradeOpponent4()
    {
        clickOnToTrade(3);
    }
    public void clickOnToTradeOpponent5()
    {
        clickOnToTrade(4);
    }
    public void toggleButtonToTrade(bool onOrOff)
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            allotherTradeButtonList[i].interactable = onOrOff;
        }
    }
    public void whoSentInfo(int SenderID, int Amount,bool NewbJob,byte OtherPlayerPoints)
    {
        int BoxNum = 0;
        foreach (Player CheckPlayer in PhotonNetwork.PlayerListOthers)
        {
            if (CheckPlayer.ActorNumber == SenderID)
            {
                if (NewbJob && ((int)OtherPlayerPoints > (int)moneyAndPoints.getMyPoints()))
                {
                    getAskTextList[BoxNum].text = "A Got newb job,and you've less points so...";
                    //swaped card 
                }
                else
                {
                    amountWantedToBribeList[BoxNum] = Amount;
                    delineTradeButtonList[BoxNum].interactable = true;
                    confirmTradeButtonList[BoxNum].interactable = true;
                    getAskTextList[BoxNum].text = "This player " + CheckPlayer.NickName + " wants to trade with the amount of $" + Amount;
                }
            }
            BoxNum++;
        }
    }
    public void confirmButton(int which)
    {
        Debug.Log("Confirm Button was click going to exchange");
    }
}
