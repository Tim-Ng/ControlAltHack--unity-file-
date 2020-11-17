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
    [SerializeField] private GameObject rollingTimeOBJ,StartRollOBJ,doneStartButtonOBJ, Task1SkillwAmountOBJ, Task2SkillwAmountOBJ, Task3SkillwAmountOBJ;
    [SerializeField] private Text Timer;
    [SerializeField] private GameObject EndOfRollOBJ,TaskStats1,TaskStats2,TaskStats3,EndturnButtonOBJ;
    [SerializeField] private Text currentGameStatus;
    private bool[] TaskDone = { true, true, true };
    private string[] whichTasks = { null, null, null};
    private List<SkillEffector> skillEffectorsList = new List<SkillEffector>();
    private float currentTime = 0f, startingTime = 0f;
    
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
        startToRollToEnd = 24,
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
            rollchancesNumber = (int)receiveData[1];
            whichIsCurrentTask((string)receiveData[0]);
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
        else if (obj.Code == (byte)PhotonEventCode.startToRollToEnd)
        {
            object[] startRollEndOBJ = (object[])obj.CustomData;
            if ((int)startRollEndOBJ[0] == 1)
            {
                //player Start
                popupcardwindowEntro.setBoolcanAttack(true);
                setCurrentCardScript(drawCharacterCard.getWhichMissionCardScript((string)startRollEndOBJ[1]),(int)startRollEndOBJ[2]);
            }
            else if ((int)startRollEndOBJ[0] == 2)
            {
                //Player done with entropy
                popupcardwindowEntro.setBoolcanAttack(false);
                rollingTimeOBJ.SetActive(true);
                EndOfRollOBJ.SetActive(false);
                StartRollOBJ.SetActive(false);
            }
            else if ((int)startRollEndOBJ[0] == 3)
            {
                //PlayerEndTurn
                currentGameStatus.text = (string)startRollEndOBJ[1];
                if (drawCharacterCard.IsMyTurn)
                {
                    EndturnButtonOBJ.SetActive(true);
                }
                else
                {
                    EndturnButtonOBJ.SetActive(false);
                }
                rollingTimeOBJ.SetActive(false);
                EndOfRollOBJ.SetActive(true);
                StartRollOBJ.SetActive(false);
            }
            else if ((int)startRollEndOBJ[0] == 4)
            {
                Timer.text = (string)startRollEndOBJ[1];
            }
        }
    }
    public void startRollTurn()
    {
        currentMissionCardScript = null;
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
                object[] dataRollWhich = new object[] { 1,currentMissionCardScript.Mission_code,PhotonNetwork.LocalPlayer.ActorNumber};
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.startToRollToEnd, dataRollWhich, AllPeople, SendOptions.SendReliable);
            }
        }
    }
    void Update()
    {
        if (currentTime <= 0)
        {
            doneStartButtonOBJ.GetComponent<Button>().interactable = true;
            currentTime = 0;
        }
        else
        {
            currentTime -= 1 * Time.deltaTime;
            doneStartButtonOBJ.GetComponent<Button>().interactable = false;
            object[] dataRollWhich = new object[] { 4, currentTime.ToString("0") };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.startToRollToEnd, dataRollWhich, AllPeople, SendOptions.SendReliable);
        }
        
    }
    public void ClickOnStartTurn()
    {
        popupcardwindowEntro.setBoolcanPlayBackOfTricks(false);
        object[] dataRollWhich = new object[] { 2, null,null };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.startToRollToEnd, dataRollWhich, AllPeople, SendOptions.SendReliable);
        object[] dataRoll = new object[] {"Task1",rollchancesNumber };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingMission, dataRoll, AllPeople, SendOptions.SendReliable);
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
    public void removeSkillChanger(string whichSkill, string whichTurnSent,int which)
    {
        if (which == 2)
        {
            foreach (SkillEffector skillEffect in skillEffectorsList)
            {
                if (whichTurnSent == skillEffect.turnNumber)
                {
                    skillEffectorsList.Remove(skillEffect);
                }
                else if (skillEffect.turnNumber == "All")
                {
                    if (whichSkill == skillEffect.skillName)
                    {
                        skillEffectorsList.Remove(skillEffect);
                        break;
                    }
                }
            }
        }
        changeSkillChangerList();
    }
    public void checkSkillChanger()
    {
        foreach (SkillEffector skillEffect in skillEffectorsList)
        {
            if ((Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 + 1)).ToString() == skillEffect.turnNumber || "All"== skillEffect.turnNumber)
            {
                if(WhichSkill.text == skillEffect.skillName)
                {
                    string holdString = RollMustBeLess.text;
                    if (skillEffect.amount < 0)
                    {
                        RollMustBeLess.text = holdString + " - " + skillEffect.amount;
                    }
                    else
                    {
                        RollMustBeLess.text = holdString + " + " + skillEffect.amount;
                    }
                    rollLessThanTask += skillEffect.amount;
                    object[] rollMustBeLessTast = new object[] { RollMustBeLess.text };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.thereIsSkillChangerMission, rollMustBeLessTast, AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                }
            }
            if ((Mathf.RoundToInt(drawCharacterCard.TurnNumber / 2 + 1)).ToString() == skillEffect.turnNumber || "All" == skillEffect.turnNumber)
            {
                for (int i = 0; i < whichTasks.Length; i++)
                {
                    if (whichTasks[i] != null)
                    {
                        if (whichTasks[i] == skillEffect.skillName)
                        {
                            if (i == 0)
                            {
                                string hold = Task1SkillwAmountOBJ.GetComponent<Text>().text;
                                if (skillEffect.amount < 0)
                                {
                                    Task1SkillwAmountOBJ.GetComponent<Text>().text = hold + " - " + skillEffect.amount.ToString();
                                }
                                else
                                {
                                    Task1SkillwAmountOBJ.GetComponent<Text>().text = hold + " + " + skillEffect.amount.ToString();
                                }
                            }
                            else if (i == 1)
                            {
                                string hold = Task2SkillwAmountOBJ.GetComponent<Text>().text;
                                if (skillEffect.amount < 0)
                                {
                                    Task2SkillwAmountOBJ.GetComponent<Text>().text = hold + " - " + skillEffect.amount.ToString();
                                }
                                else
                                {
                                    Task2SkillwAmountOBJ.GetComponent<Text>().text = hold + " + " + skillEffect.amount.ToString();
                                }
                            }
                            else if (i == 2)
                            {
                                string hold = Task3SkillwAmountOBJ.GetComponent<Text>().text;
                                if (skillEffect.amount < 0)
                                {
                                    Task3SkillwAmountOBJ.GetComponent<Text>().text = hold + " - " + skillEffect.amount.ToString();
                                }
                                else
                                {
                                    Task3SkillwAmountOBJ.GetComponent<Text>().text = hold + " + " + skillEffect.amount.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void changeSkillChangerList()
    {
        skillChangerEliment.SetActive(true);
        foreach (Transform child in skillChangerEliment.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (SkillEffector skillEffect in skillEffectorsList)
        {
            GameObject copy = Instantiate(skillTemplate, transform.position, Quaternion.identity);
            copy.transform.SetParent(skillChangerEliment.transform, false);
            copy.GetComponent<Text>().text = "Skill Name: "+skillEffect.skillName +"\nAmount Change: +"+skillEffect.amount.ToString()+"\nIn turn:"+skillEffect.turnNumber;
        }
    }
    public void setCurrentCardScript(MissionCardScript missionCardScript,int ActorNumber)
    {
        currentMissionCardScript = missionCardScript;
        if (drawCharacterCard.IsMyTurn)
        {
            popupcardwindowEntro.setBoolcanPlayBackOfTricks(true);
            currentRollerCharCardScript = drawCharacterCard.getMyCharScript();
            RollButton.SetActive(true);
            doneStartButtonOBJ.SetActive(true);
        }
        else
        {
            popupcardwindowEntro.setBoolcanPlayBackOfTricks(false);
            currentRollerCharCardScript = drawCharacterCard.getWhichOtherCharScript(main_Game_Before_Start.findPlayerPosition(main_Game_Before_Start.FindPlayerUsingActorId(ActorNumber)));
            RollButton.SetActive(false);
            doneStartButtonOBJ.SetActive(false);
        }
        TaskThreeStatusOBJ.SetActive(false);
        TaskTwoStatusOBJ.SetActive(false);
        TaskStats2.SetActive(false);
        TaskStats3.SetActive(false);
        Task3SkillwAmountOBJ.SetActive(false);
        Task2SkillwAmountOBJ.SetActive(false);

        TaskOneStatus.text = "Task 1 : waiting";
        TaskStats1.GetComponent<Text>().text = "Task 1: waiting";
        Task1SkillwAmountOBJ.GetComponent<Text>().text = "1]" + currentMissionCardScript.skill_name_1 + "\nAmount:" + currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_1);
        whichTasks[0] = currentMissionCardScript.skill_name_1;
        popupcardwindowEntro.setBoolcanPlay(true);
        missionCardOBJ.SetActive(false);
        missionCardOBJ.GetComponent<MissionCardDisplay>().mission_script = currentMissionCardScript;
        missionCardOBJ.GetComponent<MissionCardDisplay>().setUpdate();
        missionCardOBJ.SetActive(true);
        rollingPlayerNickName.text = main_Game_Before_Start.FindPlayerUsingActorId(ActorNumber).NickName;
        if (currentMissionCardScript.hasThirdMission)
        {
            howManyTask = 3;
            TaskThreeStatusOBJ.SetActive(true);
            TaskThreeStatus.text = "Task 3 : waiting";
            TaskStats3.GetComponent<Text>().text = "Task 3: waiting";
            Task3SkillwAmountOBJ.GetComponent<Text>().text = "3]" + currentMissionCardScript.skill_name_3 + "\nAmount:" + currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_3);
            Task3SkillwAmountOBJ.SetActive(true);
            TaskStats3.SetActive(true);
            whichTasks[2] = currentMissionCardScript.skill_name_3;

            TaskTwoStatusOBJ.SetActive(true);
            TaskTwoStatus.text = "Task 2 : waiting";
            TaskStats2.GetComponent<Text>().text = "Task 2 : waiting";
            Task2SkillwAmountOBJ.GetComponent<Text>().text = "2]" + currentMissionCardScript.skill_name_2 + "\nAmount:" + currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_2);
            Task2SkillwAmountOBJ.SetActive(true);
            TaskStats2.SetActive(true);
            whichTasks[1] = currentMissionCardScript.skill_name_2;
        }
        else if (currentMissionCardScript.hasSecondMission)
        {
            howManyTask = 2;
            TaskTwoStatusOBJ.SetActive(true);
            TaskTwoStatus.text = "Task 2 : waiting";
            TaskStats2.GetComponent<Text>().text = "Task 2 : waiting";
            Task2SkillwAmountOBJ.GetComponent<Text>().text = "2]" + currentMissionCardScript.skill_name_2 + "\nAmount:" + currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_2);
            Task2SkillwAmountOBJ.SetActive(true);
            TaskStats2.SetActive(true);
            whichTasks[1] = currentMissionCardScript.skill_name_2;
        }
        else
        {
            howManyTask = 1;
        }
        rollingTimeOBJ.SetActive(false);
        EndOfRollOBJ.SetActive(false);
        StartRollOBJ.SetActive(true);
        if (drawCharacterCard.IsMyTurn)
        {
            checkSkillChanger();
            currentTime = 3;
        }
    }
    public void whichIsCurrentTask(string taskNum)
    {
        whichTaskName = taskNum;
        ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
        if (currentRollerCharCardScript != null)
        {
            if (taskNum == "Task1")
            {
                TaskOneStatus.text = "Task 1 : currently";
                TaskStats1.GetComponent<Text>().text = TaskOneStatus.text;
                WhichSkill.text = currentMissionCardScript.skill_name_1;
                rollLessThanTask = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_1);
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
                TaskStats2.GetComponent<Text>().text = TaskTwoStatus.text;
                WhichSkill.text = currentMissionCardScript.skill_name_2;
                rollLessThanTask = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_2);
                rollLessThanTask += currentMissionCardScript.second_change_hardnum;
                RollMustBeLess.text = rollLessThanTask.ToString();
                if (drawCharacterCard.IsMyTurn)
                {
                    checkSkillChanger();
                }
            }
            else if (taskNum == "Task3")
            {
                TaskThreeStatus.text = "Task 3 : currently";
                TaskStats3.GetComponent<Text>().text = TaskThreeStatus.text;
                WhichSkill.text = currentMissionCardScript.skill_name_3;
                whichTasks[2] = WhichSkill.text;
                rollLessThanTask = currentRollerCharCardScript.find_which(currentMissionCardScript.skill_name_3);
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
                    TaskStats1.GetComponent<Text>().text = TaskOneStatus.text;
                    rollchancesNumber -= 1;
                    ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
                    TaskDone[0] = false;
                }
                else
                {
                    TaskOneStatus.text = "Task 1 : passed";
                    TaskStats1.GetComponent<Text>().text = TaskOneStatus.text;
                    TaskDone[0] = true;
                }
            }
            else if (whichTaskName == "Task2")
            {
                if (x > rollLessThanTask)
                {
                    TaskTwoStatus.text = "Task 2 : failed";
                    TaskStats2.GetComponent<Text>().text = TaskTwoStatus.text;
                    rollchancesNumber -= 1;
                    ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
                    TaskDone[1] = false;
                }
                else
                {
                    TaskTwoStatus.text = "Task 2 : passed";
                    TaskStats2.GetComponent<Text>().text = TaskTwoStatus.text;
                    TaskDone[1] = true;
                }
            }
            else if (whichTaskName == "Task3")
            {
                if (x > rollLessThanTask)
                {
                    TaskThreeStatus.text = "Task 3 : failed";
                    TaskStats3.GetComponent<Text>().text = TaskThreeStatus.text;
                    rollchancesNumber -= 1;
                    ChancesLeft.text = "Reroll chances = " + rollchancesNumber;
                    TaskDone[2] = false;
                }
                else
                {
                    TaskThreeStatus.text = "Task 3 : passed";
                    TaskStats3.GetComponent<Text>().text = TaskThreeStatus.text;
                    TaskDone[2] = true;
                }
            }
            else
            {
                Debug.LogError("Problem with task name");
            }
            checkWhichIfAnyFailed();
        }
    }
    public void failTaskItems()
    {
        moneyAndPoints.subPoints((byte)currentMissionCardScript.failure_amount_hacker_cread);
    }
    public void checkWhichIfAnyFailed()
    {
        if (TaskDone[0] && TaskDone[1] && TaskDone[2])
        {
            if (whichTaskName == "Task1" && currentMissionCardScript.hasSecondMission)
            {
                object[] dataRoll = new object[] { "Task2", rollchancesNumber };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingMission, dataRoll, AllPeople, SendOptions.SendReliable);
            }
            else if (whichTaskName == "Task2")
            {
                object[] dataRoll = new object[] { "Task3", rollchancesNumber };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.whoRollingMission, dataRoll, AllPeople, SendOptions.SendReliable);
            }
            howManyTask -= 1;
            if (howManyTask == 0)
            {
                openRollDone("Passing");
            }
        }
        else
        {
            //open fail page
            openRollDone("Falied");
        }
    }
    public void openRollDone(string WinOrNot)
    { 
        object[] dataRollWhich = new object[] { 3, WinOrNot };
        PhotonNetwork.RaiseEvent((byte)PhotonEventCode.startToRollToEnd, dataRollWhich, AllPeople, SendOptions.SendReliable);
    }
    public void successTaskItems()
    {
        moneyAndPoints.addPoints((byte)currentMissionCardScript.success_amount_hacker_cread);
    }
    public void resetPlayGameAgain()
    {
        skillEffectorsList.Clear();
        restRollTime();
    }
    public void clickOnDoneTurn()
    {
        if (TaskDone[0] && TaskDone[1] && TaskDone[2])
        {
            successTaskItems();
        }
        else
        {
            failTaskItems();
        }
        currentTime = 0;
        drawCharacterCard.EndTurn();
    }
    public void restRollTime()
    {
        TaskDone[0] = true;
        TaskDone[1] = true;
        TaskDone[2] = true;
        whichTasks[0] = null;
        whichTasks[1] = null;
        whichTasks[2] = null;
        currentTime = 0;
        popupcardwindowEntro.setBoolcanAttack(false);
        popupcardwindowEntro.setBoolcanPlay(false);
        removeSkillChanger(null , (Mathf.RoundToInt((drawCharacterCard.TurnNumber / 2 + 1))-1).ToString() , 2);
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
