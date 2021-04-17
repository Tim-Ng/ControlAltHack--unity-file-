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
using TradeScripts;

namespace rollmissions
{
    /// <summary>
    /// This class is the controller for the rolling Mission and the display of the skill effectors. It is used to set up/receive the current roller’s state.
    /// Like how much they had rolled, if they failed this mission or not as well as the progression of the current roll (Before, during and after).
    /// </summary>
    public struct SkillEffector
    {
        /// <summary>
        /// The skillName being effected
        /// </summary>
        public AllJobs skillName;
        /// <summary>
        /// The amount changed
        /// </summary>
        public int amount;
        /// <summary>
        /// Which round this skill is being effected
        /// </summary>
        public int RoundNumber;
        /// <summary>
        /// The constructor to input the value of all the variables
        /// </summary>
        /// <param name="SkillName">The enum for the skill being changed</param>
        /// <param name="Amount">The amount being changed</param>
        /// <param name="roundNumber"> The round what this will take effect</param>
        public SkillEffector(AllJobs SkillName, int Amount, int roundNumber)
        {
            skillName = SkillName;
            amount = Amount;
            RoundNumber = roundNumber;
        }
    }
    /// <summary>
    /// This class is the controller for the rolling Mission and the display of the skill effectors
    /// </summary>
    public class rollingMissionControl : MonoBehaviour
    {
        /// <summary>
        /// The variable that holds the script that hold and controls the element for the rolling mission
        /// </summary>
        [SerializeField,Header("rolling mission element scripts")] private DuringMissionRollController missionRollController = null;
        /// <summary>
        /// The variable that holds the script for the user infomation which is UserAreaControlers
        /// </summary>
        [SerializeField,Header("User info scripts")] private UserAreaControlers userArea = null;
        /// <summary>
        /// The card UI game object for the mission card during roll.
        /// </summary>
        [SerializeField,Header("Other Mission Objects")] private GameObject MissionCard = null;
        /// <summary>
        /// The object that holds the rolling mission elements
        /// </summary>
        [SerializeField] private GameObject RollingMissionOBJ = null;
        /// <summary>
        /// The game object template for displaying the skill effector 
        /// </summary>
        [SerializeField, Header("Skill Effector elements")] private GameObject skillTemplate = null;
        /// <summary>
        /// The game object that holds all skill effector UI objects
        /// </summary>
        [SerializeField] private GameObject skillChangerEliment = null;
        /// <summary>
        /// This list holds all the skill effectors in the form of SkillEffector
        /// </summary>
        private List<SkillEffector> skillEffectorsList = new List<SkillEffector>();

