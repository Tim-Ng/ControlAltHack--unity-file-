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
    public class CharCardPopup : MonoBehaviour
    {
        [SerializeField] private UserAreaControlers userAreaControlers = null;
        [SerializeField] private GameObject popUp = null, cardInpopUp = null, buttonSelect = null, playingCardArea = null, skillChangerEliment = null, activateSkillButton = null;
        [SerializeField] private GameObject forChar10 = null, rollRollButton = null;
        [SerializeField] private GameObject swapSkills = null, swapButton = null, skillToSwapOptions = null, skillSwapToOptions = null, statusSwapOBJ = null;
        [SerializeField] private DuringMissionRollController missionRollItems = null;
        [SerializeField] private rollingMissionControl missionRollController = null;
        [SerializeField] private EventHandeler EventManger = null;
        [SerializeField] private TurnManager turnManager = null;
        [SerializeField] private drawEntropyCard drawEntropy = null;
        private AllJobs skill1, skill2;
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
                rollRollButton.GetComponent<Button>().interactable = true;
                setUpSkillToSwap(whichScript.character_code);
            }
            else if (whichScript.character_code == 10)
            {
                forChar10.SetActive(true);
                RoundNumber = turnManager.RoundNumber;
                activateSkillButton.GetComponent<Button>().interactable = false;
                swapButton.GetComponent<Button>().interactable = true;
            }
        }
        public void setUpSkillToSwap(int Which)
        {
            var dropdownSkillToSwapOptions = skillToSwapOptions.GetComponent<Dropdown>();
            var dropdownSkillSwapTo = skillSwapToOptions.GetComponent<Dropdown>();
            if (Which == 5)
            {
                dropdownSkillToSwapOptions.options.Clear();
                dropdownSkillSwapTo.options.Clear();
                List<AllJobs> SkillsToSwapFor5 =new List<AllJobs>() { AllJobs.HardHack, AllJobs.Crypt, AllJobs.NetNinja, AllJobs.SearchFU, AllJobs.Kitchen, AllJobs.SoftWiz, AllJobs.SocialEng };
                foreach (var item in SkillsToSwapFor5)
                {
                    dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(item) });
                    dropdownSkillSwapTo.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(item) });
                }
                dropdownSkillToSwapOptions.onValueChanged.AddListener(delegate { skill1 = SkillsToSwapFor5[dropdownSkillToSwapOptions.value]; updateStatus(); });
                dropdownSkillSwapTo.onValueChanged.AddListener(delegate { skill2 = SkillsToSwapFor5[dropdownSkillSwapTo.value]; updateStatus(); });
            }
            else if (Which == 12)
            {
                dropdownSkillToSwapOptions.options.Clear();
                dropdownSkillSwapTo.options.Clear();
                List<AllJobs> SkillsToSwapFor12 = new List<AllJobs>() { AllJobs.HardHack, AllJobs.Crypt, AllJobs.NetNinja, AllJobs.SocialEng, AllJobs.SoftWiz, AllJobs.Kitchen };
                foreach (var item in SkillsToSwapFor12)
                {
                    dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(item) });
                }
                dropdownSkillSwapTo.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(AllJobs.HardHack) });
                dropdownSkillToSwapOptions.onValueChanged.AddListener(delegate { skill1 = SkillsToSwapFor12[dropdownSkillToSwapOptions.value]; updateStatus(); });
                skill2 = AllJobs.HardHack;
            }
            else if (Which == 13)
            {
                dropdownSkillToSwapOptions.options.Clear();
                dropdownSkillSwapTo.options.Clear();
                List<AllJobs> SkillSwapToFor13 = new List<AllJobs>() { AllJobs.HardHack,AllJobs.SocialEng };
                dropdownSkillToSwapOptions.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(AllJobs.Crypt) });
                foreach (var item in SkillSwapToFor13)
                {
                    dropdownSkillSwapTo.options.Add(new Dropdown.OptionData() { text = GetStringOfTask.get_string_of_job(item) });
                }
                skill1 = AllJobs.Crypt;
                dropdownSkillSwapTo.onValueChanged.AddListener(delegate { skill2 = SkillSwapToFor13[dropdownSkillSwapTo.value]; updateStatus(); });
            }
        }
        public void updateStatus()
        {
            statusSwapOBJ.GetComponent<Text>().text = GetStringOfTask.get_string_of_job(skill1) + " swap to " + GetStringOfTask.get_string_of_job(skill2);
            if (skill1 == AllJobs.Null || skill2 == AllJobs.Null || skill1 ==skill2)
            {
                swapButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                swapButton.GetComponent<Button>().interactable = true;
            }
        }
    }
}
