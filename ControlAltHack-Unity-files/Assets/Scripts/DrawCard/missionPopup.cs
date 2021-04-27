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
    /// <summary>
    /// This class control the mission popup 
    /// </summary>
    public class missionPopup : MonoBehaviour
    {
        ///<summary>
        ///This are the game object that is in the Charater Card Popup area.
        /// </summary>
        [SerializeField] private GameObject popUp = null, missionCardInpopUp = null,cardCharInpopUp = null, exitButton = null,selectButton =null,attendOrNot = null,missionStuffs = null,infoStuffs = null;
        ///<summary>
        ///This are the game object that is in the Charater Card Popup area.
        /// </summary>
        [SerializeField] private GameObject attendanceInfo = null;

        /// <summary>
        /// The game object that this script is attatched to
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// To hold the script to of UserAreaControlers
        /// </summary>
        private UserAreaControlers userAreaControlers = null;
        /// <summary>
        /// To hold the script to of TradeControler
        /// </summary>
        private TradeControler tradeController = null;
        /// <summary>
        /// To hold the script to of drawMissionCard
        /// </summary>
        private drawMissionCard drawMission = null;
        /// <summary>
        /// To hold the script to of EventHandeler
        /// </summary>
        private EventHandeler EventManger = null;
        /// <summary>
        /// To hold the script to of TurnManager
        /// </summary>
        private TurnManager turnManager= null;
        /// <summary>
        /// This is the indicator to state if this player is attending the meeting or not
        /// </summary>
        /// <remarks>
        /// If true then is attending <br/>
        /// Else is not attending
        /// </remarks>
        [HideInInspector]
        public bool AttendingOrNot = false;
        /// <summary>
        /// This is to hold the current mission card data  
        /// </summary>
        private MissionCardScript whichScript = null;
        /// <summary>
        /// This is to hold the script of which missionDisplay script had open this
        /// </summary>
        private missionDisplay whichCard = null;
        /// <summary>
        /// When the script is loaded this function will fill in the data for the scripts that we this class needs
        /// </summary>
        private void Start()
        {
            ScriptsODJ = gameObject;
            userAreaControlers = ScriptsODJ.GetComponent<UserAreaControlers>();
            EventManger = ScriptsODJ.GetComponent<EventHandeler>();
            drawMission = ScriptsODJ.GetComponent<drawMissionCard>();
            tradeController = ScriptsODJ.GetComponent<TradeControler>();
            turnManager = ScriptsODJ.GetComponent<TurnManager>();
        }
        /// <summary>
        /// This function is used to close the mission popup 
        /// </summary>
        public void closePopUp() { popUp.SetActive(false); }
        /// <summary>
        /// This function is to open the mission pop up with the input of missionDisplay.
        /// </summary>
        /// <remarks>
        /// This is used for when you click on the card <br/>
        /// Since this function is the same for the other clickOnCard so it will pass to the next one
        /// </remarks>
        /// <param name="missionDisplay">The script of the missionDisplay (card)</param>
        /// <param name="whichPerson">The person who has this mission which is usually the current player so (0)</param>
        /// <param name="noExit">If you can exit or not <br/> true then cannot exit else you can</param>
        public void clickOnCard(missionDisplay missionDisplay, int whichPerson, bool noExit)
        {
            whichCard = missionDisplay;
            clickOnCard(whichCard.getInfo(), whichPerson, noExit);
        }
        /// <summary>
        /// This function is to open the mission pop up with the input of missionCardScript.
        /// </summary>
        /// <remarks>
        /// This is mostly used when another person's mission card is click on [during mission roll / during trading] 
        /// </remarks>
        /// <param name="missionScript">The infomation of mission card</param>
        /// <param name="whichPerson">The person who holds the card</param>
        /// <param name="noExit">If you can exit or not <br/> true then cannot exit else you can</param>
        public void clickOnCard(MissionCardScript missionScript,int whichPerson ,bool noExit)
        {
            popUp.SetActive(true);
            missionStuffs.SetActive(true);
            infoStuffs.SetActive(false);
            whichScript = missionScript;
            missionCardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
            cardCharInpopUp.GetComponent<Image>().sprite = userAreaControlers.users[whichPerson].characterScript.artwork_front_info;
            if (noExit && (whichCard != null ? whichCard.cardAttendInRound != turnManager.RoundNumber : true))
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
        /// <summary>
        /// This function is called when the button to select this mission card is pressed.
        /// </summary>
        /// <remarks>
        /// This is usally only for the person with the caracter card with the ID of 7 [GABRIEL] when he can draw 2 mission cards 
        /// </remarks>
        public void selectThisMission()
        {
            userAreaControlers.users[0].missionScript = whichScript;
            drawMission.removeOtherThanMissionCard(whichScript);
            exitButton.SetActive(false);
            selectButton.SetActive(false);
            attendOrNot.SetActive(true);
        }
        /// <summary>
        /// This is when the button to attend the meeting is pressed on 
        /// </summary>
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
        /// <summary>
        /// This is when the button to skip the meeting is pressed on
        /// </summary>
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
        /// <summary>
        /// This is to open/close the attending meeting info popup 
        /// </summary>
        /// <param name="openOrClose"> If true then open else close</param>
        public void onClickInfoPopUp(bool openOrClose) => attendanceInfo.SetActive(openOrClose);
    }
}
