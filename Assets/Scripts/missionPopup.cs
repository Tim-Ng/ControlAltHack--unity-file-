using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserAreas;
using TradeScripts;

namespace DrawCards {
    public class missionPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popUp = null, missionCardInpopUp = null,cardCharInpopUp = null, exitButton = null,selectButton =null,attendOrNot = null,missionStuffs = null,infoStuffs = null;
        [SerializeField] private GameObject attendanceInfo = null;

        private GameObject ScriptsODJ = null;
        private UserAreaControlers userAreaControlers = null;
        private TradeControler tradeController = null;
        private drawMissionCard drawMission = null;
        private EventHandeler EventManger = null;
        private TurnManager turnManager= null;
        public bool AttendingOrNot = false;
        private MissionCardScript whichScript = null;
        private missionDisplay whichCard = null;
        private void Start()
        {
            ScriptsODJ = gameObject;
            userAreaControlers = ScriptsODJ.GetComponent<UserAreaControlers>();
            EventManger = ScriptsODJ.GetComponent<EventHandeler>();
            drawMission = ScriptsODJ.GetComponent<drawMissionCard>();
            tradeController = ScriptsODJ.GetComponent<TradeControler>();
            turnManager = ScriptsODJ.GetComponent<TurnManager>();
        }
        public void closePopUp()
        {
            popUp.SetActive(false);
        }
        public void clickOnCard(missionDisplay missionDisplay, int whichPerson, bool noExit)
        {
            whichCard = missionDisplay;
            clickOnCard(whichCard.getInfo(), whichPerson, noExit);
        }
        public void clickOnCard(MissionCardScript missionScript,int whichPerson ,bool noExit)
        {
            popUp.SetActive(true);
            missionStuffs.SetActive(true);
            infoStuffs.SetActive(false);
            whichScript = missionScript;
            missionCardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
            cardCharInpopUp.GetComponent<Image>().sprite = userAreaControlers.users[whichPerson].characterScript.artwork_front_info;
            if (noExit && whichCard.cardAttendInRound != turnManager.RoundNumber)
            {
                if (userAreaControlers.users[0].MissionCards >= 2)
                {
                    exitButton.SetActive(true);
                    selectButton.SetActive(true);
                    attendOrNot.SetActive(false);
                }
                else
                {
                    exitButton.SetActive(false);
                    selectButton.SetActive(false);
                    attendOrNot.SetActive(true);
                    userAreaControlers.users[0].missionScript = whichScript;
                }
            }
            else
            {
                exitButton.SetActive(true);
                selectButton.SetActive(false);
                attendOrNot.SetActive(false);
            }
        }
        public void selectThisMission()
        {
            userAreaControlers.users[0].missionScript = whichScript;
            drawMission.removeOtherThanMissionCard(whichScript);
            exitButton.SetActive(false);
            selectButton.SetActive(false);
            attendOrNot.SetActive(true);
        }
        public void clickOnAttend()
        {
            popUp.SetActive(false);
            AttendingOrNot = true;
            whichCard.cardAttendInRound = turnManager.RoundNumber;
            userAreaControlers.setandsendIfAttending();
            tradeController.setAllAreas();
            object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " is attending this meeting. ", null, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManger.AllPeople, SendOptions.SendReliable);
        }
        public void clickOnNotAttend()
        {
            popUp.SetActive(false);
            AttendingOrNot = false;
            whichCard.cardAttendInRound = turnManager.RoundNumber;
            userAreaControlers.setandsendIfNotAttending(false);
            object[] player = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setWaiting, player, EventManger.AllPeople, SendOptions.SendReliable);
            object[] chatInfo = new object[] { PhotonNetwork.LocalPlayer.NickName + " is not attending this meeting. ", null, false };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.forChat, chatInfo, EventManger.AllPeople, SendOptions.SendReliable);
        }
        public void onClickInfoPopUp(bool openOrClose) => attendanceInfo.SetActive(openOrClose);
    }
}
