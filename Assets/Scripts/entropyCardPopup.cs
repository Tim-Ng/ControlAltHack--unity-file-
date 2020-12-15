using DrawCards;
using ExitGames.Client.Photon;
using main;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserAreas;

public class entropyCardPopup : MonoBehaviour
{
    [SerializeField] private GameObject popUp = null, cardInpopUpEntropy = null,playButton = null;
    [SerializeField] private drawEntropyCard drawEntro = null;
    private EntropyCardScript  whichScript = null;
    public void opendCharCard(EntropyCardScript info,bool canPlayButton)
    {
        playButton.SetActive(canPlayButton);
        whichScript = info;
        cardInpopUpEntropy.GetComponent<Image>().sprite = whichScript.artwork_info;
        popUp.SetActive(true);
    }
    public void closePopUp()
    {
        popUp.SetActive(false);
    }
    public void clickOnDiscard()
    {
        closePopUp();
        drawEntro.removeAnEntropyCard(whichScript);
    }
}
