using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawCards
{
    public class characterCardDisplay : MonoBehaviour
    {
        private CharCardScript infoChar = null;
        [SerializeField] private GameObject BackSide = null, InfoSide = null;
        public void setID(int which)
        {
            infoChar =charCardDeck.cardDeck[which];
            BackSide.GetComponent<Image>().sprite = infoChar.artwork_back;
            InfoSide.GetComponent<Image>().sprite = infoChar.artwork_back;
            setBackSide(true);
            setInfoSide(false);
        }
        public CharCardScript getInfo()
        {
            return infoChar;
        }
        public void clickOnBackSide()
        {
            setBackSide(false);
            setInfoSide(true);
        }
        public void clickOnInfo()
        {
            GameObject popUp = GameObject.Find("/MainGame/Game Interface");
            popUp.GetComponent<CharCardPopup>().opendCharCard(infoChar);
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
