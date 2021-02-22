using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DrawCards;
using UnityEngine.UI;

namespace rollmissions
{
    /// <summary>
    /// The class script is to holds the objects for the mission roll
    /// </summary>
    public class DuringMissionRollController : MonoBehaviour
    {
        /// <summary>
        /// This is the mission card object to display
        /// </summary>
        [SerializeField] GameObject MissionCard = null;
        /// <summary>
        /// These game objects are the ones holding the game objects for their respective roll stages [before,during and after]
        /// </summary>
        [SerializeField] GameObject beforeMission = null, duringMission = null, afterMission = null;
        /// <summary>
        /// These are the game object for the elements of before task mission
        /// </summary>
        [SerializeField] GameObject task1beforeText = null, task2beforeText = null, task3beforeText = null, startMissionButton = null, timeText = null, currentRollerName = null;
        /// <summary>
        /// These are the game object for the elements of current task mission
        /// </summary>
        [SerializeField] GameObject currentTaskText = null, rolledNumber = null, task1duringText = null, task2duringText = null, task3duringText = null, rollGoalText = null, chancesLeft = null, whichTask = null,rollButton = null;
        /// <summary>
        /// These are the game object for the elements of after task mission
        /// </summary>
        [SerializeField] GameObject currentMissionStatusText = null, TaskOneStatus = null, TaskTwoStatus = null, TaskThreeStatus = null,endMissionButton = null,missionStatusOutPut = null;
        /// <summary>
        /// The name of the current roller
        /// </summary>
        private string rollerName;
        /// <summary>
        /// Getting and setting the current rollerName varaible as well as setting the text of the game object.
        /// </summary>
        public string setgetCurrentRollerName
        {
            get { return rollerName; }
            set
            {
                rollerName = value;
                currentRollerName.GetComponent<Text>().text = rollerName;
            }
        }
        /// <summary>
        /// Value of the current mission ID 
        /// </summary>
        private int currentMissionID;
        /// <summary>
        /// Getting and setting the currentMissionID variable as well as setting the image of the MissionCard gameObject
        /// </summary>
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
        /// <summary>
        /// Getting and setting the text on the task1beforeText game object
        /// </summary>
        public string settask1beforeText
        {
            get { return task1beforeText.GetComponent<Text>().text; }
            set { task1beforeText.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// Getting and setting the text on the task2beforeText game object
        /// </summary>
        public string settask2beforeText
        {
            get { return task2beforeText.GetComponent<Text>().text; }
            set { task2beforeText.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// Set true if there is task 2 else there is no task 2
        /// </summary>
        /// <remarks>
        /// This will control the game objects task2beforeText, task2duringText and TaskTwoStatus
        /// </remarks>
        public bool setActivetask2
        {
            set 
            { 
                task2beforeText.SetActive(value);
                task2duringText.SetActive(value);
                TaskTwoStatus.SetActive(value);
            }
        }
        /// <summary>
        /// Getting and setting the text on the task3beforeText game object
        /// </summary>
        public string settask3beforeText
        {
            get { return task3beforeText.GetComponent<Text>().text; }
            set { task3beforeText.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// Set true if there is task 3 else there is no task 3
        /// </summary>
        /// <remarks>
        /// This will control the game objects task3beforeText, task3duringText and TaskThreeStatus
        /// </remarks>
        public bool setActivetask3
        {
            set
            {
                task3beforeText.SetActive(value);
                task3duringText.SetActive(value);
                TaskThreeStatus.SetActive(value);
            }
        }
        /// <summary>
        /// setting if the game object is visible or not of the startMissionButton game object
        /// </summary>
        public bool setActiveStartMissionButton
        {
            set { startMissionButton.SetActive(value); }
        }
        /// <summary>
        /// setting if the button is interactable or not of the startMissionButton game object
        /// </summary>
        public bool setStartMissionButton
        {
            set { startMissionButton.GetComponent<Button>().interactable = value; }
        }
        /// <summary>
        /// setting the text of the timer game object (timeText)
        /// </summary>
        public string setTimerText
        {
            set { timeText.GetComponent<Text>().text = value + "s"; }
        }
        /// <summary>
        /// setting and getting if the game object is visible or not of the beforeMission game object
        /// </summary>
        public bool setbeforeMission
        {
            get { return beforeMission.activeSelf; }
            set { beforeMission.SetActive(value); }
        }
        /// <summary>
        /// setting and getting if the game object is visible or not of the setDuringMission game object
        /// </summary>
        public bool setDuringMission
        {
            get { return duringMission.activeSelf; }
            set { duringMission.SetActive(value); }
        }
        /// <summary>
        /// setting the text of the game object currentTaskText
        /// </summary>
        public string setCurrentText
        {
            set { currentTaskText.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// setting the text of the game object rolledNumber
        /// </summary>
        public int setRolledNumber
        {
            set { rolledNumber.GetComponent<Text>().text = value.ToString(); }
        }
        /// <summary>
        /// setting the text of the game object task1duringText
        /// </summary>
        public string settask1duringText
        {
            set { task1duringText.GetComponent<Text>().text = "Task 1: "+ value; }
        }
        /// <summary>
        /// setting the text of the game object task2duringText
        /// </summary>
        public string settask2duringText
        {
            set { task2duringText.GetComponent<Text>().text = "Task 2: "+ value; }
        }
        /// <summary>
        /// setting the text of the game object task3duringText
        /// </summary>
        public string settask3duringText
        {
            set { task3duringText.GetComponent<Text>().text = "Task 3: "+value; }
        }
        /// <summary>
        /// setting the text of the game object chancesLeft
        /// </summary>
        public string setNumberOfChances
        {
            set { chancesLeft.GetComponent<Text>().text = "Chances left: " + value; }
        }
        /// <summary>
        /// setting the text of the game object whichTask
        /// </summary>
        public string setWhichIsCurrentTask
        {
            set { whichTask.GetComponent<Text>().text = "Current task :" + value; }
        }
        /// <summary>
        /// setting the text of the game object rollGoalText
        /// </summary>
        public string setrollGoalText
        {
            set { rollGoalText.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// setting and getting if the game object is visible or not of the afterMission game object
        /// </summary>
        public bool setAfterMission
        {
            get { return afterMission.activeSelf; }
            set { afterMission.SetActive(value); }
        }
        /// <summary>
        /// setting if the game object should be visible or not of the rollButton game object
        /// </summary>
        public bool setActiveRollButton
        {
            set { rollButton.SetActive(value); }
        }
        /// <summary>
        /// setting and getting the text of the game object currentMissionStatusText
        /// </summary>
        public string setcurrentMissionStatusText
        {
            get { return currentMissionStatusText.GetComponent<Text>().text; }
            set { currentMissionStatusText.GetComponent<Text>().text = value; }
        }
        /// <summary>
        /// setting and getting the text of the game object TaskOneStatus
        /// </summary>
        public string setTaskOneStatus
        {
            get { return TaskOneStatus.GetComponent<Text>().text; }
            set
            {
                TaskOneStatus.GetComponent<Text>().text = value;
            }
        }
        /// <summary>
        /// setting and getting the text of the game object TaskTwoStatus
        /// </summary>
        public string setTaskTwoStatusStatus
        {
            get { return TaskTwoStatus.GetComponent<Text>().text; }
            set
            {
                TaskTwoStatus.GetComponent<Text>().text = value;
                
            }
        }
        /// <summary>
        /// setting and getting the text of the game object TaskThreeStatus
        /// </summary>
        public string setTaskThreeStatusStatus
        {
            get { return TaskThreeStatus.GetComponent<Text>().text; }
            set
            {
                TaskThreeStatus.GetComponent<Text>().text = value;
                
            }
        }
        /// <summary>
        /// setting if the game object endMissionButton is visible or not
        /// </summary>
        public bool setActiveendMissionButton
        {
            set { endMissionButton.SetActive(value); }
        }
        /// <summary>
        /// setting if the button endMissionButton is interactable or not
        /// </summary>
        public bool setInteractableendMissionButton
        {
            set { endMissionButton.GetComponent<Button>().interactable = value; }
        }
        /// <summary>
        /// setting and getting the text of the game object missionStatusOutPut
        /// </summary>
        public string setGetMissionStatusOutPut
        {
            get { return missionStatusOutPut.GetComponent<Text>().text; }
            set { missionStatusOutPut.GetComponent<Text>().text = value; }
        }

    }
}
