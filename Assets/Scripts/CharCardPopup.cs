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
    public struct skillToSwap
    {
        public jobInfos info;
        public string OnList;
        public skillToSwap(jobInfos Info)
        {
            info = Info;
            OnList = "Task " + info.position + ":" + GetStringOfTask.get_string_of_job(info.skillName);
        }
    }
    public class CharCardPopup : MonoBehaviour
    {
        [SerializeField] private UserAreaControlers userAreaControlers = null;
        [SerializeField] private GameObject popUp = null, cardInpopUp = null, buttonSelect = null, playingCardArea = null, skillChangerEliment = null, activateSkillButton = null;
        [SerializeField] private GameObject forChar10 = null, rollRollButton = null,rolledValue = null, statusfor10OBJ = null;
        [SerializeField] private GameObject swapSkills = null, swapButton = null, skillToSwapOptions = null, skillSwapToOptions = null, statusSwapOBJ = null;
        [SerializeField] private DuringMissionRollController missionRollItems = null;
        [SerializeField] private rollingMissionControl missionRollController = null;
        [SerializeField] private EventHandeler EventManger = null;
        [SerializeField] private TurnManager turnManager = null;
        [SerializeField] private drawEntropyCard drawEntropy = null;
        private readonly List<AllJobs> SkillsToSwapFor12 = new List<AllJobs>() { AllJobs.Crypt, AllJobs.NetNinja, AllJobs.SocialEng, AllJobs.SoftWiz, AllJobs.Kitchen };
        private readonly List<AllJobs> SkillsToSwapFor5 = new List<AllJobs>() { AllJobs.HardHack, AllJobs.Crypt, AllJobs.NetNinja, AllJobs.SearchFU, AllJobs.Kitchen, AllJobs.SoftWiz, AllJobs.SocialEng };
        private readonly List<AllJobs> SkillSwapToFor13 = new List<AllJobs>() { AllJobs.HardHack, AllJobs.SocialEng };
        private skillToSwap skill1;
        private AllJobs skill2;
        private int RoundNumber = 0;
        private CharCardScript whichScript = null;
        public void opendCharCard(CharCardScript info)
        {
            buttonSelect.SetActive(true);
            popUp.SetActive(true);
            whichScript = info;
            cardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
            skillChangerEliment.SetActive(false);
        }
        public void closePopUp()
        {
            popUp.SetActive(false);
        }
        public void selectThisChar()
        {
            userAreaControlers.setMyCharacter(whichScript);
            foreach (Transform child in playingCardArea.transform)
            {
                Destroy(child.gameObject);
            }
            closePopUp();
            object[] player = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setWaiting, player, EventManger.AllPeople, SendOptions.SendReliable);
        }
        public void clickOnAvertar(int which)
        {
            buttonSelect.SetActive(false);
            popUp.SetActive(true);
            whichScript = userAreaControlers.users[which].characterScript;
            cardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
            if (which == 0)
            {
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
            else
            {
                activateSkillButton.SetActive(false);
                skillChangerEliment.SetActive(false);
            }
        }
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
        public void setUpSkillToSwap(int Which)
        {
            var dropdownSkillToSwapOptions = skillToSwapOptions.GetComponent<Dropdown>();
            var dropdownSkillSwapTo = skillSwapToOptions.GetComponent<Dropdown>();
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
                    swapButton.GetComponent<Button>().interactable = false;
                    statusSwapOBJ.GetComponent<Text>().text = "No skill To swap to";
                    swapButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    skill1 = JobInfoToSwap[0];
                    skill2 = SkillsToSwapFor5[0];
                    dropdownSkillToSwapOptions.interactable = true;
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
                    swapButton.GetComponent<Button>().interactable = false;
                    statusSwapOBJ.GetComponent<Text>().text = "No skill To swap to";
                    swapButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    skill1 = JobInfoToSwap[0];
                    skill2 = AllJobs.HardHack;
                    dropdownSkillToSwapOptions.interactable = true;
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
                    swapButton.GetComponent<Button>().interactable = false;
                    statusSwapOBJ.GetComponent<Text>().text = "No skill To swap to";
                    swapButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    skill1 = JobInfoToSwap[0];
                    skill2 = SkillSwapToFor13[0];
                    dropdownSkillToSwapOptions.interactable = true;
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
        public void onClickOnSwap()
        {
            missionRollController.swapSkill(skill1, skill2);
            closePopUp();
        }
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
