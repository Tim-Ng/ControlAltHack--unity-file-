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

namespace rollmissions {
    public class rollingMissionControl : MonoBehaviour
    {
        [SerializeField] private DuringMissionRollController missionRollController = null;
        [SerializeField] private UserAreaControlers userArea = null;
        [SerializeField] private missionPopup popup= null;
        [SerializeField] private GameObject MissionCard = null, RollingMissionOBJ = null;
        [SerializeField] private EventHandeler EventManger = null;
        private MissionCardScript currentCard = null ;
        private MissionCardScript setGetCurrentCard
        {
            get { return currentCard; }
            set 
            { 
                currentCard = value;
                MissionCard.GetComponent<Image>().sprite = currentCard.artwork_front_info;
            }
        }
        private int numberOfChances=0,whoTurn=0;
        private float currentTime = 0f, startingTime = 0f;
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
            missionRollController.settask1beforeText = "1] TaskName: " + GetStringOfTask.get_string_of_job(setGetCurrentCard.skill_name_1) + "\n   Amonunt: " + userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_1);
            if (setGetCurrentCard.hasSecondMission)
                missionRollController.settask2beforeText = "2] TaskName: " + GetStringOfTask.get_string_of_job(setGetCurrentCard.skill_name_2) + "\n   Amonunt: " + userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_2);
            if (setGetCurrentCard.hasThirdMission)
            missionRollController.settask3beforeText = "3] TaskName: " + GetStringOfTask.get_string_of_job(setGetCurrentCard.skill_name_3) + "\n   Amonunt: " + userArea.users[0].characterScript.find_which(setGetCurrentCard.skill_name_3);
            object[] textForBeforeTask = new object[] { missionRollController.settask1beforeText, setGetCurrentCard.hasSecondMission, missionRollController.settask2beforeText, setGetCurrentCard.hasThirdMission, missionRollController.settask3beforeText };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.textForTextBeforeRoll, textForBeforeTask, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            whoTurn = 0;
            numberOfChances = 0;
            if (popup.AttendingOrNot)
            {
                numberOfChances = 1;
            }
            else
            {
                numberOfChances = 2;
            }
            missionRollController.setNumberOfChances = numberOfChances;
            currentTime = 30;
            missionRollController.setgetCurrentRollerName = PhotonNetwork.LocalPlayer.NickName;
            RollingMissionOBJ.SetActive(true);
            object[] playerRollingdata = new object[] {PhotonNetwork.LocalPlayer , userArea.users[0].missionScript.Mission_code,numberOfChances};
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendWhoRolling, playerRollingdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveSetOtherPlayerRoll(Player whichPlayer, int MissionCardCode,int numberofChances)
        {
            switchStage(1);
            whoTurn = userArea.findPlayerPosition(whichPlayer);
            missionRollController.setgetCurrentRollerName = whichPlayer.NickName;
            setGetCurrentCard = missionCardDeck.cardDeck[MissionCardCode - 1];
            missionRollController.setActiveStartMissionButton = false;
            this.numberOfChances = numberofChances;
            missionRollController.setNumberOfChances = numberOfChances;
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
            object[] playerDuringdata = new object[] { };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setToDuringRoll, playerDuringdata, EventManger.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void onReceiveChangeToDuringTask()
        {
            switchStage(2);
            onReceiveDuringTaskProgressText(1, "In prograss ");
            onReceiveDuringTaskProgressText(2, "waiting");
            onReceiveDuringTaskProgressText(3, "waiting");
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
    }
}
