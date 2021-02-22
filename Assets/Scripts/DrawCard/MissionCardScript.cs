using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace DrawCards {
    /// <summary>
    /// This script for a scriptableObject to hold the data of missionCards.
    /// </summary>
    /// <remarks>
    /// To create a scroptableObject right click and hover on create find the fileName "New Card" and click on MissionCard
    /// </remarks>
    [CreateAssetMenu(fileName = "New Card", menuName = "MissionCard")]
    public class MissionCardScript : ScriptableObject
    {
        /// <summary>
        /// This is to hold the mission card Code 
        /// </summary>
        [Header("Mission Card ID")]
        public int Mission_code;
        /// <summary>
        /// This holds the mission Title 
        /// </summary>
        public string MissonTitle;
        /// <summary>
        /// If true then it is a newb mission else it is not
        /// </summary>
        [Header("Is this a newb mission")]
        public bool Newb_job;

        /// <summary>
        /// The enum of the job type for task 1
        /// </summary>
        [Header("First task")]
        public AllJobs skill_name_1;
        /// <summary>
        /// The hardness of the task of task 1 
        /// </summary>
        public int fist_change_hardnum;

        /// <summary>
        /// If this mission has a second task. <br/>
        /// If true will have task 2 else does not have task 2
        /// </summary>
        [Header("Second task")]
        public bool hasSecondMission;
        /// <summary>
        /// The enum of the job type for task 2
        /// </summary>
        public AllJobs skill_name_2;
        /// <summary>
        /// The hardness of the task of task 2
        /// </summary>
        public int second_change_hardnum;

        /// <summary>
        /// If this mission has a third task. <br/>
        /// If true will have task 3 else does not have task 3
        /// </summary>
        [Header("Third task")]
        public bool hasThirdMission;
        /// <summary>
        /// The enum of the job type for task 3
        /// </summary>
        public AllJobs skill_name_3;
        /// <summary>
        /// The hardness of the task of task 3
        /// </summary>
        public int third_change_hardnum;

        /// <summary>
        /// The amount of cred add if success 
        /// </summary>
        [Header("If success")]
        public int success_amount_hacker_cread;
        /// <summary>
        /// The text that state the other benifits for passing this mission
        /// </summary>
        public string other_success_things;
        /// <summary>
        /// The text that state the amount of benifits
        /// </summary>
        public int other_success_how_much;
        /// <summary>
        /// The text for the items get from passing this mission
        /// </summary>
        [Multiline]
        public string ifpassText;

        /// <summary>
        /// The amount of cred sub if fail 
        /// </summary>
        [Header("If fail")]
        public int failure_amount_hacker_cread;
        /// <summary>
        /// The text that state the other debuff/things to loose if fail this mission
        /// </summary>
        public string other_failure_things;
        /// <summary>
        /// The text that state the amount of debuff/things to loose
        /// </summary>
        public int other_failure_how_much;
        /// <summary>
        /// The text for the items get from failing this mission
        /// </summary>
        [Multiline]
        public string iffailText;

        /// <summary>
        /// Image/Sprite of the back of the card 
        /// </summary>
        [Header("Sprite of the card")]
        public Sprite artwork_back;
        /// <summary>
        /// Image/Sprite of the front of the card [info]
        /// </summary>
        public Sprite artwork_front_info;
        
        
    }
}
