using main;
using rollmissions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserAreas;

namespace DrawCards
{
    /// <summary>
    /// This script is to control the entorpy popup
    /// </summary>
    public class entropyCardPopup : MonoBehaviour
    {
        /// <summary>
        /// These are the game object that are in the entorpy card pop up
        /// </summary>
        [SerializeField] private GameObject popUp = null, cardInpopUpEntropy = null, playButton = null;
        /// <summary>
        /// This is the script for the DuringMissionRollController [that controls the elements of the mission roll]
        /// </summary>
        [SerializeField] private DuringMissionRollController missionRollController = null;
        /// <summary>
        /// This holds the game object of the scripts needed are attached to
        /// </summary>
        private GameObject ScriptsODJ = null;
        /// <summary>
        /// This holds the script of drawEntropyCard
        /// </summary>
        private drawEntropyCard drawEntro = null;
        /// <summary>
        /// This holds the script of rollingMissionControl
        /// </summary>
        private rollingMissionControl rollingContoler = null;
        /// <summary>
        /// This holds the script of UserAreaControlers
        /// </summary>
        private UserAreaControlers userArea = null;
        /// <summary>
        /// This holds the script of playEntropyCard
        /// </summary>
        private playEntropyCard playEntropy = null;
        /// <summary>
        /// This holds the script of TurnManager
        /// </summary>
        private TurnManager turnManager = null;
        /// <summary>
        /// This holds the script of entropyCardDisplay
        /// </summary>
        private entropyCardDisplay thisEntorpyCardDisplay = null;
        /// <summary>
        /// This holds the script of EntropyCardScript
        /// </summary>
        private EntropyCardScript whichScript = null;
        /// <summary>
        /// This holds the list of entropy card can be played before mission roll
        /// </summary>
        private readonly List<int> Before = new List<int> { 1, 2, 4, 5, 6, 7, 8, 9, 11, 12, 13, 16, 17, 18, 20, 21, 22, 23, 24 };
        /// <summary>
        /// This holds the list of entropy card that is LightingStrike [attack other player's rolls] 
        /// </summary>
        private readonly List<int> LightingStrike = new List<int> { 25, 26, 27, 28, 29 };
        /// <summary>
        /// This holds the list of entropy card can be played after mission roll
        /// </summary>
        private readonly List<int> After = new List<int> { 3, 10, 14, 15, 20, 2, 1, 22, 23, 24 };
        /// <summary>
        /// This script is run when it is rendered.
        /// </summary>
        /// <remarks>
        /// This function will run and get the scripts to the variable that holds them. 
        /// </remarks>
        private void Start()
        {
            ScriptsODJ = gameObject;
            userArea = ScriptsODJ.GetComponent<UserAreaControlers>();
            playEntropy = ScriptsODJ.GetComponent<playEntropyCard>();
            rollingContoler = ScriptsODJ.GetComponent<rollingMissionControl>();
            turnManager = ScriptsODJ.GetComponent<TurnManager>();
            drawEntro = ScriptsODJ.GetComponent<drawEntropyCard>();
        }
        /// <summary>
        /// This function is used to open the entropy card pop up.
        /// </summary>
        /// <param name="info"> The info of the entropy card script</param>
        /// <param name="EntorpyCardDisplay">The entropy card display script of the entropy card. This is needed to tell when this card is played</param>
        /// <param name="Roundnumber">The current round number</param>
        public void opendCharCard(EntropyCardScript info, entropyCardDisplay EntorpyCardDisplay, int Roundnumber)
        {
            thisEntorpyCardDisplay = EntorpyCardDisplay;
            whichScript = info;
            playButton.SetActive(checkIfCanPlay(whichScript, Roundnumber));
            checkMoney();
            cardInpopUpEntropy.GetComponent<Image>().sprite = whichScript.artwork_info;
            popUp.SetActive(true);
        }
        /// <summary>
        /// This is to check if the player has enough money to play this card.
        /// </summary>
        /// <remarks>
        /// If not enought then the button is not interactable <br/>
        /// else the button is interactable
        /// </remarks>
        public void checkMoney()
        {
            if (userArea.users[0].amountOfMoney >= whichScript.Cost)
            {
                playButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                playButton.GetComponent<Button>().interactable = false;
            }
        }
        /// <summary>
        /// This is to check if this card can be played 
        /// </summary>
        /// <remarks>
        /// It takes into account if the card had been played or the current stage of the mission roll.
        /// </remarks>
        /// <param name="script">
        /// The script of the entropy data
        /// </param>
        /// <param name="Roundnumber">
        /// The round of which it was last played.
        /// </param>
        /// <returns>
        /// If the card can be played <br/>
        /// If true can play <br/>
        /// If flase then cant 
        /// </returns>
        public bool checkIfCanPlay(EntropyCardScript script, int Roundnumber)
        {
            int checkValue = script.EntropyCardID;
            if (turnManager.RoundNumber != Roundnumber)
            {
                if (checkValue == 30 || checkValue == 31)
                {
                    return true;
                }
                else
                {
                    if (turnManager.IsMyTurn)
                    {
                        if (missionRollController.setbeforeMission)
                        {
                            if (Before.Contains(checkValue))
                            {
                                return true;
                            }
                        }
                        else if (missionRollController.setAfterMission)
                        {
                            if (rollingContoler.CurrentMissionStatus == false)
                            {
                                if (After.Contains(checkValue))
                                {
                                    return true;
                                }
                            }
                        }
                        else
                            return false;
                    }
                    else
                    {
                        if (missionRollController.setbeforeMission)
                        {
                            if (LightingStrike.Contains(checkValue)){
                                return true;
                            }
                        }
                        else
                            return false;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// This function is to close the popup 
        /// </summary>
        public void closePopUp(){ popUp.SetActive(false); }
        /// <summary>
        /// This is to discard the entorpy card
        /// </summary>
        /// <remarks>
        /// Will sent the infomation to the function removeAnEntorpyCard to the drawEntropyCard srcipt. <br/>
        /// Then closes the pop up.
        /// </remarks>
        public void clickOnDiscard()
        {
            closePopUp();
            drawEntro.removeAnEntropyCard(whichScript, true);
        }
        /// <summary>
        /// This function is called when the play is clicked
        /// </summary>
        /// <remarks>
        /// The function ifThisIsPlayed() from the entropyCardDisplay script will be called <br/>
        /// The function onPlayEntropyCard( input of the current script ) from the playEntropyCard script<br/>
        /// The function subMyMoney( the cost of the current entropy card ) from UserControlArea script
        /// </remarks>
        public void clickOnPlayButton()
        {
            thisEntorpyCardDisplay.ifThisIsPlayed();
            playEntropy.onPlayEntropyCard(whichScript);
            userArea.subMyMoney(whichScript.Cost);
            closePopUp();
        }
    }
}