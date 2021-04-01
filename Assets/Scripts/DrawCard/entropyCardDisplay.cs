using main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawCards
{
    /// <summary>
    /// To render the image and function of an Entropy Cards.
    /// </summary>
    public class entropyCardDisplay : MonoBehaviour
    {
        /// <summary>
        /// This is to hold the data of the Entropy card that the object of this script holds
        /// </summary>
        private EntropyCardScript infoEntro = null;
        /// <summary>
        /// To hold the game object of back of the card
        /// </summary>
        [SerializeField] private GameObject BackSide = null;
        /// <summary>
        /// To hold the game object of front of the card
        /// </summary>
        [SerializeField] private GameObject InfoSide = null;
        
        /// <summary>
        /// Hold the gameObject of gameInterface
        /// </summary>
        private GameObject gameInterfaceOBJ = null;
        /// <summary>
        /// The turn where this card was last played
        /// </summary>
        private int turnPlayed = 0;
        /// <summary>
        /// To see if the card has popup or not, this is when entropy card can be played
        /// </summary>
        private bool HasChangePosition= false;
        /// <summary>
        /// This function is run then the script loaded.
        /// Then the gameInterfaceOBJ is found 
        /// </summary>
        private void Start()
        {
            gameInterfaceOBJ = GameObject.Find("/MainGame/Game Interface");
        }
        /// <summary>
        /// Each frame the script will check if this card type can be played and will pop it up if it is playable 
        /// </summary>
        private void Update()
        {
            float posX=gameObject.GetComponent<RectTransform>().position.x;
            float posY=gameObject.GetComponent<RectTransform>().position.y;
            if (gameInterfaceOBJ.GetComponent<entropyCardPopup>().checkIfCanPlay(infoEntro, turnPlayed))
            {
                if (!HasChangePosition)
                {
                    gameObject.GetComponent<RectTransform>().position = new Vector2(posX, posY+10f);
                    HasChangePosition = true;
                }
            }
            else
            {
                if (HasChangePosition)
                {
                    gameObject.GetComponent<RectTransform>().position = new Vector2(posX, posY-10f);
                    HasChangePosition = false;
                }
            }
        }
        /// <summary>
        /// To set the current entropy Card Data with an input
        /// </summary>
        /// <remarks>
        /// Uses the static list from entropyCardDeck script to get the data.
        /// </remarks>
        /// <param name="which">
        /// This is the param to tell which entorpy card it is.
        /// </param>
        public void setID(int which)
        {
            infoEntro = entropyCardDeck.cardDeck[which-1];
            BackSide.GetComponent<Image>().sprite = infoEntro.artwork_back;
            InfoSide.GetComponent<Image>().sprite = infoEntro.artwork_info;
            setBackSide(true);
            setInfoSide(false);
        }
        /// <summary>
        /// To get the EntropyCardScript of this script 
        /// </summary>
        /// <returns>
        /// Return the value of the infoEntro [EntropyCardScript]
        /// </returns>
        public EntropyCardScript getInfo()
        {
            return infoEntro;
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
        /// When the front side of card is click on
        /// </summary>
        /// <remarks>
        /// To open the entropy Card Popup
        /// </remarks>
        public void clickOnInfo() => gameInterfaceOBJ.GetComponent<entropyCardPopup>().opendCharCard(infoEntro, this, turnPlayed);
        /// <summary>
        /// If this card is play then the turnPlayed will be update to the current round
        /// </summary>
        public void ifThisIsPlayed() => turnPlayed = gameInterfaceOBJ.GetComponent<TurnManager>().RoundNumber;
        /// <summary>
        /// To set the back of the card is visible or not
        /// </summary>
        /// <param name="TorF">
        /// True  = Shown
        /// False = Not shown
        /// </param>
        public void setBackSide(bool TorF) { BackSide.SetActive(TorF); }
        /// <summary>
        /// To set the front of the card is visible or not
        /// </summary>
        /// <param name="TorF">
        /// True  = Shown
        /// False = Not shown
        /// </param>
        public void setInfoSide(bool TorF) { InfoSide.SetActive(TorF);  }
        
    }
}
