using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrawCards;
using UnityEngine.UI;

namespace rollmissions
{
    public class DuringMissionRollController : MonoBehaviour
    {
        [SerializeField] GameObject MissionCard = null;
        [SerializeField] GameObject beforeMission = null, duringMission = null, afterMission = null, currentRollerName = null;
        [SerializeField] GameObject task1beforeText = null, task2beforeText = null, task3beforeText = null, startMissionButton = null, timeText = null;
        [SerializeField] GameObject currentTaskText = null, rolledNumber = null, task1duringText = null, task2duringText = null, task3duringText = null, rollGoalText = null, chancesLeft = null, whichTask = null,rollButton = null;
        [SerializeField] GameObject currentMissionStatusText = null, TaskOneStatus = null, TaskTwoStatus = null, TaskThreeStatus = null,endMissionButton = null,missionStatusOutPut = null;
        private string rollerName;
        public string setgetCurrentRollerName
        {
            get { return rollerName; }
            set
            {
                rollerName = value;
                currentRollerName.GetComponent<Text>().text = rollerName;
            }
        }
        private int currentMissionID;
        public int setgetMissionCard
        {
            get { return currentMissionID; }
            set
            {
                currentMissionID = value;
                MissionCard.GetComponent<Image>().sprite = missionCardDeck.cardDeck[currentMissionID - 1].artwork_front_info;
                MissionCard.GetComponent<Button>().interactable = true;
            }
        }

        public string settask1beforeText
        {
            get { return task1beforeText.GetComponent<Text>().text; }
            set { task1beforeText.GetComponent<Text>().text = value; }
        }
        public string settask2beforeText
        {
            get { return task2beforeText.GetComponent<Text>().text; }
            set { task2beforeText.GetComponent<Text>().text = value; }
        }
        public bool setActivetask2
        {
            set 
            { 
                task2beforeText.SetActive(value);
                task2duringText.SetActive(value);
                TaskTwoStatus.SetActive(value);
            }
        }
        public string settask3beforeText
        {
            get { return task3beforeText.GetComponent<Text>().text; }
            set { task3beforeText.GetComponent<Text>().text = value; }
        }
        public bool setActivetask3
        {
            set
            {
                task3beforeText.SetActive(value);
                task3duringText.SetActive(value);
                TaskThreeStatus.SetActive(value);
            }
        }
        public bool setActiveStartMissionButton
        {
            set { startMissionButton.SetActive(value); }
        }
        public bool setStartMissionButton
        {
            set { startMissionButton.GetComponent<Button>().interactable = value; }
        }
        public string setTimerText
        {
            set { timeText.GetComponent<Text>().text = value + "s"; }
        }
        public bool setbeforeMission
        {
            get { return beforeMission.activeSelf; }
            set { beforeMission.SetActive(value); }
        }

        public bool setDuringMission
        {
            get { return duringMission.activeSelf; }
            set { duringMission.SetActive(value); }
        }
        public string setCurrentText
        {
            set { currentTaskText.GetComponent<Text>().text = value; }
        }
        public int setRolledNumber
        {
            set { rolledNumber.GetComponent<Text>().text = value.ToString(); }
        }
        public string settask1duringText
        {
            set { task1duringText.GetComponent<Text>().text = "Task 1: "+ value; }
        }
        public string settask2duringText
        {
            set { task2duringText.GetComponent<Text>().text = "Task 2: "+ value; }
        }
        public string settask3duringText
        {
            set { task3duringText.GetComponent<Text>().text = "Task 3: "+value; }
        }
        public string setNumberOfChances
        {
            set { chancesLeft.GetComponent<Text>().text = "Chances left: " + value; }
        }
        public string setWhichIsCurrentTask
        {
            set { whichTask.GetComponent<Text>().text = "Current task :" + value; }
        }
        public string setrollGoalText
        {
            set { rollGoalText.GetComponent<Text>().text = value; }
        }
        public bool setAfterMission
        {
            get { return afterMission.activeSelf; }
            set { afterMission.SetActive(value); }
        }
        public bool setActiveRollButton
        {
            set { rollButton.SetActive(value); }
        }
        public string setcurrentMissionStatusText
        {
            get { return currentMissionStatusText.GetComponent<Text>().text; }
            set { currentMissionStatusText.GetComponent<Text>().text = value; }
        }
        private string TaskOneStatusValue;
        public string setTaskOneStatus
        {
            get { return TaskOneStatusValue; }
            set
            {
                TaskOneStatusValue = value;
                TaskOneStatus.GetComponent<Text>().text = TaskOneStatusValue;
            }
        }
        private string TaskTwoStatusValue;
        public string setTaskTwoStatusStatus
        {
            get { return TaskTwoStatusValue; }
            set
            {
                TaskTwoStatusValue = value;
                TaskTwoStatus.GetComponent<Text>().text = TaskTwoStatusValue;
                
            }
        }
        private string TaskThreeStatusValue;
        public string setTaskThreeStatusStatus
        {
            get { return TaskThreeStatusValue; }
            set
            {
                TaskThreeStatusValue = value;
                TaskThreeStatus.GetComponent<Text>().text = value;
                
            }
        }
        public bool setActiveendMissionButton
        {
            set { endMissionButton.SetActive(value); }
        }
        public bool setInteractableendMissionButton
        {
            set { endMissionButton.GetComponent<Button>().interactable = value; }
        }
        public string setGetMissionStatusOutPut
        {
            get { return missionStatusOutPut.GetComponent<Text>().text; }
            set { missionStatusOutPut.GetComponent<Text>().text = value; }
        }

    }
}
