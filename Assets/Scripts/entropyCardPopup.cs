using DrawCards;
using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using rollmissions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserAreas;

public class entropyCardPopup : MonoBehaviour
{
    [SerializeField] private GameObject popUp = null, cardInpopUpEntropy = null,playButton = null;
    [SerializeField] private drawEntropyCard drawEntro = null;
    [SerializeField] private DuringMissionRollController missionRollController = null;
    private entropyCardDisplay thisEntorpyCardDisplay = null;
    [SerializeField] private TurnManager turnManager = null;
    private EntropyCardScript  whichScript = null;
    private int[] Before = { 1,2,4,6,7,8,9,11,12,13,16,17,18,20,21,22,23,24};
    private int[] LightingStrike = { 25,26,27,28,29};
    private int[] After = { 3, 5, 10, 14, 15, 20, 2,1, 22, 23, 24 };
    public void opendCharCard(EntropyCardScript info, entropyCardDisplay EntorpyCardDisplay,int Roundnumber)
    {
        thisEntorpyCardDisplay = EntorpyCardDisplay;
        whichScript = info;
        playButton.SetActive(checkIfCanPlay(Roundnumber));
        cardInpopUpEntropy.GetComponent<Image>().sprite = whichScript.artwork_info;
        popUp.SetActive(true);
    }
    public bool checkIfCanPlay(int Roundnumber)
    {
        int checkValue = whichScript.EntropyCardID;
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
                        for (int i = 0; i < Before.Length; i++)
                        {
                            if (checkValue == Before[i])
                                return true;
                        }
                    }
                    else if (missionRollController.setAfterMission)
                    {
                        for (int i = 0; i < After.Length; i++)
                        {
                            if (checkValue == After[i])
                                return true;
                        }
                    }
                    else
                        return false;
                }
                else
                {
                    if (missionRollController.setbeforeMission)
                    {
                        for (int i = 0; i < LightingStrike.Length; i++)
                        {
                            if (checkValue == LightingStrike[i])
                                return true;
                        }
                    }
                    else
                        return false;
                }
            }
        }
        else
            return false;
        return false;
    }
    public void closePopUp()
    {
        popUp.SetActive(false);
    }
    public void clickOnDiscard()
    {
        closePopUp();
        drawEntro.removeAnEntropyCard(whichScript,true);
    }
    public void clickOnPlayButton()
    {
        thisEntorpyCardDisplay.ifThisIsPlayed();
    }
}
