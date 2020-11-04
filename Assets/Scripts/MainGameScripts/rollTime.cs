﻿using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class rollTime : MonoBehaviour
{
    [SerializeField] private GameObject rollMissionTime;
    [SerializeField] private GameObject missionCardOBJ;
    [SerializeField] private Text rollingPlayerNickName, DiceNumber,
        ChancesLeft, WhichTask, WhichSkill, RollMustBeLess;
    [SerializeField] private GameObject RollButton;
    [SerializeField] private DrawCharacterCard drawCharacterCard;
    [SerializeField] private popupcardwindowEntropy popupcardwindowEntro;
    private int rollchancesNumber ;
    [SerializeField] private PanelToTrade panelToTrade;
    private CharCardScript currentRollerCharCardScript = null;
    [SerializeField] private Main_Game_before_start main_Game_Before_Start;
    private int rollMoreThanTask1, rollMoreThanTask2, rollMoreThanTask3;
    private string whichTaskName;
    [SerializeField] private MoneyAndPoints moneyAndPoints;
    private MissionCardScript currentMissionCardScript;
    private int howManyTask;
    [SerializeField] private Text TaskOneStatus,TaskTwoStatus,TaskThreeStatus;
    [SerializeField] private GameObject TaskThreeStatusOBJ;

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
        rolledNumber = 16,
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
            rollchancesNumber = (int)receiveData[3];
            whenOtherPlayerPlay(drawCharacterCard.getWhichMissionCardScript((string)receiveData[0]), (int)receiveData[2]);
            whichIsCurrentTask(drawCharacterCard.getWhichMissionCardScript((string)receiveData[0]), (string) receiveData[1]);
        }
    }
    public void startRollTurn()
    {
        missionCardOBJ.SetActive(false);
        if (drawCharacterCard.TurnNumber % 2 != 0)
        {
            Debug.LogError("Problem its not Roll Time");
        }
        else
        {
            rollMissionTime.SetActive(true);
            if (drawCharacterCard.IsMyTurn)
            {
                if (!panelToTrade.getIfIAttend())
                {
                    rollchancesNumber = 2;
                }
                else
                {
                    rollchancesNumber = 1;
                }
                currentMissionCardScript = drawCharacterCard.getCurrentMissionScript();
                object[] dataRoll = new object[] { currentMissionCardScript.Mission_code , "Task1" , PhotonNetwork.LocalPlayer.ActorNumber,rollchancesNumber };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRolling, dataRoll, AllPeople, SendOptions.SendReliable);
            }
        }
    }
    public void whenOtherPlayerPlay(MissionCardScript missionCardScript,int ActorNumber)
    {
        TaskThreeStatusOBJ.SetActive(false);
        TaskOneStatus.text = "Task 1 : waiting";
        TaskTwoStatus.text = "Task 2 : waiting";
        currentMissionCardScript = missionCardScript;
        popupcardwindowEntro.setBoolcanPlay(true);
        missionCardOBJ.SetActive(true);
        missionCardOBJ.GetComponent<MissionCardDisplay>().mission_script = currentMissionCardScript;
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
        rollingPlayerNickName.text = main_Game_Before_Start.FindPlayerUsingActorId(ActorNumber).NickName;
        if (currentMissionCardScript.hasThirdMission)
        {
            howManyTask = 3;
            TaskThreeStatusOBJ.SetActive(true);
            TaskThreeStatus.text = "Task 1 : waiting";
        }
        else
        {
            howManyTask = 2;
        }
    }
    public void whichIsCurrentTask(MissionCardScript missionCardScript,string taskNum)
    {
        whichTaskName = taskNum;
        ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
        if (taskNum == "Task1")
        {
            TaskThreeStatus.text = "Task 1 : currently";
            WhichSkill.text = currentMissionCardScript.skill_name_1;
            rollMoreThanTask1 = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_1);
            if (rollMoreThanTask1 == 0)
            {
                rollMoreThanTask1 = currentRollerCharCardScript.find_which("Kitchen Sink");
            }
            rollMoreThanTask1 += currentMissionCardScript.fist_change_hardnum;
            WhichTask.text = "First Task:";
            RollMustBeLess.text = rollMoreThanTask1.ToString();
        }
        else if (taskNum == "Task2")
        {
            WhichSkill.text = currentMissionCardScript.skill_name_2;
            rollMoreThanTask2 = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_2);
            if (rollMoreThanTask2 == 0)
            {
                rollMoreThanTask2 = currentRollerCharCardScript.find_which("Kitchen Sink");
            }
            rollMoreThanTask2 += currentMissionCardScript.second_change_hardnum;
            WhichTask.text = "Second Task:";
            RollMustBeLess.text = rollMoreThanTask2.ToString();
        }
        else if (taskNum == "Task3")
        {
            WhichSkill.text = currentMissionCardScript.skill_name_3;
            rollMoreThanTask3 = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_3);
            if (rollMoreThanTask3 == 0)
            {
                rollMoreThanTask3 = currentRollerCharCardScript.find_which("Kitchen Sink");
            }
            rollMoreThanTask3 += currentMissionCardScript.third_change_hardnum;
            WhichTask.text = "Third Task:";
            RollMustBeLess.text = rollMoreThanTask3.ToString();
        }
    }
    public void clickOnRollDice()
    {
        if (rollchancesNumber != 0)
        {
            System.Random rand = new System.Random((int)DateTime.Now.Ticks);
            int x = rand.Next(0, 18);
            DiceNumber.text = x.ToString();
            if (whichTaskName == "Task1")
            {
                if (x > rollMoreThanTask1)
                {
                    TaskOneStatus.text = "Task 1 : failed";
                    rollchancesNumber -= 1;
                    ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
                    failTask();
                }
                else
                {
                    TaskOneStatus.text = "Task 1 : passed";
                    successTask();
                }
            }
        }
    }
    public void failTask()
    {
        if (currentRollerCharCardScript.character_code == 12)
        {

        }
        else if (rollchancesNumber == 0)
        {
            drawCharacterCard.EndTurn();
            failTaskItems();
        }
    }
    public void failTaskItems()
    {
        moneyAndPoints.addPoints((byte)currentMissionCardScript.failure_amount_hacker_cread);
    }
    public void successTask()
    {
        if (howManyTask > 0)
        {
            howManyTask -= 1;
            object[] dataRoll = new object[] { currentMissionCardScript.Mission_code, "Task2", rollchancesNumber };
        }
        else
        {
            successTaskItems();
        }
    }
    public void successTaskItems()
    {
        moneyAndPoints.addMyMoney(currentMissionCardScript.success_amount_hacker_cread);
        drawCharacterCard.EndTurn();
    }
}
