using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using Photon.Realtime;
using rollmissions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserAreas;

namespace DrawCards
{
    public class playEntropyCard : MonoBehaviour
    {
        private GameObject ScriptsODJ = null;
        private rollingMissionControl rollingControl = null;
        private UserAreaControlers userArea = null;
        private EventHandeler EventManager = null;
        private TurnManager turnManager = null;
        private drawEntropyCard drawEntropy = null;

        [SerializeField] private DuringMissionRollController missionRollController = null;
        [SerializeField] private GameObject lightningRollOBJs = null, entropyRollCard = null, whichSkillAgainst = null, amountRolled = null, amountNeeded= null,rollButtonEntropy = null;
        private int amountNeededToRoll = 0;
        private EntropyCardScript entropyRollStrike = null;
        public int setamountNeededToRoll
        {
            get { return amountNeededToRoll; }
            set
            {
                amountNeededToRoll = value;
                amountNeeded.GetComponent<Text>().text = amountNeededToRoll.ToString();
            }
        }
        private readonly List<int> extendSive = new List<int> { 20, 21, 22, 23, 24 };
        private void Start()
        {
            ScriptsODJ = gameObject;
            userArea = ScriptsODJ.GetComponent<UserAreaControlers>();
            rollingControl = ScriptsODJ.GetComponent<rollingMissionControl>();
            EventManager = ScriptsODJ.GetComponent<EventHandeler>();
            turnManager = ScriptsODJ.GetComponent<TurnManager>();
            drawEntropy = ScriptsODJ.GetComponent<drawEntropyCard>();
        }
        public void onPlayEntropyCard(EntropyCardScript whichScript)
        {
            int entropyID = whichScript.EntropyCardID;
            if (whichScript.SkillEffecter)
            {
                rollingControl.addSkillEffector(whichScript.whichSkillIncrease1, turnManager.RoundNumber, whichScript.byHowMuchSkillIncrease1);
                if (whichScript.AnotherSecondSkill)
                {
                    rollingControl.addSkillEffector(whichScript.whichSkillIncrease2, turnManager.RoundNumber, whichScript.byHowMuchSkillIncrease2);
                }
                if (userArea.users[0].characterScript.character_code == 9)
                {
                    rollingControl.addSkillEffector(AllJobs.SocialEng, turnManager.RoundNumber,999);
                }
            }
            else if (entropyID == 14)
            {
                rollingControl.convertFailedToPass(AllJobs.Connnections, whichScript.Cost,whichScript);
            }
            else if (entropyID == 15)
            {
                rollingControl.convertFailedToPass(AllJobs.Crypt, whichScript.Cost, whichScript);
            }
            else if (entropyID == 10)
            {
                rollingControl.checkIfCanReroll(AllJobs.LockPicking,whichScript.Cost, 10);
            }
            else if (entropyID == 3)
            {
                rollingControl.checkIfCanReroll(AllJobs.NetNinja, whichScript.Cost, 3);
            }
            else if (extendSive.Contains(entropyID))
            {
                if (missionRollController.setbeforeMission)
                {
                    if (entropyID == 20)
                    {
                        if (userArea.users[0].characterScript.find_which(AllJobs.Crypt)>= 12)
                        {
                            rollingControl.addSkillEffector(AllJobs.Crypt, turnManager.RoundNumber, 1);
                        }
                        else
                        {
                            rollingControl.addSkillEffector(AllJobs.Crypt, turnManager.RoundNumber,( 12 - userArea.users[0].characterScript.find_which(AllJobs.Crypt)));
                        }
                    }
                    else if (entropyID == 21)
                    {
                        if (userArea.users[0].characterScript.find_which(AllJobs.HardHack) >= 12)
                        {
                            rollingControl.addSkillEffector(AllJobs.HardHack, turnManager.RoundNumber, 1);
                        }
                        else
                        {
                            rollingControl.addSkillEffector(AllJobs.HardHack, turnManager.RoundNumber, (12 - userArea.users[0].characterScript.find_which(AllJobs.HardHack)));
                        }
                    }
                    else if (entropyID == 22)
                    {
                        if (userArea.users[0].characterScript.find_which(AllJobs.NetNinja) >= 12)
                        {
                            rollingControl.addSkillEffector(AllJobs.NetNinja, turnManager.RoundNumber, 1);
                        }
                        else
                        {
                            rollingControl.addSkillEffector(AllJobs.NetNinja, turnManager.RoundNumber, (12 - userArea.users[0].characterScript.find_which(AllJobs.NetNinja)));
                        }
                    }
                    else if (entropyID == 23)
                    {
                        if (userArea.users[0].characterScript.find_which(AllJobs.SocialEng) >= 12)
                        {
                            rollingControl.addSkillEffector(AllJobs.SocialEng, turnManager.RoundNumber, 1);
                        }
                        else
                        {
                            rollingControl.addSkillEffector(AllJobs.SocialEng, turnManager.RoundNumber, (12 - userArea.users[0].characterScript.find_which(AllJobs.SocialEng)));
                        }
                    }
                    else if (entropyID == 24)
                    {
                        if (userArea.users[0].characterScript.find_which(AllJobs.SoftWiz) >= 12)
                        {
                            rollingControl.addSkillEffector(AllJobs.SoftWiz, turnManager.RoundNumber, 1);
                        }
                        else
                        {
                            rollingControl.addSkillEffector(AllJobs.SoftWiz, turnManager.RoundNumber, (12 - userArea.users[0].characterScript.find_which(AllJobs.SoftWiz)));
                        }
                    }
                }
                else if (missionRollController.setAfterMission)
                {
                    if (entropyID == 20)
                    {
                        rollingControl.convertFailedToPass(AllJobs.Crypt, whichScript.Cost, whichScript);
                    }
                    else if (entropyID == 21)
                    {
                        rollingControl.convertFailedToPass(AllJobs.HardHack, whichScript.Cost, whichScript);
                    }
                    else if (entropyID == 22)
                    {
                        rollingControl.convertFailedToPass(AllJobs.NetNinja, whichScript.Cost, whichScript);
                    }
                    else if (entropyID == 23)
                    {
                        rollingControl.convertFailedToPass(AllJobs.SocialEng, whichScript.Cost, whichScript);
                    }
                    else if (entropyID == 24)
                    {
                        rollingControl.convertFailedToPass(AllJobs.SoftWiz, whichScript.Cost, whichScript);
                    }
                }
            }
            else if (entropyID == 25)
            {
                object[] entropyOBJData = new object[] { entropyID };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.lightningRoll, entropyOBJData, new RaiseEventOptions() { TargetActors = new int[] { turnManager.PlayerIdToMakeThisTurn } }, SendOptions.SendReliable);
            }
            else if (entropyID == 26)
            {
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.skillChangeID26, null, new RaiseEventOptions() { TargetActors = new int[] { turnManager.PlayerIdToMakeThisTurn } }, SendOptions.SendReliable);
            }
            else if (entropyID == 27) 
            {
                object[] entropyOBJData = new object[] { entropyID };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.lightningRoll, entropyOBJData, new RaiseEventOptions() { TargetActors = new int[] { turnManager.PlayerIdToMakeThisTurn } }, SendOptions.SendReliable);
            }
            else if (entropyID == 28)
            {
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.cancelMissionID28, null, new RaiseEventOptions() { TargetActors = new int[] { turnManager.PlayerIdToMakeThisTurn } }, SendOptions.SendReliable);
            }
            else if (entropyID == 29)
            {
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.goZeroMoneyID29, null, new RaiseEventOptions() { TargetActors = new int[] { turnManager.PlayerIdToMakeThisTurn } }, SendOptions.SendReliable);
            }
            else if (entropyID == 30)
            {
                object[] Amount = new object[] { 2 };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.shareFate, Amount, EventManager.AllPeople, SendOptions.SendReliable);
            }
            else if (entropyID == 31)
            {
                object[] Amount = new object[] { -2 };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.shareFate, Amount, EventManager.AllPeople, SendOptions.SendReliable);
            }
            if (turnManager.IsMyTurn)
            {
                object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " has played " + whichScript.name+".", null, false };
                PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
            }
            else
            {
                if (entropyID == 30)
                {
                    object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " has played share fate " + whichScript.name + " all people cred +2.", null, false };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
                }
                else if (entropyID == 31)
                {
                    object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " has played share fate " + whichScript.name + " all people cred -2.", null, false };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
                }
                else
                {
                    object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " has played attack "+userArea.users[userArea.findPlayerPosition(turnManager.PlayerIdToMakeThisTurn)].Nickname +" with card " + whichScript.name +".", null, false };
                    PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManager.AllPeople, SendOptions.SendReliable);
                }
            }
            if (whichScript.removeAfterPlay)
            {
                drawEntropy.removeAnEntropyCard(whichScript,false);
            }
        }
        public void lightningRoll(int whichCard)
        {
            lightningRollOBJs.SetActive(true);
            entropyRollStrike = entropyCardDeck.cardDeck[whichCard - 1];
            entropyRollCard.GetComponent<Image>().sprite = entropyRollStrike.artwork_info;
            rollButtonEntropy.SetActive(true);
            if (whichCard == 25)
            {
                whichSkillAgainst.GetComponent<Text>().text = GetStringOfTask.get_string_of_job(AllJobs.SocialEng);
                setamountNeededToRoll = userArea.users[0].characterScript.find_which(AllJobs.SocialEng);
            }
            else if (whichCard == 27)
            {
                whichSkillAgainst.GetComponent<Text>().text = GetStringOfTask.get_string_of_job(AllJobs.HardHack);
                setamountNeededToRoll = userArea.users[0].characterScript.find_which(AllJobs.HardHack);
            }
            object[] entropyLightningRollJData = new object[] { whichCard, whichSkillAgainst.GetComponent<Text>().text , setamountNeededToRoll };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendLightingStrikeRoll, entropyLightningRollJData, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
        }
        public void clickOnRollButtonlightningRoll()
        {
            System.Random rand = new System.Random((int)DateTime.Now.Ticks);
            int x = rand.Next(0, 18);
            amountRolled.GetComponent<Text>().text = x.ToString();
            object[] entropyLightningRolledJData = new object[] { amountRolled.GetComponent<Text>().text };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.sendLightingStrikeRolled, entropyLightningRolledJData, EventManager.AllOtherThanMePeopleOptions, SendOptions.SendReliable);
            if (x < amountNeededToRoll)
            {
                if (entropyRollStrike.EntropyCardID == 25)
                {
                    userArea.subMyCred(1);
                    lightningRollOBJs.SetActive(false);
                }
                else
                {
                    rollingControl.onClickEndTurnButton();
                }
            }
            lightningRollOBJs.SetActive(false);
        }
        public void onReceiveSomeoneLightningRoll(int whichCard,string whichSkill, int whichAmount)
        {
            lightningRollOBJs.SetActive(true);
            rollButtonEntropy.SetActive(false);
            entropyRollCard.GetComponent<Image>().sprite = entropyCardDeck.cardDeck[whichCard - 1].artwork_info;
            setamountNeededToRoll = whichAmount;
            whichSkillAgainst.GetComponent<Text>().text = whichSkill;
        }
        public void onReceiveRolled(string amountRolledText)
        {
            amountRolled.GetComponent<Text>().text = amountRolledText;
            lightningRollOBJs.SetActive(false);
        }
        public void onPlayRollReset() => lightningRollOBJs.SetActive(false);
    }
}