        /// <summary>
        /// This holds the game object of the which the this script is attached to 
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// This holds the script for missionPopup
        /// </summary>
        private missionPopup popup = null;
        /// <summary>
        /// This holds the script for EventHandeler
        /// </summary>
        private EventHandeler EventManger = null;
        /// <summary>
        /// This holds the script for TurnManager
        /// </summary>
        private TurnManager turnManager = null;
        /// <summary>
        /// This holds the script for drawEntropyCard
        /// </summary>
        private drawEntropyCard drawEntropy = null;
        /// <summary>
        /// This holds the script for TradeControler
        /// </summary>
        private TradeControler tradeControl = null;
        /// <summary>
        /// This list holds all the job/task of this mission in the form of jobInfos
        /// </summary>
        public List<jobInfos> JobInfoList = new List<jobInfos>();
        /// <summary>
        /// The current mission card data
        /// </summary>
        private MissionCardScript currentCard = null;
        /// <summary>
        /// If mission is failing or not
        /// </summary>
        [HideInInspector]
        public bool CurrentMissionStatus = false;
        /// <summary>
        /// This is for when entropy card ID 3 is played
        /// </summary>
        private bool entopy3 = false;
        /// <summary>
        /// This function is called when this script is rendered.
        /// </summary>
        /// <remarks>
        /// It will set all the scripts that are needed
        /// </remarks>
        private void Start()
        {
            ScriptsODJ=gameObject;
            tradeControl = ScriptsODJ.GetComponent<TradeControler>();
            popup = ScriptsODJ.GetComponent<missionPopup>();
            EventManger = ScriptsODJ.GetComponent<EventHandeler>();
            turnManager = ScriptsODJ.GetComponent<TurnManager>();
            drawEntropy = ScriptsODJ.GetComponent<drawEntropyCard>();
        }
        /// <summary>
        /// This function is to set/get the current card that is being rolled
        /// </summary>
        private MissionCardScript setGetCurrentCard
        {
            get { return currentCard; }
            set
            {
                currentCard = value;
                MissionCard.GetComponent<Image>().sprite = currentCard.artwork_front_info;
            }
        }
        /// <summary>
        /// Holds the number of chances
        /// </summary>
        private int numberOfChances = 0;
        /// <summary>
        /// Hold the poistion of the current player rolling from the UserAreaContol.users
        /// </summary>
        private int whoTurn;
        /// <summary>
        /// The progression of the current mission <br/>
        /// 0 = first <br/> 
        /// 1 = second <br/>
        /// 2 = third <br/>
        /// </summary>
        private int currentTask = 0;
        /// <summary>
        /// The amount of entropy card to removed to end your mission roll
        /// </summary>
        private int entropyRemove = 0;
        /// <summary>
        /// The amount of entropy card discarded during the mission roll
        /// </summary>
        [HideInInspector]
        public int numberOfEntroCardsRemoved = 0;
        /// <summary>
        /// To set and get the number of chances left 
        /// </summary>
        /// <remarks>
        /// when set the UI for the number of chances is set as well as ending the data to everone else
        /// </remarks>
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
        /// <summary>
        /// The variable for the timer
        /// </summary>
        private float currentTime = 0f;
        /// <summary>
        /// Function will run for every frame
        /// </summary>
        /// <remarks>
        /// If timer is not 0 then the interatebility of the start mission button is set to false <br/>
        /// Else its true
        /// </remarks>
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
                setTimer(currentTime.ToString("0"));
                //This causes issues with the game therefore removed to chage it to a function;
                //object[] timer = new object[] { currentTime.ToString("0") };
                //PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setTimerForRoll, timer, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        /// <summary>
        /// To set the time mer text 
        /// </summary>
        /// <param name="time"> The current timer value</param>
        public void setTimer(string time) => missionRollController.setTimerText = time;
        /// <summary>
        /// When its your time to run
        /// </summary>
        /// <remarks>
        /// This will reset all the value needed to start roll. <br/>
        /// Then will add the mission job/task into the JobInfoList and update the text for all the text while sending the text to other people <br/>
        /// Then setting the number of chances and tell send info to everyone about your mission card as well as telling this current player is rolling now
        /// </remarks>
        public void setRollTimeIsMyTurn()
        {
            switchStage(1);
            numberOfEntroCardsRemoved = 0;
            missionRollController.setActiveStartMissionButton = true;
            missionRollController.setStartMissionButton = false;
            setGetCurrentCard = userArea.users[0].missionScript;
            missionRollController.setActivetask2 = setGetCurrentCard.hasSecondMission;
            missionRollController.setActivetask3 = setGetCurrentCard.hasThirdMission;
            JobInfoList.Clear();
            JobInfoList.Add(new jobInfos(1, setGetCurrentCard.skill_name_1, userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_1), false));
            missionRollController.settask1beforeText = "1] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[0].skillName) + "\nAmonunt: " + JobInfoList[0].amount;
            if (setGetCurrentCard.fist_change_hardnum != 0)
            {
                if (setGetCurrentCard.fist_change_hardnum > 0)
                    missionRollController.settask1beforeText += "+" + setGetCurrentCard.fist_change_hardnum;

                else
                    missionRollController.settask1beforeText += setGetCurrentCard.fist_change_hardnum;
                JobInfoList[0].addSkillAmount(setGetCurrentCard.fist_change_hardnum);
            }
            if (setGetCurrentCard.hasSecondMission)
            {
                JobInfoList.Add(new jobInfos(2, setGetCurrentCard.skill_name_2, userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_2), false));
                missionRollController.settask2beforeText = "2] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[1].skillName) + "\nAmonunt: " + JobInfoList[1].amount;
                if (setGetCurrentCard.second_change_hardnum != 0)
                {
                    if (setGetCurrentCard.second_change_hardnum > 0)
                        missionRollController.settask2beforeText += "+" + setGetCurrentCard.second_change_hardnum;

                    else
                        missionRollController.settask2beforeText += setGetCurrentCard.second_change_hardnum;
                    JobInfoList[1].addSkillAmount(setGetCurrentCard.second_change_hardnum);
                }
            }
            if (setGetCurrentCard.hasThirdMission)
            {
                JobInfoList.Add(new jobInfos(3, setGetCurrentCard.skill_name_3, userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_3), false));
                missionRollController.settask3beforeText = "3] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[2].skillName) + "\nAmonunt: " + JobInfoList[2].amount;
                if (setGetCurrentCard.third_change_hardnum != 0)
                {
                    if (setGetCurrentCard.third_change_hardnum > 0)
                        missionRollController.settask3beforeText += "+" + setGetCurrentCard.third_change_hardnum;

                    else
                        missionRollController.settask3beforeText += setGetCurrentCard.third_change_hardnum;
                    JobInfoList[2].addSkillAmount(setGetCurrentCard.third_change_hardnum);
                }
            }
            CurrentMissionStatus = false;
            object[] textForBeforeTask = new object[] { missionRollController.settask1beforeText, setGetCurrentCard.hasSecondMission, missionRollController.settask2beforeText, setGetCurrentCard.hasThirdMission, missionRollController.settask3beforeText };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.textForTextBeforeRoll, textForBeforeTask, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            checkSkillEffector();
            whoTurn = 0;
            numberOfChances = 0;
            if (popup.AttendingOrNot)
            {
                setgetnumberOfChances = 1;
                if (tradeControl.HowManyPeople == 0)
                {
                    setgetnumberOfChances += 1;
                }
            }
            else
            {
                setgetnumberOfChances = 2;
            }
            if (userArea.users[0].characterScript.character_code == 1)
            {
                setgetnumberOfChances += 1;
            }
            startTimer();
            missionRollController.setgetCurrentRollerName = PhotonNetwork.LocalPlayer.NickName;
            RollingMissionOBJ.SetActive(true);
            object[] playerRollingdata = new object[] { PhotonNetwork.LocalPlayer, userArea.users[0].missionScript.Mission_code };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendWhoRolling, playerRollingdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            object[] chatInfo = new object[] { "It's "+PhotonNetwork.LocalPlayer.NickName+" turn to roll their mission.", null, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManger.AllPeople, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is to set the current timer 
        /// </summary>
        public void startTimer() => currentTime = 3;
        /// <summary>
        /// When receiving the value on who is current rolling and their current mission card
        /// </summary>
        /// <param name="whichPlayer">Which player who is playin </param>
        /// <param name="MissionCardCode">The mission card code being played </param>
        public void onReceiveSetOtherPlayerRoll(Player whichPlayer, int MissionCardCode)
        {
            switchStage(1);
            startTimer();
            whoTurn = userArea.findPlayerPosition(whichPlayer);
            missionRollController.setgetCurrentRollerName = whichPlayer.NickName;
            setGetCurrentCard = missionCardDeck.cardDeck[MissionCardCode - 1];
            missionRollController.setActiveStartMissionButton = false;
            RollingMissionOBJ.SetActive(true);
        }
        /// <summary>
        /// To receiving and setting the text for all the text
        /// </summary>
        /// <param name="task1">Text for task 1</param>
        /// <param name="hasTask2">If there is task 2 then true else false</param>
        /// <param name="task2">Text for task 2</param>
        /// <param name="hasTask3">If there is task 3 then true else false</param>
        /// <param name="task3">Text for task 3</param>
        public void onReceiveTextBeforeRollTasks(string task1, bool hasTask2, string task2, bool hasTask3, string task3)
        {
            missionRollController.settask1beforeText = task1;
            missionRollController.setActivetask2 = hasTask2;
            missionRollController.setActivetask3 = hasTask3;
            if (hasTask2)
                missionRollController.settask2beforeText = task2;
            if (hasTask3)
                missionRollController.settask3beforeText = task3;
        }
        /// <summary>
        /// To switch between the stages before roll, during roll and after roll or close
        /// </summary>
        /// <param name="whichStage">
        /// If switchStage = 1 then open before only <br/>
        /// else If switchStage = 2 then open during only <br/>
        /// else If switchStage = 3 then open after only <br/>
        /// else close everything and popup 
        /// </param>
        public void switchStage(int whichStage)
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
            else if (whichStage == 3)
            {
                missionRollController.setbeforeMission = false;
                missionRollController.setDuringMission = false;
                missionRollController.setAfterMission = true;
            }
            else
            {
                RollingMissionOBJ.SetActive(false);
                missionRollController.setbeforeMission = false;
                missionRollController.setDuringMission = false;
                missionRollController.setAfterMission = false;
            }
        }
        /// <summary>
        /// This is to attached to the mission card to open the pop up for the current mission
        /// </summary>
        public void OnClickOnMissionCard() => popup.clickOnCard(setGetCurrentCard, whoTurn, false);
        /// <summary>
        /// This is attached to the start button to start rolling.
        /// </summary>
        /// <remarks>
        /// To set all the text and for missions for during mission <br/>
        /// and tell that it is rolling time
        /// </remarks>
        public void onClickOnStartButton()
        {
            switchStage(2);
            onReceiveDuringTaskProgressText(1, "In prograss ");
            onReceiveDuringTaskProgressText(2, "waiting");
            onReceiveDuringTaskProgressText(3, "waiting");
            setDuringStatusText(1, "Failed");
            if (JobInfoList.Count >= 2)
            {
                setDuringStatusText(2, "Failed");
            }
            if(JobInfoList.Count == 3)
            {
                setDuringStatusText(3, "Failed");
            }
            missionRollController.setActiveRollButton = true;
            currentTask = 0;
            setCurrentTask();
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setToDuringRoll, null, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// On receiving that it is during roll
        /// </summary>
        public void onReceiveChangeToDuringTask()
        {
            switchStage(2);
            onReceiveDuringTaskProgressText(1, "In prograss ");
            onReceiveDuringTaskProgressText(2, "waiting");
            onReceiveDuringTaskProgressText(3, "waiting");
            missionRollController.setActiveRollButton = false;
        }
        /// <summary>
        /// This is for characters like iva that can reroll and for some entropy cards
        /// </summary>
        /// <remarks>
        /// To send everyone that you have reroll
        /// </remarks>
        public void chanceToReroll()
        {
            for (int i =0;i < JobInfoList.Count; i++)
            {
                if (!JobInfoList[i].passingOrNot)
                {
                    currentTask = i;
                    break;
                }
            }
            setCurrentTask();
            setDuringProgessText(currentTask + 1, "In progress");
            switchStage(2);
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setSceneForReRoll, null, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is to set the current task
        /// </summary>
        /// <remarks>
        /// If passing then will skip to next task if next is not the last task then setCurrentTask will be called again else setEndScene <br/>
        /// else the current task text will be set 
        /// </remarks>
        public void setCurrentTask()
        {
            if (JobInfoList[currentTask].passingOrNot)
            {
                setDuringProgessText(currentTask + 1, "Passed");
                setDuringStatusText(currentTask + 1, "Passed");
                currentTask += 1;
                if ((currentTask + 1) <= JobInfoList.Count)
                {
                    setCurrentTask();
                }
                else
                {
                    setEndScene();
                }
            }
            else
            {
                setDuringProgessText(currentTask + 1, "In progress");
                missionRollController.setRollOrNextTaskInteractability = true;
                missionRollController.setCurrentText = GetStringOfTask.get_string_of_job(JobInfoList[currentTask].skillName);
                missionRollController.setWhichIsCurrentTask = (currentTask + 1).ToString();
                missionRollController.setrollGoalText = JobInfoList[currentTask].amount.ToString();
                object[] playerDuringtextdata = new object[] { GetStringOfTask.get_string_of_job(JobInfoList[currentTask].skillName), (currentTask + 1).ToString(), JobInfoList[currentTask].amount.ToString() };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setDuringRollText, playerDuringtextdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        /// <summary>
        /// This is when receiveing the text for the roll text 
        /// </summary>
        /// <param name="currentTaskName">the text of currentTaskName</param>
        /// <param name="whichIsCurrentTask">the text of whichIsCurrentTask</param>
        /// <param name="mustbeLessthan">the text of mustbeLessthan</param>
        public void onReceiveControllText(string currentTaskName, string whichIsCurrentTask, string mustbeLessthan)
        {
            missionRollController.setCurrentText = currentTaskName;
            missionRollController.setWhichIsCurrentTask = whichIsCurrentTask;
            missionRollController.setrollGoalText = mustbeLessthan;
        }
        /// <summary>
        /// When receiving the number of chances for the current roll
        /// </summary>
        /// <param name="value">The number of rolls</param>
        public void onReceiveNumberOfChances(string value)
        {
            missionRollController.setNumberOfChances = value;
        }
        /// <summary>
        /// To set the UI text for the during roll progress. And sent to everyone else
        /// </summary>
        /// <param name="which">
        /// Which task :<br/>
        /// 1 = the first task <br/>
        /// 2 = the second task <br/>
        /// 3 = the third task <br/>
        /// </param>
        /// <param name="text">The text for the task</param>
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
        /// <summary>
        /// On receiving the text of the during task roll progress
        /// </summary>
        /// <param name="which">
        /// Which task :<br/>
        /// 1 = the first task <br/>
        /// 2 = the second task <br/>
        /// 3 = the third task <br/>
        /// </param>
        /// <param name="text">The text for the task</param>
        public void onReceiveDuringTaskProgressText(int which, string text)
        {
            if (which == 1)
                missionRollController.settask1duringText = text;
            else if (which == 2)
                missionRollController.settask2duringText = text;
            else if (which == 3)
                missionRollController.settask3duringText = text;
        }
        /// <summary>
        /// This is to set the status of all the tasks
        /// </summary>
        /// <param name="which">
        /// Which task :<br/>
        /// 1 = the first task <br/>
        /// 2 = the second task <br/>
        /// 3 = the third task <br/>
        /// </param>
        /// <param name="text">The text for the status of the roll</param>
        public void setDuringStatusText(int which, string text)
        {
            string tempstring = GetStringOfTask.get_string_of_job(JobInfoList[which - 1].skillName);
            if (which == 1)
                missionRollController.setTaskOneStatus = tempstring + ":"+text;
            else if (which == 2)
                missionRollController.setTaskTwoStatusStatus = tempstring + ":" + text;
            else if (which == 3)
                missionRollController.setTaskThreeStatusStatus = tempstring + ":" + text;
            object[] setStatusTextdata = new object[] { which, tempstring + ":" + text };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setStatusText, setStatusTextdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// On receiving to set the status of all the tasks
        /// </summary>
        /// <param name="which">
        /// Which task :<br/>
        /// 1 = the first task <br/>
        /// 2 = the second task <br/>
        /// 3 = the third task <br/>
        /// </param>
        /// <param name="text">The text for the status of the roll</param>
        public void onReceiveTaskStatusText(int which, string text)
        {
            if (which == 1)
                missionRollController.setTaskOneStatus = text;
            else if (which == 2)
                missionRollController.setTaskTwoStatusStatus = text;
            else if (which == 3)
                missionRollController.setTaskThreeStatusStatus = text;
        }
        /// <summary>
        /// This is when there was an update to the skill [ adding skillEffector ]
        /// </summary>
        /// <param name="which">
        /// Which task :<br/>
        /// 1 = the first task <br/>
        /// 2 = the second task <br/>
        /// 3 = the third task <br/>
        /// </param>
        /// <param name="amount">The amount changed</param>
        public void upDateSkillAmount(int which, int amount)
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
        /// <summary>
        /// To check if there are any skill effectors for the current tasks
        /// </summary>
        public void checkSkillEffector()
        {
            foreach (SkillEffector skillEffector in skillEffectorsList)
            {
                if (turnManager.RoundNumber == skillEffector.RoundNumber)
                {
                    for (int i = 0; i < JobInfoList.Count; i++)
                    {
                        if (skillEffector.skillName == JobInfoList[i].skillName)
                        {
                            upDateSkillAmount(JobInfoList[i].position, skillEffector.amount);
                        }
                    }
                }
                object[] textForBeforeTask = new object[] { missionRollController.settask1beforeText, setGetCurrentCard.hasSecondMission, missionRollController.settask2beforeText, setGetCurrentCard.hasThirdMission, missionRollController.settask3beforeText };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.textForTextBeforeRoll, textForBeforeTask, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        /// <summary>
        /// This is used to check the skill effector after a swap in skills
        /// </summary>
        /// <param name="position">The position of the task being swapped</param>
        public void checkSkillEffector(int position)
        {
            foreach (SkillEffector skillEffector in skillEffectorsList)
            {
                if (turnManager.RoundNumber == skillEffector.RoundNumber)
                {
                    if (skillEffector.skillName == JobInfoList[position - 1].skillName)
                    {
                        upDateSkillAmount(position, skillEffector.amount);
                    }
                }
                object[] textForBeforeTask = new object[] { missionRollController.settask1beforeText, setGetCurrentCard.hasSecondMission, missionRollController.settask2beforeText, setGetCurrentCard.hasThirdMission, missionRollController.settask3beforeText };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.textForTextBeforeRoll, textForBeforeTask, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        /// <summary>
        /// To add new skill effectors
        /// </summary>
        /// <param name="whichJob">Which job is being effected</param>
        /// <param name="whichTurn">On which turn</param>
        /// <param name="amount">The amount change</param>
        public void addSkillEffector(AllJobs whichJob, int whichTurn, int amount)
        {
            SkillEffector tempEffector= new SkillEffector(whichJob, amount, whichTurn);
            GameObject copy = Instantiate(skillTemplate, transform.position, Quaternion.identity);
            copy.transform.SetParent(skillChangerEliment.transform, false);
            copy.GetComponent<skillEffectDisplay>().setSkillID(tempEffector);
            skillEffectorsList.Add(tempEffector);
            if (missionRollController.setbeforeMission)
            {
                if (turnManager.RoundNumber == whichTurn)
                {
                    for (int i = 0; i < JobInfoList.Count; i++)
                    {
                        if (whichJob == JobInfoList[i].skillName)
                        {
                            upDateSkillAmount(JobInfoList[i].position, amount);
                        }
                    }
                }
                object[] textForBeforeTask = new object[] { missionRollController.settask1beforeText, setGetCurrentCard.hasSecondMission, missionRollController.settask2beforeText, setGetCurrentCard.hasThirdMission, missionRollController.settask3beforeText };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.textForTextBeforeRoll, textForBeforeTask, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        /// <summary>
        /// This is when the roll button is pressed that will start the roll and see if the task has passed or failed
        /// </summary>
        public void clickOnRollButton()
        {
            if (numberOfChances != 0)
            {
                System.Random rand = new System.Random((int)DateTime.Now.Ticks);
                int x = rand.Next(0, 18);
                missionRollController.setRolledNumber = x;
                object[] dataDice = new object[] { x };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.rolledNumberMission, dataDice, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
                if (JobInfoList[currentTask].amount >= x)
                {
                    setDuringProgessText(currentTask + 1, "Passed");
                    JobInfoList[currentTask].passingOrNot = true;
                    setDuringStatusText(currentTask + 1, "Passed");
                    currentTask += 1;
                    if (entopy3)
                    {
                        entopy3 = false;
                    }
                }
                else
                {
                    setDuringProgessText(currentTask + 1, "Failed");
                    JobInfoList[currentTask].passingOrNot = false;
                    setDuringStatusText(currentTask + 1, "Failed");
                    setgetnumberOfChances -= 1;
                    if (entopy3)
                    {
                        drawEntropy.removeAnEntropyCard(entropyCardDeck.cardDeck[2],false ); // remove for entorpy 3 if failed
                        entopy3 = false;
                    }
                }
                missionRollController.setRollOrNextTaskInteractability = false;
            }

        }
        /// <summary>
        /// This function is for the click next button 
        /// </summary>
        public void clickOnNextButton()
        {
            if ((currentTask + 1) <= JobInfoList.Count && numberOfChances != 0)
            {
                setCurrentTask();
            }
            else
            {
                setEndScene();
            }
        }
        /// <summary>
        /// On receiving the amount rolled
        /// </summary>
        /// <param name="rolledValue">amount rolled</param>
        public void onReceiveRolledValue(int rolledValue) => missionRollController.setRolledNumber = rolledValue;
        /// <summary>
        /// To set the end scene
        /// </summary>
        public void setEndScene()
        {
            switchStage(3);
            checkCurrentMissionStatus();
            missionRollController.setActiveendMissionButton = true;
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setEndScene, null, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// To check is the current mission is failing or not 
        /// </summary>
        public void checkCurrentMissionStatus()
        {
            CurrentMissionStatus = true;
            for (int i = 0; i < JobInfoList.Count; i++)
            {
                CurrentMissionStatus = CurrentMissionStatus && JobInfoList[i].passingOrNot;
            }
            if (CurrentMissionStatus == false && setGetCurrentCard.Mission_code == 2)
            {
                if (userArea.users[0].NumberOfCards >= 2)
                {
                    entropyRemove = 2;
                }
                else if (userArea.users[0].NumberOfCards == 1)
                {
                    entropyRemove = 1;
                }
                else if (userArea.users[0].NumberOfCards ==0)
                {
                    entropyRemove = 0;
                }
                missionRollController.setInteractableendMissionButton = false;
                setCurrentMissionOutputText("Remove " + entropyRemove + " entropy cards to continue");
            }
            else
            {
                setStatusOutputText();
            }
            if (CurrentMissionStatus)
                missionRollController.setcurrentMissionStatusText = "PASSED";
            else
                missionRollController.setcurrentMissionStatusText = "FAILED";
            object[] CurrentMissionStatusData = new object[] { missionRollController.setcurrentMissionStatusText };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setCurrentMissionStatus, CurrentMissionStatusData, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// To set the status text [ reward or penalty] of the curent mission 
        /// </summary>
        public void setStatusOutputText()
        {
            if (CurrentMissionStatus)
            {
                setCurrentMissionOutputText(setGetCurrentCard.ifpassText);
            }
            else
            {
                setCurrentMissionOutputText(setGetCurrentCard.iffailText);
            }
        }
        /// <summary>
        /// This is to set the current mission status text as well as sending it to other people 
        /// </summary>
        /// <param name="text">the text of reward or penalty </param>
        public void setCurrentMissionOutputText(string text)
        {
            missionRollController.setGetMissionStatusOutPut = text;
            object[] CurrentMissionStatusOutputData = new object[] { missionRollController.setGetMissionStatusOutPut };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setStatusOutput, CurrentMissionStatusOutputData, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        /// <summary>
        /// On receiving and set the current mission status text
        /// </summary>
        /// <param name="text">the text of reward or penalty </param>
        public void onReceiveCurrentMissionOutputText(string text) => missionRollController.setGetMissionStatusOutPut = text;
        /// <summary>
        /// On receiving to change to the end scene
        /// </summary>
        public void onReceiveEndScene()
        {
            switchStage(3);
            missionRollController.setActiveendMissionButton = false;
        }
        /// <summary>
        /// When an entropy card is removed this function is called
        /// </summary>
        public void removedAnEntropy()
        {
            if (userArea.users[0].characterScript.character_code== 8)
            {
                setgetnumberOfChances += 1;
                entropyRemove = 0;
                if (missionRollController.setAfterMission)
                {
                    chanceToReroll();
                }
            }
            else
            {
                if (missionRollController.setAfterMission && setGetCurrentCard.Mission_code == 2 && CurrentMissionStatus==false)
                {
                    if (entropyRemove != 0)
                    {
                        entropyRemove -= 1;
                        missionRollController.setInteractableendMissionButton = false;
                        setCurrentMissionOutputText("Remove " + entropyRemove + " entropy cards to continue");
                    }
                    else
                    {
                        missionRollController.setInteractableendMissionButton = true;
                        setCurrentMissionOutputText(setGetCurrentCard.iffailText);
                    }
                }
            }
            numberOfEntroCardsRemoved += 1;
        }
        /// <summary>
        /// This is to set the text for the current mission status text 
        /// </summary>
        /// <param name="text">The text either pass or fail </param>
        public void onReceiveCurrentMissionStatus(string text) => missionRollController.setcurrentMissionStatusText = text;
        /// <summary>
        /// This function is called when the button to end roll is clicked
        /// </summary>
        /// <remarks>
        /// Will set the correct reward/penalty as well as taking into account to the mission Id <br/>
        /// This will also remove all the skill effector with the current round number 
        /// </remarks>
        public void onClickEndTurnButton()
        { 
            if (CurrentMissionStatus)
            {
                userArea.addMyCred(setGetCurrentCard.success_amount_hacker_cread);
                if (userArea.users[0].characterScript.character_code == 14)
                {
                    for (int i = 0; i < JobInfoList.Count; i++)
                    {
                        if (JobInfoList[i].skillName == AllJobs.HardHack)
                        {
                            userArea.addMyCred(1);
                            break;
                        }
                    }
                }
                if (setGetCurrentCard.Mission_code == 2 || setGetCurrentCard.Mission_code == 14 || setGetCurrentCard.Mission_code == 18 || setGetCurrentCard.Mission_code == 19 || setGetCurrentCard.Mission_code == 33 || setGetCurrentCard.Mission_code == 35 || setGetCurrentCard.Mission_code == 37)
                {
                    drawEntropy.drawEntropyCards(1);
                }
                else if (setGetCurrentCard.Mission_code == 3 || setGetCurrentCard.Mission_code == 11 || setGetCurrentCard.Mission_code == 16 || setGetCurrentCard.Mission_code == 28 || setGetCurrentCard.Mission_code == 34)
                {
                    userArea.addMyMoney(setGetCurrentCard.other_success_how_much);
                }
                object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " has pass their mission.", null, false };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManger.AllPeople, SendOptions.SendReliable);
            }
            else
            {
                userArea.subMyCred(setGetCurrentCard.failure_amount_hacker_cread);
                if (userArea.users[0].characterScript.character_code== 11)
                {
                    userArea.subMyCred(1);
                }
                if (setGetCurrentCard.Mission_code == 7 || setGetCurrentCard.Mission_code == 27)
                {
                    userArea.subMyMoney(setGetCurrentCard.other_success_how_much);
                }
                else if (setGetCurrentCard.Mission_code == 5)
                {
                    addSkillEffector(AllJobs.SoftWiz, turnManager.RoundNumber + 1, -2);
                }
                object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " has failed their mission.", null, false };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManger.AllPeople, SendOptions.SendReliable);
            }
            foreach (Transform child in skillChangerEliment.transform)
            {
                if (child.GetComponent<skillEffectDisplay>().getSkillTurn() == turnManager.RoundNumber)
                {
                    skillEffectorsList.Remove(child.GetComponent<skillEffectDisplay>().getSkillEffector());
                    GameObject.Destroy(child.gameObject);
                }
            }
            for (int i =0; i < skillEffectorsList.Count; i++)
            {
                if (turnManager.RoundNumber == skillEffectorsList[i].RoundNumber)
                {
                    skillEffectorsList.Remove(skillEffectorsList[i]);
                }
            }
            turnManager.EndTurn();
        }
        /// <summary>
        /// This function is used to swap askill of a task
        /// </summary>
        /// <param name="skill1">The info of which task to swap</param>
        /// <param name="skill2">The skill to change to</param>
        public void swapSkill(skillToSwap skill1,AllJobs skill2)
        {
            int whichPosition = skill1.info.position;
            JobInfoList[whichPosition - 1].skillName = skill2;
            JobInfoList[whichPosition - 1].amount = userArea.users[0].characterScript.find_which(JobInfoList[whichPosition - 1].skillName);
            if (whichPosition == 1)
            {
                missionRollController.settask2beforeText = "1] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[0].skillName) + "\n   Amonunt: " + JobInfoList[0].amount;
                setDuringStatusText(1, "Failed");
                if (setGetCurrentCard.fist_change_hardnum != 0)
                {
                    if (setGetCurrentCard.fist_change_hardnum > 0)
                        missionRollController.settask1beforeText += "+" + setGetCurrentCard.fist_change_hardnum;

                    else
                        missionRollController.settask1beforeText += setGetCurrentCard.fist_change_hardnum;
                    JobInfoList[0].addSkillAmount(setGetCurrentCard.fist_change_hardnum);
                }
            }
            else if (whichPosition == 2)
            {
                setDuringStatusText(2, "Failed");
                missionRollController.settask2beforeText = "2] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[1].skillName) + "\n   Amonunt: " + JobInfoList[1].amount;
                if (setGetCurrentCard.second_change_hardnum != 0)
                {
                    if (setGetCurrentCard.second_change_hardnum > 0)
                        missionRollController.settask2beforeText += "+" + setGetCurrentCard.second_change_hardnum;

                    else
                        missionRollController.settask2beforeText += setGetCurrentCard.second_change_hardnum;
                    JobInfoList[1].addSkillAmount(setGetCurrentCard.second_change_hardnum);
                }
            }
            else if (whichPosition == 3)
            {
                setDuringStatusText(3, "Failed");
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
            checkSkillEffector(whichPosition);
        }
        /// <summary>
        /// Entorpy card to function to convert the task from fail to pass
        /// </summary>
        /// <param name="whichTask"> Which kind of task to convert to pass </param>
        /// <param name="cost">The cost of the card</param>
        /// <param name="entropyCard"> The entropy card Id</param>
        public void convertFailedToPass(AllJobs whichTask, int cost,EntropyCardScript entropyCard)
        {
            bool ifCanChange = false;
            for (int i = 0; i < JobInfoList.Count; i++)
            {
                if (!JobInfoList[i].passingOrNot)
                {
                    if (JobInfoList[i].skillName == whichTask)
                    {
                        ifCanChange = true;
                        break;
                    }
                    else
                    {
                        ifCanChange = false;
                        break;
                    }
                }
            }
            if (ifCanChange)
            {
                for (int i = 0; i < JobInfoList.Count; i++)
                {
                    if (JobInfoList[i].skillName == whichTask)
                    {
                        JobInfoList[i].passingOrNot = true;
                        setDuringStatusText(i + 1, "Passed");
                        setDuringProgessText(i + 1, "Passed");
                        ifCanChange = true;
                    }
                }
                checkCurrentMissionStatus();
                if (!CurrentMissionStatus)
                {
                    setgetnumberOfChances += 1;
                    chanceToReroll();
                }
                if (entropyCard.EntropyCardID == 14 || entropyCard.EntropyCardID == 15)
                {
                    drawEntropy.removeAnEntropyCard(entropyCard, false);
                }
            }
            else
            {
                userArea.addMyMoney(cost);
            }
        }
        /// <summary>
        /// To check if the entropy card played can late reroll. <br/>
        /// If entropy is 3 then the varable entopy3 = to true if the next roll fails entropy card with id 3 will be removed
        /// </summary>
        /// <param name="whichTask"> Which task </param>
        /// <param name="cost">the cost</param>
        /// <param name="cardID">The entropy card id</param>
        public void checkIfCanReroll(AllJobs whichTask,int cost,int cardID)
        {
            bool CantReroll = true;
            for (int i = 0; i < JobInfoList.Count; i++)
            {
                if (!JobInfoList[i].passingOrNot)
                {
                    if (JobInfoList[i].skillName == whichTask)
                    {
                        CantReroll = false;
                        setgetnumberOfChances += 1;
                        chanceToReroll();
                        break;
                    }
                    else
                    {
                        CantReroll = true;
                        break;
                    }
                }
            }
            if (CantReroll)
            {
                userArea.addMyMoney(cost);
            }
            else
            {
                if (cardID == 3)
                {
                    entopy3 = true;
                }
            }
        }
    }
}
