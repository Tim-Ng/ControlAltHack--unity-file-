using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;
using main;
using UserAreas;
using UnityEngine.UI;
using rollmissions;

namespace DrawCards
{
    /// <summary>
    /// This is a user define variable to hold the data of the job to be swapped
    /// </summary>
    public struct skillToSwap
    {
        /// <summary>
        /// The data of the job
        /// </summary>
        public jobInfos info;
        /// <summary>
        /// The text to be displayed on the dropdown list
        /// </summary>
        public string OnList;
        /// <summary>
        /// The constructer to input the infos
        /// </summary>
        /// <param name="Info">
        /// The info of the jobs
        /// </param>
        public skillToSwap(jobInfos Info)
        {
            info = Info;
            OnList = "Task " + info.position + ":" + GetStringOfTask.get_string_of_job(info.skillName);
        }
    }
    /// <summary>
    /// This class control the character popup 
    /// </summary>
    public class CharCardPopup : MonoBehaviour
    {
        ///<summary>
        ///This are the game object that is in the character skill abilities 
        /// </summary>
        [SerializeField, Header("Character Ability ")] private GameObject forChar10 = null;
        ///<summary>
        ///This are the game object that is in the character skill abilities 
        /// </summary>
        [SerializeField] private GameObject rollRollButton = null, rolledValue = null, statusfor10OBJ = null;
        /// <summary>
        /// This is the game object for the skill activation button
        /// </summary>
        [SerializeField] private GameObject activateSkillButton = null;
        ///<summary>
        ///This are the game object that is in the Charater Card Popup area.
        /// </summary>
        [SerializeField] private GameObject swapSkills = null, swapButton = null, skillToSwapOptions = null, skillSwapToOptions = null, statusSwapOBJ = null;

        ///<summary>
        ///This are the game object for the popup
        /// </summary>
        [SerializeField, Space(20)] private GameObject popUp = null;
        /// <summary>
        /// This is the game object for skill effector displays 
        /// </summary>
        [SerializeField] private GameObject skillChangerEliment = null;
        ///<summary>
        ///This are the game object that is in the Charater Card Popup area.
        /// </summary>
        [SerializeField] private GameObject cardInpopUp = null, buttonSelect = null, playingCardArea = null;
        /// <summary>
        /// The script that holds the data to the DuringMissionRollController.s
        /// </summary>
        [SerializeField] private DuringMissionRollController missionRollItems = null;
        /// <summary>
        /// Script where this script is attacthed to.
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// Refering to the script UserAreaControlers
        /// </summary>
        private UserAreaControlers userAreaControlers = null;
        /// <summary>
        /// Refering to the script rollingMissionControl
        /// </summary>
        private rollingMissionControl missionRollController = null;
        /// <summary>
        /// Refering to the script EventHandeler
        /// </summary>
        private EventHandeler EventManager = null;
        /// <summary>
        /// Refering to the script turnManager
        /// </summary>
        private TurnManager turnManager = null;
        /// <summary>
        /// Refering to the script drawEntropyCard
        /// </summary>
        private drawEntropyCard drawEntropy = null;

