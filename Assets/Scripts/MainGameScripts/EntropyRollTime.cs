using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntropyRollTime : MonoBehaviour
{
    [SerializeField] private GameObject rollEntropyArea,EntropyCard,rollmissionCardArea;
    [SerializeField]private Text rollingPlayerNickName, DiceNumber,
        ChancesLeft, WhichTask, WhichSkill, RollMustBeLess, TaskStatus;
    [SerializeField] private GameObject RollButton;
    [SerializeField] private DrawCharacterCard drawCharacterCard;
    [SerializeField] private DrawEntropyCard drawEntropyCard;
    private int rollchancesNumber;
    private CharCardScript currentRollerCharCardScript = null;
    [SerializeField] private Main_Game_before_start main_Game_Before_Start;
    [SerializeField] private rollTime RollTime;
    private int rollLessThanTask;
    [SerializeField] private MoneyAndPoints moneyAndPoints;
    private EntropyCardScript currentEntropyCardScript;
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
        whoRollingEntro = 20,
        rolledNumberEntro = 21,
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
        if (obj.Code == (byte)PhotonEventCode.whoRollingEntro)
        {
            object[] receiveData = (object[])obj.CustomData;
            rollchancesNumber = (int)receiveData[1];
            if ((bool)receiveData[2])
            {
                whenOtherPlayerPlay((int)receiveData[0]);
            }
            else
            {
                rollEntropyArea.SetActive(false);
                rollmissionCardArea.SetActive(true);
            }
        }
        else if (obj.Code == (byte)PhotonEventCode.rolledNumberEntro)
        {
            object[] receiveData = (object[])obj.CustomData;
            DiceNumber.text = ((int) receiveData[0]).ToString();
        }
    }
    public void startEntropyRollTurn(int entroID)
    {
        rollmissionCardArea.SetActive(false);
        rollEntropyArea.SetActive(true);
        EntropyCard.SetActive(false);
        currentEntropyCardScript = drawEntropyCard.FindWhichEntropyWithEntroID(entroID);
        if (drawCharacterCard.IsMyTurn)
        {
            object[] dataRoll = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, 1, true };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingEntro, dataRoll, AllPeople, SendOptions.SendReliable);
        }
    }
    public void whenOtherPlayerPlay(int ActorNumber)
    {
        EntropyCard.SetActive(false);
        EntropyCard.GetComponent<EntropyCardDisplay>().entropyData = currentEntropyCardScript;
        EntropyCard.GetComponent<EntropyCardDisplay>().setUpdate();
        EntropyCard.SetActive(true);
        if (drawCharacterCard.IsMyTurn)
        {
            currentRollerCharCardScript = drawCharacterCard.getMyCharScript();
            RollButton.SetActive(true);
        }
        else
        {
            currentRollerCharCardScript = drawCharacterCard.getWhichOtherCharScript(main_Game_Before_Start.findPlayerPosition(main_Game_Before_Start.FindPlayerUsingActorId(ActorNumber)));
            RollButton.SetActive(false);
        }
        WhichSkill.text = currentEntropyCardScript.RollVSWhich;
        rollLessThanTask = currentRollerCharCardScript.find_which(currentEntropyCardScript.RollVSWhich);
        if (rollLessThanTask == 0)
        {
            rollLessThanTask = currentRollerCharCardScript.find_which("Kitchen Sink");
        }
        rollingPlayerNickName.text = main_Game_Before_Start.FindPlayerUsingActorId(ActorNumber).NickName;
        TaskStatus.text = "Task : waiting";
        ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
    }
    public void clickOnRollDice()
    {
        if (rollchancesNumber != 0)
        {
            System.Random rand = new System.Random((int)DateTime.Now.Ticks);
            int x = rand.Next(0, 18);
            DiceNumber.text = x.ToString();
            object[] dataDice = new object[] { x };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.rolledNumberEntro, dataDice, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            if (x > rollLessThanTask)
            {
                TaskStatus.text = "Task : failed";
                rollchancesNumber -= 1;
                ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
                failTaskItems();
            }
            else
            {
                TaskStatus.text = "Task : passed";
                successTaskItems();
            }
        }
        else
        {
            Debug.LogError("Problem with entropy task name");
        }
    }
    public void failTaskItems()
    {
        if (currentEntropyCardScript.EntropyCardID == 12)
        {
            RollTime.enturnForEntorpyLightning();
        }
        moneyAndPoints.subPoints((byte)currentEntropyCardScript.minus_how_much_cred);
        object[] dataRoll = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, 0, false };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingEntro, dataRoll, AllPeople, SendOptions.SendReliable);
    }
    public void successTaskItems()
    {
        moneyAndPoints.addPoints((byte)currentEntropyCardScript.add_how_much_cred);
        object[] dataRoll = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, 0, false };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingEntro, dataRoll, AllPeople, SendOptions.SendReliable);
    }
}
