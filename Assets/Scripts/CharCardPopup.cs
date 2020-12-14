using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using ExitGames.Client.Photon;
using main;
using UserAreas;
using UnityEngine.UI;

namespace DrawCards
{
    public class CharCardPopup : MonoBehaviour
    {
        [SerializeField] private UserAreaControlers userAreaControlers = null;
        [SerializeField] private GameObject popUp = null,cardInpopUp = null,buttonSelect;
        private CharCardScript whichScript;
        public void opendCharCard(CharCardScript info)
        {
            buttonSelect.SetActive(true);
            popUp.SetActive(true);
            whichScript = info;
            cardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
        }
        public void closePopUp()
        {
            popUp.SetActive(false);
        }
        public void selectThisChar()
        {
            userAreaControlers.setMyCharacter(whichScript);
            closePopUp();
        }
        public void clickOnAvertar(int which)
        {
            buttonSelect.SetActive(false);
            popUp.SetActive(true);
            whichScript = userAreaControlers.users[which].characterScript;
            cardInpopUp.GetComponent<Image>().sprite = whichScript.artwork_front_info;
        }
    }
}
