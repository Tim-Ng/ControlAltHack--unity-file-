using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawCards
{
    public class characterCardDisplay : MonoBehaviour
    {
        private CharCardScript infoChar;
        [SerializeField] private GameObject BackSide, InfoSide;
        public void setID(int which)
        {
            infoChar = drawCharacterCard.cardDeck[which];
            BackSide.GetComponent<Image>().sprite = infoChar.artwork_back;
            InfoSide.GetComponent<Image>().sprite = infoChar.artwork_back;
            setBackSide(true);
            setInfoSide(false);
        }
        public void clickOnBackSide()
        {
            setBackSide(false);
            setInfoSide(true);
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
