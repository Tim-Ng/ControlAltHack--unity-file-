using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawCards
{
    public class missionDisplay : MonoBehaviour
    {
        private MissionCardScript infoMission = null;
        [SerializeField] private GameObject BackSide = null, InfoSide = null;
        public void setID(int which)
        {
            infoMission = missionCardDeck.cardDeck[which - 1];
            BackSide.GetComponent<Image>().sprite = infoMission.artwork_back;
            InfoSide.GetComponent<Image>().sprite = infoMission.artwork_front_info;
            setBackSide(true);
            setInfoSide(false);
        }
        public MissionCardScript getInfo()
        {
            return infoMission;
        }
        public void clickOnBackSide()
        {
            setBackSide(false);
            setInfoSide(true);
        }
        public void clickOnInfo()
        {
            GameObject popUp = GameObject.Find("/MainGame/Game Interface");
            popUp.GetComponent<missionPopup>().clickOnCard(infoMission,0);
        }
        public void setBackSide(bool TorF)
        {
            BackSide.SetActive(TorF);
        }
        public void setInfoSide(bool TorF)
        {
            InfoSide.SetActive(TorF);
        }
    }
}
