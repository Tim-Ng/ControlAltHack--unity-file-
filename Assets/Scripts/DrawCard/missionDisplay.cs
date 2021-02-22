using main;
using UnityEngine;
using UnityEngine.UI;

namespace DrawCards
{
    /// <summary>
    /// To render the image and function of the Mission Cards.
    /// </summary>
    public class missionDisplay : MonoBehaviour
    {
        /// <summary>
        /// This is to hold the data of the Mission card that the object of this script holds
        /// </summary>
        private MissionCardScript infoMission = null;
        /// <summary>
        /// To hold the game object of back of the card
        /// </summary>
        [SerializeField] private GameObject BackSide = null;
        /// <summary>
        /// To hold the game object of front of the card
        /// </summary>
        [SerializeField] private GameObject InfoSide = null;
        /// <summary>
        /// The turn when the player attended with this card 
        /// </summary>
        private int roundAttend = 0;
        /// <summary>
        /// To get/set the roundAttend variable
        /// </summary>
        public int cardAttendInRound
        {
            get { return roundAttend; } set { roundAttend = value; }
        }
        /// <summary>
        /// To set the current mission Card Data with an input
        /// </summary>
        /// <remarks>
        /// Uses the static list from missionCardDeck script to get the data.
        /// </remarks>
        /// <param name="which">
        /// This is the param to tell which mission card it is.
        /// </param>
        public void setID(int which)
        {
            infoMission = missionCardDeck.cardDeck[which - 1];
            BackSide.GetComponent<Image>().sprite = infoMission.artwork_back;
            InfoSide.GetComponent<Image>().sprite = infoMission.artwork_front_info;
            setBackSide(true);
            setInfoSide(false);
        }
        /// <summary>
        /// This is to get the mission card data of this script
        /// </summary>
        /// <returns>
        /// The mission card data [infoMission] of this script [MissionCardScript]
        /// </returns>
        public MissionCardScript getInfo() { return infoMission;}
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
        /// To open the mission Card Popup <br/>
        /// If the turn number is and odd number then you cant exit the popup and you can only attend or not attend the meeting <br/>
        /// Else dont have button to attend meeting but only have exit 
        /// </remarks>
        public void clickOnInfo()
        {
            GameObject popUp = GameObject.Find("/MainGame/Game Interface");
            if (popUp.GetComponent <TurnManager>().TurnNumber %2 == 1)
                popUp.GetComponent<missionPopup>().clickOnCard(this, 0, true);
            else
                popUp.GetComponent<missionPopup>().clickOnCard(this, 0, false);
        }
        /// <summary>
        /// To set the back of the card is visible or not
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
        /// To set the front of the card is visible or not
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
