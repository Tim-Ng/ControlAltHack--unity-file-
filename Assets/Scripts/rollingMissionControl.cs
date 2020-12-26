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
    public class jobInfos
    {
        public int position; // 1 , 2 or 3
        public AllJobs skillName;
        public int amount;
        public bool passingOrNot { get; set; }
        public jobInfos(int Position, AllJobs SkillName, int Amount, bool PassingOrNOt)
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
        public int RoundNumber;
        public SkillEffector(AllJobs SkillName, int Amount, int roundNumber)
        {
            skillName = SkillName;
            amount = Amount;
            RoundNumber = roundNumber;
        }
    }
    public class rollingMissionControl : MonoBehaviour
    {
        [SerializeField] private DuringMissionRollController missionRollController = null;
        [SerializeField] private UserAreaControlers userArea = null;
        [SerializeField] private missionPopup popup = null;
        [SerializeField] private GameObject MissionCard = null, RollingMissionOBJ = null;
        [SerializeField] private EventHandeler EventManger = null;
        [SerializeField] private TurnManager turnManager = null;
        [SerializeField] private drawEntropyCard drawEntropy = null;
        [SerializeField] private TradeControler tradeControl = null;
        [SerializeField] private GameObject skillTemplate = null, skillChangerEliment = null;
        private List<SkillEffector> skillEffectorsList = new List<SkillEffector>();
        public List<jobInfos> JobInfoList = new List<jobInfos>();
        private MissionCardScript currentCard = null;
        public bool CurrentMissionStatus = false;
        private bool entopy3 = false;
        private MissionCardScript setGetCurrentCard
        {
            get { return currentCard; }
            set
            {
                currentCard = value;
                MissionCard.GetComponent<Image>().sprite = currentCard.artwork_front_info;
            }
        }
        private int numberOfChances = 0, whoTurn, currentTask = 0, entropyRemove = 0;
        public int numberOfEntroCardsRemoved = 0;
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
                setTimer(currentTime.ToString("0"));
                object[] timer = new object[] { currentTime.ToString("0") };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setTimerForRoll, timer, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        public void setTimer(string time) => missionRollController.setTimerText = time;
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
            missionRollController.settask1beforeText = "1] TaskName: " + GetStringOfTask.get_string_of_job(JobInfoList[0].skillName) + "\n   Amonunt: " + JobInfoList[0].amount;
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
            if (setGetCurrentCard.hasThirdMission)
            {
                JobInfoList.Add(new jobInfos(3, setGetCurrentCard.skill_name_3, userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_3), false));
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
            currentTime = 3;
            missionRollController.setgetCurrentRollerName = PhotonNetwork.LocalPlayer.NickName;
            RollingMissionOBJ.SetActive(true);
            object[] playerRollingdata = new object[] { PhotonNetwork.LocalPlayer, userArea.users[0].missionScript.Mission_code };
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
        public void OnClickOnMissionCard() => popup.clickOnCard(setGetCurrentCard, whoTurn, false);
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
            object[] playerDuringdata = new object[] { };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setToDuringRoll, playerDuringdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveChangeToDuringTask()
        {
            switchStage(2);
            onReceiveDuringTaskProgressText(1, "In prograss ");
            onReceiveDuringTaskProgressText(2, "waiting");
            onReceiveDuringTaskProgressText(3, "waiting");
            missionRollController.setActiveRollButton = false;
        }
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
                missionRollController.setCurrentText = GetStringOfTask.get_string_of_job(JobInfoList[currentTask].skillName);
                missionRollController.setWhichIsCurrentTask = (currentTask + 1).ToString();
                missionRollController.setrollGoalText = JobInfoList[currentTask].amount.ToString();
                object[] playerDuringtextdata = new object[] { GetStringOfTask.get_string_of_job(JobInfoList[currentTask].skillName), (currentTask + 1).ToString(), JobInfoList[currentTask].amount.ToString() };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setDuringRollText, playerDuringtextdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            }
        }
        public void onReceiveControllText(string currentTaskName, string whichIsCurrentTask, string mustbeLessthan)
        {
            missionRollController.setCurrentText = currentTaskName;
            missionRollController.setWhichIsCurrentTask = whichIsCurrentTask;
            missionRollController.setrollGoalText = mustbeLessthan;

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
        public void onReceiveDuringTaskProgressText(int which, string text)
        {
            if (which == 1)
                missionRollController.settask1duringText = text;
            else if (which == 2)
                missionRollController.settask2duringText = text;
            else if (which == 3)
                missionRollController.settask3duringText = text;
        }
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
        public void onReceiveTaskStatusText(int which, string text)
        {
            if (which == 1)
                missionRollController.setTaskOneStatus = text;
            else if (which == 2)
                missionRollController.setTaskTwoStatusStatus = text;
            else if (which == 3)
                missionRollController.setTaskThreeStatusStatus = text;
        }
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
        public void onReceiveRolledValue(int rolledValue) => missionRollController.setRolledNumber = rolledValue;
        public void setEndScene()
        {
            switchStage(3);
            checkCurrentMissionStatus();
            missionRollController.setActiveendMissionButton = true;
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setEndScene, null, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
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
        public void setCurrentMissionOutputText(string text)
        {
            missionRollController.setGetMissionStatusOutPut = text;
            object[] CurrentMissionStatusOutputData = new object[] { missionRollController.setGetMissionStatusOutPut };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setStatusOutput, CurrentMissionStatusOutputData, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveCurrentMissionOutputText(string text) => missionRollController.setGetMissionStatusOutPut = text;
        public void onReceiveEndScene()
        {
            switchStage(3);
            missionRollController.setActiveendMissionButton = false;
        }
        public void removedAnEntropy()
        {
            if (userArea.users[0].characterScript.character_code== 8)
            {
                setgetnumberOfChances += 1;
                if (missionRollController.setAfterMission)
                {
                    chanceToReroll();
                }
            }
            if (entropyRemove != 0)
                entropyRemove -= 1;
            if (entropyRemove != 0)
            {
                missionRollController.setInteractableendMissionButton = false;
                setCurrentMissionOutputText("Remove " + entropyRemove + " entropy cards to continue");
            }
            else
            {
                missionRollController.setInteractableendMissionButton = true;
            }
            numberOfEntroCardsRemoved += 1;
        }
        public void onReceiveCurrentMissionStatus(string text) => missionRollController.setcurrentMissionStatusText = text;
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
            }
            foreach (Transform child in skillChangerEliment.transform)
            {
                if (child.GetComponent<skillEffectDisplay>().getSkillTurn() == turnManager.RoundNumber)
                {
                    skillEffectorsList.Remove(child.GetComponent<skillEffectDisplay>().getSkillEffector());
                    GameObject.Destroy(child.gameObject);
                }
            }
            turnManager.EndTurn();
        }
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