        /// <summary>
        /// This is a readonly list of the possible job swap that can only be used for character with the code 12 [Miro]
        /// </summary>
        private readonly List<AllJobs> SkillsToSwapFor12 = new List<AllJobs>() { AllJobs.Crypt, AllJobs.NetNinja, AllJobs.SocialEng, AllJobs.SoftWiz, AllJobs.Kitchen };
        /// <summary>
        /// This is a readonly list of the possible job swap that can only be used for character with the code 5 [Deborah]
        /// </summary>
        private readonly List<AllJobs> SkillsToSwapFor5 = new List<AllJobs>() { AllJobs.HardHack, AllJobs.Crypt, AllJobs.NetNinja, AllJobs.SearchFU, AllJobs.Kitchen, AllJobs.SoftWiz, AllJobs.SocialEng };
        /// <summary>
        /// This is a readonly list of the possible job swap that can only be used for character with the code 13 [Roxana]
        /// </summary>
        private readonly List<AllJobs> SkillSwapToFor13 = new List<AllJobs>() { AllJobs.HardHack, AllJobs.SocialEng };
        /// <summary>
        /// Holds the info of the skill to be swap.
        /// </summary>
        private skillToSwap skill1;
        /// <summary>
        /// The job to be swapped with.
        /// </summary>
        private AllJobs skill2;
        /// <summary>
        /// The round number that this Character Card's ability was last used.
        /// </summary>
        private int RoundNumber = 0;
        /// <summary>
        /// The data of the character to be displayed 
        /// </summary>
        private CharCardScript whichScript = null;
        /// <summary>
        /// The dropdown of Jobs that can be swapped
        /// </summary>
        private Dropdown dropdownSkillToSwapOptions = null;
        /// <summary>
        /// The dropdown of all the options to choose
        /// </summary>
        private Dropdown dropdownSkillSwapTo = null;
        /// <summary>
        /// When the script is loaded this function will fill in the data for the scripts that we this class needs
        /// </summary>
        private void Start()
        {
            ScriptsODJ = gameObject; // this will give out the data of the object that this script is attached to 
            userAreaControlers = ScriptsODJ.GetComponent<UserAreaControlers>();
            missionRollController = ScriptsODJ.GetComponent<rollingMissionControl>();
            EventManager = ScriptsODJ.GetComponent<EventHandeler>();
            turnManager = ScriptsODJ.GetComponent<TurnManager>();
            drawEntropy = ScriptsODJ.GetComponent<drawEntropyCard>();
            dropdownSkillToSwapOptions = skillToSwapOptions.GetComponent<Dropdown>();
            dropdownSkillSwapTo = skillSwapToOptions.GetComponent<Dropdown>();
        }
        /// <summary>
        /// This function is to open the popup as well as setup the data
        /// </summary>
        /// <param name="info"> The info of the charater data to be display</param>
        public void opendCharCard(CharCardScript info)
        {
            activateSkillButton.SetActive(false);
            swapSkills.SetActive(false);
            forChar10.SetActive(false);
            buttonSelect.SetActive(true);
            popUp.SetActive(true);
            whichScript = info;
            cardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
            skillChangerEliment.SetActive(false);
        }
        /// <summary>
        /// To close the character card popup
        /// </summary>
        public void closePopUp()
        {
            popUp.SetActive(false);
        }
        /// <summary>
        /// This is to select the character of the user
        /// </summary>
        /// <remarks>
        /// Will send the info to the userAreaControlers.
        /// Will destory all the character cards on the user area.
        /// Wll send everyone about the charater that is selected.
        /// </remarks>
        public void selectThisChar()
        {
            userAreaControlers.setMyCharacter(whichScript);
            foreach (Transform child in playingCardArea.transform)
            {
                Destroy(child.gameObject);
            }
            closePopUp();
            object[] player = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setWaiting, player, EventManager.AllPeople, SendOptions.SendReliable);
            object[] chatInfo = new object[] {PhotonNetwork.LocalPlayer.NickName + " has selected "+whichScript.character_card_name+".", null, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
        }
        /// <summary>
        /// This is to click on the Avertar images on each player which will display their character info.
        /// </summary>
        /// <remarks>
        /// If the avertar click on is the person him or herself then this function will check whether the special ability can be play.
        /// Else it will just close off the skill changer element and the special ability button.
        /// </remarks>
        /// <param name="which">
        /// This is the param to tell which person you click on.
        /// </param>
        public void clickOnAvertar(int which)
        {
            buttonSelect.SetActive(false);
            popUp.SetActive(true);
            whichScript = userAreaControlers.users[which].characterScript;
            cardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
            if (which == 0)
            {
                activateSkillButton.SetActive(false);
                skillChangerEliment.SetActive(false);
                skillChangerEliment.SetActive(true);
                if (whichScript.character_code == 2 && turnManager.TurnNumber != 0)
                {
                    activateSkillButton.SetActive(true);
                    swapSkills.SetActive(false);
                    forChar10.SetActive(false);
                    if (userAreaControlers.users[0].amountOfMoney >= 1000 && userAreaControlers.users[0].NumberOfCards <= 4)
                    {
                        activateSkillButton.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        activateSkillButton.GetComponent<Button>().interactable = false;
                    }
                }
                else if (whichScript.character_code == 10 && turnManager.TurnNumber != 0)
                {
                    activateSkillButton.SetActive(true);
                    if (RoundNumber == turnManager.RoundNumber)
                    {
                        activateSkillButton.GetComponent<Button>().interactable = false;
                    }
                    else
                    {
                        swapSkills.SetActive(false);
                        forChar10.SetActive(false);
                        if (userAreaControlers.users[0].NumberOfCards <= 4)
                        {
                            activateSkillButton.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            activateSkillButton.GetComponent<Button>().interactable = false;
                        }
                    }
                }
                else if (missionRollItems.setbeforeMission && turnManager.IsMyTurn)
                {
                    if (((whichScript.character_code == 13 ||whichScript.character_code == 12) && (missionRollController.numberOfEntroCardsRemoved == 2)) || whichScript.character_code == 5 || whichScript.character_code == 10)
                    {
                        activateSkillButton.SetActive(true);
                        if (RoundNumber == turnManager.RoundNumber)
                        {
                            activateSkillButton.GetComponent<Button>().interactable = false;
                        }
                        else
                        {
                            activateSkillButton.GetComponent<Button>().interactable = true;
                            swapSkills.SetActive(false);
                            forChar10.SetActive(false);
                        }
                    }
                }
                else
                {
                    activateSkillButton.SetActive(false);
                    swapSkills.SetActive(false);
                    forChar10.SetActive(false);
                }

            }
        }
        /// <summary>
        /// The function to click on When the skill is activated and will act accorrding to your character.
        /// </summary>
        public void onClickOnActivatedSkill()
        {
            if (whichScript.character_code == 2)
            {
                drawEntropy.drawEntropyCards(1);
                userAreaControlers.subMyMoney(1000);
                if (userAreaControlers.users[0].amountOfMoney >= 1000 && userAreaControlers.users[0].NumberOfCards <= 4)
                {
                    activateSkillButton.GetComponent<Button>().interactable = true;
                }
                else
                {
                    activateSkillButton.GetComponent<Button>().interactable = false;
                }
            }
            else if (whichScript.character_code == 12 || whichScript.character_code == 13 || whichScript.character_code == 5)
            {
                swapSkills.SetActive(true);
                RoundNumber = turnManager.RoundNumber;
                activateSkillButton.GetComponent<Button>().interactable = false;
                swapButton.GetComponent<Button>().interactable = true;
                setUpSkillToSwap(whichScript.character_code);
            }
            else if (whichScript.character_code == 10)
            {
                forChar10.SetActive(true);
                rolledValue.GetComponent<Text>().text = "0";
                RoundNumber = turnManager.RoundNumber;
                statusfor10OBJ.GetComponent<Text>().text = "Click to roll";
                activateSkillButton.GetComponent<Button>().interactable = false;
                rollRollButton.GetComponent<Button>().interactable = true;
            }
        }
        /// <summary>
        /// This is to set up the skill to swap ability and the dropdown list of the skill that can be swapped
        /// </summary>
        /// <param name="Which"> The current player's character code</param>
        public void setUpSkillToSwap(int Which)
        {
            List<skillToSwap> JobInfoToSwap = new List<skillToSwap>();
            dropdownSkillToSwapOptions.options.Clear();
            dropdownSkillSwapTo.options.Clear();
            if (Which == 5)
            {
                foreach (jobInfos JobInfo in missionRollController.JobInfoList)
                {
                    if (SkillsToSwapFor5.Contains(JobInfo.skillName))
                    {
                        var NewSkillToSwap = new skillToSwap(JobInfo);
                        JobInfoToSwap.Add(NewSkillToSwap);
                        dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = NewSkillToSwap.OnList });
                    }
                }
                if (JobInfoToSwap.Count == 0)
                {
                    dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = "No Skills" });
                    dropdownSkillToSwapOptions.interactable = false;
                    dropdownSkillSwapTo.interactable = false;
                    swapButton.GetComponent<Button>().interactable = false;
                    statusSwapOBJ.GetComponent<Text>().text = "No skill To swap to";
                    swapButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    skill1 = JobInfoToSwap[0];
                    skill2 = SkillsToSwapFor5[0];
                    dropdownSkillToSwapOptions.interactable = true;
                    dropdownSkillSwapTo.interactable = true;
                    statusSwapOBJ.GetComponent<Text>().text = "Select skills to swap";
                    swapButton.GetComponent<Button>().interactable = false;
                    updateStatus();
                }
                foreach (var item in SkillsToSwapFor5)
                {
                    dropdownSkillSwapTo.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(item) });
                }
                dropdownSkillToSwapOptions.onValueChanged.AddListener(delegate { skill1 = JobInfoToSwap[dropdownSkillToSwapOptions.value]; updateStatus(); });
                dropdownSkillSwapTo.onValueChanged.AddListener(delegate { skill2 = SkillsToSwapFor5[dropdownSkillSwapTo.value]; updateStatus(); });
            }
            else if (Which == 12)
            {
                foreach (jobInfos JobInfo in missionRollController.JobInfoList)
                {
                    if (SkillsToSwapFor12.Contains(JobInfo.skillName))
                    {
                        var NewSkillToSwap = new skillToSwap(JobInfo);
                        JobInfoToSwap.Add(NewSkillToSwap);
                        dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = NewSkillToSwap.OnList });
                    }
                }
                if (JobInfoToSwap.Count == 0)
                {
                    dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = "No Skills" });
                    dropdownSkillToSwapOptions.interactable = false;
                    dropdownSkillSwapTo.interactable = false;
                    swapButton.GetComponent<Button>().interactable = false;
                    statusSwapOBJ.GetComponent<Text>().text = "No skill To swap to";
                    swapButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    skill1 = JobInfoToSwap[0];
                    skill2 = AllJobs.HardHack;
                    dropdownSkillToSwapOptions.interactable = true;
                    dropdownSkillSwapTo.interactable = true;
                    statusSwapOBJ.GetComponent<Text>().text = "Select skills to swap";
                    swapButton.GetComponent<Button>().interactable = false;
                    updateStatus();
                }
                dropdownSkillSwapTo.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(AllJobs.HardHack) });
                dropdownSkillToSwapOptions.onValueChanged.AddListener(delegate { skill1 = JobInfoToSwap[dropdownSkillToSwapOptions.value]; updateStatus(); });
            }
            else if (Which == 13)
            {
                dropdownSkillToSwapOptions.options.Clear();
                dropdownSkillSwapTo.options.Clear();
                foreach (jobInfos JobInfo in missionRollController.JobInfoList)
                {
                    if (JobInfo.skillName == AllJobs.Crypt)
                    {
                        var NewSkillToSwap = new skillToSwap(JobInfo);
                        JobInfoToSwap.Add(NewSkillToSwap);
                        dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = NewSkillToSwap.OnList });
                    }
                }
                if (JobInfoToSwap.Count == 0)
                {
                    dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = "No Skills" });
                    dropdownSkillToSwapOptions.interactable = false;
                    dropdownSkillSwapTo.interactable = false;
                    swapButton.GetComponent<Button>().interactable = false;
                    statusSwapOBJ.GetComponent<Text>().text = "No skill To swap to";
                    swapButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    skill1 = JobInfoToSwap[0];
                    skill2 = SkillSwapToFor13[0];
                    dropdownSkillToSwapOptions.interactable = true;
                    dropdownSkillSwapTo.interactable = true;
                    statusSwapOBJ.GetComponent<Text>().text = "Select skills to swap";
                    swapButton.GetComponent<Button>().interactable = false;
                    updateStatus();
                }
                foreach (var item in SkillSwapToFor13)
                {
                    dropdownSkillSwapTo.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(item) });
                }
                dropdownSkillToSwapOptions.onValueChanged.AddListener(delegate { skill1 = JobInfoToSwap[dropdownSkillToSwapOptions.value]; updateStatus(); });
                dropdownSkillSwapTo.onValueChanged.AddListener(delegate { skill2 = SkillSwapToFor13[dropdownSkillSwapTo.value]; updateStatus(); });
            }
        }
        /// <summary>
        /// This is to update the status to see if the skills can be swapped
        /// </summary>
        public void updateStatus()
        {
            if (skill1.info.skillName == AllJobs.Null || skill2 == AllJobs.Null || skill1.info.skillName == skill2)
            {
                swapButton.GetComponent<Button>().interactable = false;
                statusSwapOBJ.GetComponent<Text>().text = "Can't swap";
            }
            else
            {
                swapButton.GetComponent<Button>().interactable = true;
                statusSwapOBJ.GetComponent<Text>().text = GetStringOfTask.get_string_of_job(skill1.info.skillName) + " swap to " + GetStringOfTask.get_string_of_job(skill2);
            }
        }
        /// <summary>
        /// This is when the button to swap skills is pressed
        /// </summary>
        /// <remarks>
        /// This will send the info to swap to the rollingMissionControl scritp
        /// </remarks>
        public void onClickOnSwap()
        {
            dropdownSkillToSwapOptions.interactable = false;
            dropdownSkillSwapTo.interactable = false;
            swapButton.GetComponent<Button>().interactable = false;
            missionRollController.swapSkill(skill1, skill2);
            closePopUp();
        }
        /// <summary>
        /// This is the Roll button for the special skill of character with the code 10 [Mei]
        /// </summary>
        /// <remarks>
        /// If pass then one card is drawn.
        /// else notthing
        /// </remarks>
        public void onRollButtonFor10()
        {
            System.Random rand = new System.Random((int)DateTime.Now.Ticks);
            int x = rand.Next(0, 18);
            rolledValue.GetComponent<Text>().text = x.ToString();
            if (x >= 11)
            {
                statusfor10OBJ.GetComponent<Text>().text = "Passed and one Entropy will be drawn";
                drawEntropy.drawEntropyCards(1);
            }
            else
            {
                statusfor10OBJ.GetComponent<Text>().text = "You have failed no entropy drawn";
            }
            rollRollButton.GetComponent<Button>().interactable = false;
        }
    }
}
