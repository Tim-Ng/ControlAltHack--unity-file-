using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawCards
{
    /// <summary>
    /// To render the image and function of the Character Cards.
    /// </summary>
    public class characterCardDisplay : MonoBehaviour
    {
        /// <summary>
        /// This is to hold the data of the character card that the object of this script holds
        /// </summary>
        private CharCardScript infoChar = null;
        /// <summary>
        /// To hold object of front and the back of the card
        /// </summary>
        [SerializeField] private GameObject BackSide = null, InfoSide = null;
        /// <summary>
        /// To set the current Character Card Data with an input
        /// </summary>
        /// <remarks>
        /// Uses the static list from charCardDeck script to get the data.
        /// </remarks>
        /// <param name="which">
        /// This is the param to tell which caracter card it is.
        /// </param>
        public void setID(int which)
        {
            infoChar =charCardDeck.cardDeck[which-1];
            BackSide.GetComponent<Image>().sprite = infoChar.artwork_back;
            InfoSide.GetComponent<Image>().sprite = infoChar.artwork_back;
            setBackSide(true);
            setInfoSide(false);
        }
        /// <summary>
        /// To flip the card when you click on the back of the card
        /// </summary>
        public void clickOnBackSide()
        {
            setBackSide(false);
            setInfoSide(true);
        }
        /// <summary>
        /// When click on the front side of the card [info side].
        /// </summary>
        public void clickOnInfo()
        {
            GameObject popUp = GameObject.Find("/MainGame/Game Interface");
            popUp.GetComponent<CharCardPopup>().opendCharCard(infoChar);
        }
        /// <summary>
        /// To set the back of the card is visible shown or not
        /// </summary>
        /// <param name="TorF"> 
        /// True  = Shown
        /// False = Not shown
        /// </param>
        public void setBackSide(bool TorF)
        {
            BackSide.SetActive(TorF);
        }
        /// <summary>
        /// To set the front of the card is visible shown or not
        /// </summary>
        /// <param name="TorF"> 
        /// True  = Shown
        /// False = Not shown
        /// </param>
        public void setInfoSide(bool TorF)
        {
            InfoSide.SetActive(TorF);
        }

    }
}
