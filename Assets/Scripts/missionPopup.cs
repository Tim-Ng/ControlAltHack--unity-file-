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
        [SerializeField] private UserAreaControlers userAreaControlers = null;
        [SerializeField] private GameObject popUp = null, missionCardInpopUp = null,cardCharInpopUp = null, exitButton = null,selectButton =null,attendOrNot = null,missionStuffs = null,infoStuffs = null;
        [SerializeField] private TradeControler tradeControler=null;
        [SerializeField] private TurnManager turnManager = null;
        [SerializeField] private drawMissionCard drawMission = null;
        [SerializeField] private EventHandeler EventManger = null;
        private MissionCardScript whichScript = null;
        public void closePopUp()
        {
            popUp.SetActive(false);
        }
        public void clickOnCard(MissionCardScript missionScript,int whichPerson )
        {
            popUp.SetActive(true);
            missionStuffs.SetActive(true);
            infoStuffs.SetActive(false);
            whichScript = missionScript;
            missionCardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
            cardCharInpopUp.GetComponent<Image>().sprite = userAreaControlers.users[whichPerson].characterScript.artwork_front_info;
            if (turnManager.TurnNumber % 2 == 1)
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
        public void clickOnCard(int missionID, int whichPerson)
        {
            popUp.SetActive(true);
            missionStuffs.SetActive(true);
            infoStuffs.SetActive(false);
            whichScript = missionCardDeck.cardDeck[missionID-1];
            missionCardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
            cardCharInpopUp.GetComponent<Image>().sprite = userAreaControlers.users[whichPerson].characterScript.artwork_front_info;
            exitButton.SetActive(true);
            selectButton.SetActive(false);
            attendOrNot.SetActive(false);
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
            userAreaControlers.setandsendIfAttending();
            tradeControler.setAllAreas();
        }
        public void clickOnNotAttend()
        {
            popUp.SetActive(false);
            object[] player = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
            PhotonNetwork.RaiseEvent((byte)PhotonEventCode.setWaiting, player, EventManger.AllPeople, SendOptions.SendReliable);
        }
        public void clickOnInfo()
        {

        }
    }
}
