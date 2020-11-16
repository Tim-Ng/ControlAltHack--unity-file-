using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public struct SkillEffector
{
    public string skillName;
    public int amount;
    public string turnNumber;
    public SkillEffector(string SkillName, int Amount, string TurnNumber)
    {
        skillName = SkillName;
        amount = Amount;
        turnNumber = TurnNumber;
    }
}

public class rollTime : MonoBehaviour
{
    [SerializeField] private GameObject rollMissionTime;
    [SerializeField] private GameObject missionCardOBJ;
    [SerializeField] private GameObject skillChangerEliment,skillTemplate;
    [SerializeField] private Text rollingPlayerNickName, DiceNumber,
        ChancesLeft, WhichTask, WhichSkill, RollMustBeLess;
    [SerializeField] private GameObject RollButton;
    [SerializeField] private DrawCharacterCard drawCharacterCard;
    [SerializeField] private popupcardwindowEntropy popupcardwindowEntro;
    private int rollchancesNumber ;
    [SerializeField] private PanelToTrade panelToTrade;
    private CharCardScript currentRollerCharCardScript = null;
    [SerializeField] private Main_Game_before_start main_Game_Before_Start;
    private int rollLessThanTask;
    private string whichTaskName;
    [SerializeField] private MoneyAndPoints moneyAndPoints;
    private MissionCardScript currentMissionCardScript;
    private int howManyTask;
    [SerializeField] private Text TaskOneStatus,TaskTwoStatus,TaskThreeStatus;
    [SerializeField] private GameObject TaskTwoStatusOBJ, TaskThreeStatusOBJ;
    [SerializeField] private GameObject userMissionCardArea;
    [SerializeField] private popupcardwindowMission popupcardwindowMissionScript;
    private List<SkillEffector> skillEffectorsList = new List<SkillEffector>();
    
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
        whoRollingMission = 15,
        rolledNumberMission = 16,
        thereIsSkillChangerMission = 19,
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
        if (obj.Code == (byte)PhotonEventCode.whoRollingMission)
        {
            object[] receiveData = (object[])obj.CustomData;
            rollchancesNumber = (int)receiveData[3];
            if ((bool)receiveData[4])
            {
                whenOtherPlayerPlay(drawCharacterCard.getWhichMissionCardScript((string)receiveData[0]), (int)receiveData[2]);
                whichIsCurrentTask(drawCharacterCard.getWhichMissionCardScript((string)receiveData[0]), (string)receiveData[1]);
            }
            else
            {
                whichIsCurrentTask(drawCharacterCard.getWhichMissionCardScript((string)receiveData[0]), (string)receiveData[1]);
            }
        }
        else if (obj.Code == (byte)PhotonEventCode.rolledNumberMission)
        {
            object[] rollData = (object[])obj.CustomData;
            int rolledNumber = (int)rollData[0];
            DiceNumber.text = rolledNumber.ToString();
        }
        else if (obj.Code == (byte)PhotonEventCode.thereIsSkillChangerMission)
        {
            object[] skillChanger = (object[])obj.CustomData;
            RollMustBeLess.text =(string)skillChanger[0];
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
                if (!panelToTrade.getIfIAttend() || panelToTrade.ifOnlyYouAttend())
                {
                    rollchancesNumber = 2;
                }
                else
                {
                    rollchancesNumber = 1;
                }
                if (drawCharacterCard.getMyCharScript().character_code == 13)
                {
                    rollchancesNumber += 1;
                }
                currentMissionCardScript = drawCharacterCard.getCurrentMissionScript();
                object[] dataRoll = new object[] { currentMissionCardScript.Mission_code , "Task1" , PhotonNetwork.LocalPlayer.ActorNumber,rollchancesNumber, true };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingMission, dataRoll, AllPeople, SendOptions.SendReliable);
            }
        }
    }
    public void whenOtherPlayerPlay(MissionCardScript missionCardScript,int ActorNumber)
    {
        TaskThreeStatusOBJ.SetActive(false);
        TaskTwoStatusOBJ.SetActive(false);
        TaskOneStatus.text = "Task 1 : waiting";
        currentMissionCardScript = missionCardScript;
        popupcardwindowEntro.setBoolcanPlay(true);
        missionCardOBJ.SetActive(false);
        missionCardOBJ.GetComponent<MissionCardDisplay>().mission_script = currentMissionCardScript;
        missionCardOBJ.GetComponent<MissionCardDisplay>().setUpdate();
        missionCardOBJ.SetActive(true);
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
            TaskThreeStatus.text = "Task 3 : waiting";
            howManyTask = 2;
            TaskTwoStatusOBJ.SetActive(true);
            TaskTwoStatus.text = "Task 2 : waiting";
        }
        else if (currentMissionCardScript.hasSecondMission)
        {
            howManyTask = 2;
            TaskTwoStatusOBJ.SetActive(true);
            TaskTwoStatus.text = "Task 2 : waiting";
        }
        else
        {
            howManyTask = 1;
        }
    }
    public void addSkillChanger(string whichSkill, int Amount, string whichTurn)
    {
        skillEffectorsList.Add(new SkillEffector(whichSkill, Amount, whichTurn));
        if (drawCharacterCard.IsMyTurn)
        {
            checkSkillChanger();
            changeSkillChangerList();
        }
    }
    public void checkSkillChanger()
    {
        foreach (SkillEffector skillEffect in skillEffectorsList)
        {
            if ((Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 + 1)).ToString() == skillEffect.turnNumber || "All"== skillEffect.turnNumber)
            {
                if(WhichSkill.text == skillEffect.skillName)
                {
                    string holdString;
                    holdString = RollMustBeLess.text;
                    RollMustBeLess.text = holdString + " + " + skillEffect.amount;
                    rollLessThanTask += skillEffect.amount;
                    object[] rollMustBeLessTast = new object[] { RollMustBeLess.text };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.thereIsSkillChangerMission, rollMustBeLessTast, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                }
            }
        }
    }
    public void changeSkillChangerList()
    {
        foreach (SkillEffector skillEffect in skillEffectorsList)
        {
            var copy = Instantiate(skillTemplate);
            copy.transform.parent = skillChangerEliment.transform;
            copy.GetComponent<Text>().text = "Skill Name: "+skillEffect.skillName +"\nAmount Change: +"+skillEffect.amount.ToString()+"\nIn turn:"+skillEffect.turnNumber;
        }
    }
    public void whichIsCurrentTask(MissionCardScript missionCardScript,string taskNum)
    {
        whichTaskName = taskNum;
        ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
        if (currentRollerCharCardScript != null)
        {
            if (taskNum == "Task1")
            {
                TaskOneStatus.text = "Task 1 : currently";
                WhichSkill.text = currentMissionCardScript.skill_name_1;
                rollLessThanTask = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_1);
                if (rollLessThanTask == 0)
                {
                    rollLessThanTask = currentRollerCharCardScript.find_which("Kitchen Sink");
                }
                rollLessThanTask += currentMissionCardScript.fist_change_hardnum;
                WhichTask.text = "First Task:";
                RollMustBeLess.text = rollLessThanTask.ToString();
                if (drawCharacterCard.IsMyTurn)
                {
                    checkSkillChanger();
                }
            }
            else if (taskNum == "Task2")
            {
                TaskTwoStatus.text = "Task 2 : currently";
                WhichSkill.text = currentMissionCardScript.skill_name_2;
                rollLessThanTask = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_2);
                if (rollLessThanTask == 0)
                {
                    rollLessThanTask = currentRollerCharCardScript.find_which("Kitchen Sink");
                }
                rollLessThanTask += currentMissionCardScript.second_change_hardnum;
                WhichTask.text = "Second Task:";
                RollMustBeLess.text = rollLessThanTask.ToString();
                if (drawCharacterCard.IsMyTurn)
                {
                    checkSkillChanger();
                }
            }
            else if (taskNum == "Task3")
            {
                TaskThreeStatus.text = "Task 3 : currently";
                WhichSkill.text = currentMissionCardScript.skill_name_3;
                rollLessThanTask = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_3);
                if (rollLessThanTask == 0)
                {
                    rollLessThanTask = currentRollerCharCardScript.find_which("Kitchen Sink");
                }
                rollLessThanTask += currentMissionCardScript.third_change_hardnum;
                WhichTask.text = "Third Task:";
                RollMustBeLess.text = rollLessThanTask.ToString();
                if (drawCharacterCard.IsMyTurn)
                {
                    checkSkillChanger();
                }
            }
        }
        
    }
    public void clickOnRollDice()
    {
        if (rollchancesNumber != 0)
        {
            System.Random rand = new System.Random((int)DateTime.Now.Ticks);
            int x = rand.Next(0, 18);
            DiceNumber.text = x.ToString();
            object[] dataDice = new object[] { x };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.rolledNumberMission, dataDice, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            if (whichTaskName == "Task1")
            {
                if (x > rollLessThanTask)
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
            else if (whichTaskName == "Task2")
            {
                if (x > rollLessThanTask)
                {
                    TaskTwoStatus.text = "Task 2 : failed";
                    rollchancesNumber -= 1;
                    ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
                    failTask();
                }
                else
                {
                    TaskTwoStatus.text = "Task 2 : passed";
                    successTask();
                }
            }
            else if (whichTaskName == "Task3")
            {
                if (x > rollLessThanTask)
                {
                    TaskThreeStatus.text = "Task 3 : failed";
                    rollchancesNumber -= 1;
                    ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
                    failTask();
                }
                else
                {
                    TaskThreeStatus.text = "Task 3 : passed";
                    successTask();
                }
            }
            else
            {
                Debug.LogError("Problem with task name");
            }
        }
    }
    public void failTask()
    {
        if (currentRollerCharCardScript.character_code == 12)
        {
            failTaskItems();
        }
        else if (rollchancesNumber == 0)
        {
            failTaskItems();
        }
    }
    public void failTaskItems()
    {
        moneyAndPoints.subPoints((byte)currentMissionCardScript.failure_amount_hacker_cread);
        drawCharacterCard.EndTurn();
    }
    public void successTask()
    {
        if (whichTaskName == "Task1" && currentMissionCardScript.hasSecondMission)
        {
            object[] dataRoll = new object[] { currentMissionCardScript.Mission_code, "Task2", PhotonNetwork.LocalPlayer.ActorNumber, rollchancesNumber, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingMission, dataRoll, AllPeople, SendOptions.SendReliable);
        }
        else if (whichTaskName == "Task2")
        {
            object[] dataRoll = new object[] { currentMissionCardScript.Mission_code, "Task3", PhotonNetwork.LocalPlayer.ActorNumber, rollchancesNumber, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingMission, dataRoll, AllPeople, SendOptions.SendReliable);
        }
        howManyTask -= 1;
        if (howManyTask == 0)
        {
            successTaskItems();
        }
    }
    public void successTaskItems()
    {
        moneyAndPoints.addPoints((byte)currentMissionCardScript.success_amount_hacker_cread);
        drawCharacterCard.EndTurn();
    }
    public void resetPlayGameAgain()
    {
        skillEffectorsList.Clear();
        restRollTime();
    }
    public void restRollTime()
    {
        rollMissionTime.SetActive(false);
        panelToTrade.RestTrades();
        popupcardwindowMissionScript.setONLYLOOK(false);
        popupcardwindowMissionScript.setcardselectedFalse();
        panelToTrade.restAttendance();
        foreach (Transform child in userMissionCardArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

}
