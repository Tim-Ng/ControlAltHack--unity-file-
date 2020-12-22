using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserAreas;
using DrawCards;
using Photon.Realtime;
using UnityEngine.UI;
using main;
using ExitGames.Client.Photon;
using Photon.Pun;
using System;

namespace rollmissions {
    public class jobInfos
    {
        public int position; // 1 , 2 or 3
        public AllJobs skillName;
        public int amount;
        public bool passingOrNot { get; set; }
        public jobInfos(int Position, AllJobs SkillName,int Amount,bool PassingOrNOt)
        {
            position = Position;
            skillName = SkillName;
            amount = Amount;
            passingOrNot = PassingOrNOt;
        }
        public void addSkillAmount(int howMuch) => amount += howMuch;
        public void subSkillAmount(int howMuch) => amount -= howMuch;
    }
    public struct SkillEffector
    {
        public AllJobs skillName;
        public int amount;
        public int turnNumber;
        public SkillEffector(AllJobs SkillName, int Amount, int TurnNumber)
        {
            skillName = SkillName;
            amount = Amount;
            turnNumber = TurnNumber;
        }
    }
    public class rollingMissionControl : MonoBehaviour
    {
        [SerializeField] private DuringMissionRollController missionRollController = null;
        [SerializeField] private UserAreaControlers userArea = null;
        [SerializeField] private missionPopup popup= null;
        [SerializeField] private GameObject MissionCard = null, RollingMissionOBJ = null;
        [SerializeField] private EventHandeler EventManger = null;
        [SerializeField] private TurnManager turnManager = null;
        private List<SkillEffector> skillEffectorsList = new List<SkillEffector>();
        private List<jobInfos> JobInfoList = new List<jobInfos>();
        private MissionCardScript currentCard = null ;
        private bool CurrentMissionStatus = true;
        private MissionCardScript setGetCurrentCard
        {
            get { return currentCard; }
            set 
            { 
                currentCard = value;
                MissionCard.GetComponent<Image>().sprite = currentCard.artwork_front_info;
            }
        }
        private int numberOfChances=0,whoTurn=0,currentTask = 0;
        private int setgetnumberOfChances
        {
            get { return numberOfChances; }
            set
            {
                numberOfChances = value;
                missionRollController.setNumberOfChances = numberOfChances.ToString();
                object[] numberOfChanceData = new object[] { numberOfChances.ToString() };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setNumberOfChances, numberOfChanceData, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        private float currentTime = 0f;
        void Update()
        {
            if (currentTime <= 0)
            {
                missionRollController.setStartMissionButton = true;
                currentTime = 0;
            }
            else
            {
                currentTime -= 1 * Time.deltaTime;
                missionRollController.setStartMissionButton = false;
                object[] timer = new object[] { currentTime.ToString("0") };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setTimerForRoll, timer, EventManger.AllPeople, SendOptions.SendReliable);
            }
        }
        public void setTimer(string time) => missionRollController.setTimerText = time;
        public void setRollTimeIsMyTurn()
        {
            switchStage(1);
            missionRollController.setActiveStartMissionButton = true;
            missionRollController.setStartMissionButton = false;
            setGetCurrentCard = userArea.users[0].missionScript;
            missionRollController.setActivetask2 = setGetCurrentCard.hasSecondMission;
            missionRollController.setActivetask3 = setGetCurrentCard.hasThirdMission;
            JobInfoList.Clear();

            JobInfoList.Add(new jobInfos(1, setGetCurrentCard.skill_name_1, userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_1),true));
            missionRollController.settask1beforeText = "1] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[0].skillName) + "\n   Amonunt: " + JobInfoList[0].amount;
            if (setGetCurrentCard.fist_change_hardnum != 0)
            {
                if (setGetCurrentCard.fist_change_hardnum > 0)
                    missionRollController.settask1beforeText += "+" +setGetCurrentCard.fist_change_hardnum;

                else
                    missionRollController.settask1beforeText += setGetCurrentCard.fist_change_hardnum;
                JobInfoList[0].addSkillAmount(setGetCurrentCard.fist_change_hardnum);
            }
            if (setGetCurrentCard.hasSecondMission)
            {
                JobInfoList.Add(new jobInfos(2, setGetCurrentCard.skill_name_2, userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_2),true));
                missionRollController.settask2beforeText = "2] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[1].skillName) + "\n   Amonunt: " + JobInfoList[1].amount;
                if (setGetCurrentCard.second_change_hardnum != 0)
                {
                    if (setGetCurrentCard.second_change_hardnum > 0)
                        missionRollController.settask2beforeText += "+" +setGetCurrentCard.second_change_hardnum;

                    else
                        missionRollController.settask2beforeText += setGetCurrentCard.second_change_hardnum;
                    JobInfoList[1].addSkillAmount(setGetCurrentCard.second_change_hardnum);
                }
            }
            if (setGetCurrentCard.hasThirdMission)
            {
                JobInfoList.Add(new jobInfos(3, setGetCurrentCard.skill_name_3, userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_3),true));
                missionRollController.settask3beforeText = "3] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[2].skillName) + "\n   Amonunt: " + JobInfoList[2].amount;
                if (setGetCurrentCard.third_change_hardnum != 0)
                {
                    if (setGetCurrentCard.third_change_hardnum > 0)
                        missionRollController.settask3beforeText += "+" + setGetCurrentCard.third_change_hardnum;

                    else
                        missionRollController.settask3beforeText += setGetCurrentCard.third_change_hardnum;
                    JobInfoList[2].addSkillAmount(setGetCurrentCard.third_change_hardnum);
                }
            }
            CurrentMissionStatus = true;
            object[] textForBeforeTask = new object[] { missionRollController.settask1beforeText, setGetCurrentCard.hasSecondMission, missionRollController.settask2beforeText, setGetCurrentCard.hasThirdMission, missionRollController.settask3beforeText };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.textForTextBeforeRoll, textForBeforeTask, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            checkSkillEffector();
            whoTurn = 0;
            numberOfChances = 0;
            if (popup.AttendingOrNot)
            {
                setgetnumberOfChances = 1;
            }
            else
            {
                setgetnumberOfChances = 2;
            }
            currentTime = 3;
            missionRollController.setgetCurrentRollerName = PhotonNetwork.LocalPlayer.NickName;
            RollingMissionOBJ.SetActive(true);
            object[] playerRollingdata = new object[] {PhotonNetwork.LocalPlayer , userArea.users[0].missionScript.Mission_code};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendWhoRolling, playerRollingdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveSetOtherPlayerRoll(Player whichPlayer, int MissionCardCode)
        {
            switchStage(1);
            whoTurn = userArea.findPlayerPosition(whichPlayer);
            missionRollController.setgetCurrentRollerName = whichPlayer.NickName;
            setGetCurrentCard = missionCardDeck.cardDeck[MissionCardCode - 1];
            missionRollController.setActiveStartMissionButton = false;
            RollingMissionOBJ.SetActive(true);
        }
        public void onReceiveTextBeforeRollTasks(string task1,bool hasTask2,string task2,bool hasTask3, string task3)
        {
            missionRollController.settask1beforeText = task1;
            missionRollController.setActivetask2= hasTask2;
            missionRollController.setActivetask3 = hasTask3;
            if (hasTask2)
                missionRollController.settask2beforeText = task2;
            if (hasTask3)
                missionRollController.settask3beforeText = task3;
        }
        private void switchStage(int whichStage)
        {
            if (whichStage == 1)
            {
                missionRollController.setbeforeMission = true;
                missionRollController.setDuringMission = false;
                missionRollController.setAfterMission = false;
            }
            else if (whichStage == 2)
            {
                missionRollController.setbeforeMission = false;
                missionRollController.setDuringMission = true;
                missionRollController.setAfterMission = false;
            }
            else
            {
                missionRollController.setbeforeMission = false;
                missionRollController.setDuringMission = false;
                missionRollController.setAfterMission = true;
            }
        }
        public void OnClickOnMissionCard() => popup.clickOnCard(setGetCurrentCard, whoTurn, false);
        public void onClickOnStartButton()
        {
            switchStage(2);
            onReceiveDuringTaskProgressText(1, "In prograss ");
            onReceiveDuringTaskProgressText(2, "waiting");
            onReceiveDuringTaskProgressText(3, "waiting");
            onReceiveTaskStatusText(1, false);
            onReceiveTaskStatusText(2, false);
            onReceiveTaskStatusText(3, false);
            missionRollController.setActiveRollButton = true;
            currentTask = 0;
            setCurrentTask();
            object[] playerDuringdata = new object[] { };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setToDuringRoll, playerDuringdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveChangeToDuringTask()
        {
            switchStage(2);
            setDuringProgessText(1, "In prograss ");
            setDuringProgessText(2, "waiting");
            setDuringProgessText(3, "waiting");
            onReceiveTaskStatusText(1, false);
            onReceiveTaskStatusText(2, false);
            onReceiveTaskStatusText(3, false);
            missionRollController.setActiveRollButton = false;
        }
        public void setCurrentTask()
        {
            missionRollController.setCurrentText = GetStringOfTask.get_string_of_job(JobInfoList[currentTask].skillName);
            missionRollController.setWhichIsCurrentTask = (currentTask + 1).ToString();
            missionRollController.setrollGoalText = JobInfoList[currentTask].amount.ToString();
            object[] playerDuringtextdata = new object[] { GetStringOfTask.get_string_of_job(JobInfoList[currentTask].skillName), (currentTask + 1).ToString(), JobInfoList[currentTask].amount.ToString()};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setDuringRollText, playerDuringtextdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveControllText(string currentTaskName,string whichIsCurrentTask,string mustbemorethan)
        {
            missionRollController.setCurrentText = currentTaskName;
            missionRollController.setWhichIsCurrentTask = whichIsCurrentTask;
            missionRollController.setrollGoalText = mustbemorethan;
            
        }
        public void onReceiveNumberOfChances(string value)
        {
            missionRollController.setNumberOfChances = value;
        }
        public void setDuringProgessText(int which, string text)
        {
            if (which == 1)
                missionRollController.settask1duringText = text;
            else if (which == 2)
                missionRollController.settask2duringText = text;
            else if (which == 3)
                missionRollController.settask3duringText = text;
            object[] setProcessTextdata = new object[] { which, text };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setProcessText, setProcessTextdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveDuringTaskProgressText(int which ,string text)
        {
            if (which == 1)
                missionRollController.settask1duringText = text;
            else if (which == 2)
                missionRollController.settask2duringText = text;
            else if (which == 3)
                missionRollController.settask3duringText = text;
        }
        public void setDuringStatusText(int which, bool text)
        {
            if (which == 1)
                missionRollController.setTaskOneStatus = text;
            else if (which == 2)
                missionRollController.setTaskTwoStatusStatus = text;
            else if (which == 3)
                missionRollController.setTaskThreeStatusStatus = text;
            object[] setStatusTextdata = new object[] { which, text };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setStatusText, setStatusTextdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveTaskStatusText(int which, bool text)
        {
            if (which == 1)
                missionRollController.setTaskOneStatus = text;
            else if (which == 2)
                missionRollController.setTaskTwoStatusStatus = text;
            else if (which == 3)
                missionRollController.setTaskThreeStatusStatus = text;
        }
        public void upDateSkillAmount(int which,int amount)
        {
            if (which == 1)
            {
                if (amount > 0)
                    missionRollController.settask1beforeText += "+" + amount;

                else
                    missionRollController.settask1beforeText += amount;
                JobInfoList[0].addSkillAmount(amount);
            }
            else if (which == 2)
            {
                if (amount > 0)
                    missionRollController.settask2beforeText += "+" + amount;

                else
                    missionRollController.settask2beforeText += amount;
                JobInfoList[1].addSkillAmount(amount);
            }
            else if (which == 3)
            {
                if (amount > 0)
                    missionRollController.settask3beforeText += "+" + amount;

                else
                    missionRollController.settask3beforeText += amount;
                JobInfoList[2].addSkillAmount(amount);
            }
        }
        public void checkSkillEffector()
        {
            foreach (SkillEffector skillEffector in skillEffectorsList)
            {
                if (turnManager.RoundNumber == skillEffector.turnNumber)
                {
                    for (int i = 0;i< JobInfoList.Count; i++)
                    {
                        if (skillEffector.skillName == JobInfoList[i].skillName)
                        {
                            JobInfoList[i].addSkillAmount(skillEffector.amount);
                            upDateSkillAmount(JobInfoList[i].position, skillEffector.amount);
                        }
                    }
                }
                object[] textForBeforeTask = new object[] { missionRollController.settask1beforeText, setGetCurrentCard.hasSecondMission, missionRollController.settask2beforeText, setGetCurrentCard.hasThirdMission, missionRollController.settask3beforeText };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.textForTextBeforeRoll, textForBeforeTask, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        public void clickOnRollButton()
        {
            if (numberOfChances != 0)
            {
                System.Random rand = new System.Random((int)DateTime.Now.Ticks);
                int x = rand.Next(0, 18);
                missionRollController.setRolledNumber = x;
                object[] dataDice = new object[] { x };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.rolledNumberMission, dataDice, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                if (JobInfoList[currentTask].amount <= x)
                {
                    setDuringProgessText(currentTask + 1, "Passed");
                    JobInfoList[currentTask].passingOrNot = true;
                    setDuringStatusText(currentTask + 1, JobInfoList[currentTask].passingOrNot);
                    currentTask += 1;
                }
                else
                {
                    setDuringProgessText(currentTask + 1, "Failed");
                    JobInfoList[currentTask].passingOrNot = false;
                    setDuringStatusText(currentTask + 1, JobInfoList[currentTask].passingOrNot);
                    setgetnumberOfChances -= 1;
                }
                if ((currentTask + 1) <= JobInfoList.Count && numberOfChances != 0)
                {
                    setCurrentTask();
                }
                else
                {
                    setEndScene();
                }
            }
            
        }
        public void onReceiveRolledValue (int rolledValue) => missionRollController.setRolledNumber = rolledValue;
        public void setEndScene()
        {
            switchStage(3);
            for (int i = 0;i< JobInfoList.Count; i++)
            {
                CurrentMissionStatus = CurrentMissionStatus && JobInfoList[i].passingOrNot;
            }
            missionRollController.setActiveendMissionButton = true;
            if (CurrentMissionStatus)
                missionRollController.setcurrentMissionStatusText = "PASSED";
            else
                missionRollController.setcurrentMissionStatusText = "FAILED";
            object[] CurrentMissionStatusData = new object[] { missionRollController.setcurrentMissionStatusText };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setCurrentMissionStatus, CurrentMissionStatusData, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setEndScene, null, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveEndScene()
        {
            switchStage(3);
            missionRollController.setActiveendMissionButton=false;
        }
        public void onReceiveCurrentMissionStatus(string text) => missionRollController.setcurrentMissionStatusText = text;
    }
}
