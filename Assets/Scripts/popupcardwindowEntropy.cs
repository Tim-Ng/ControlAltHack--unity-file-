using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupcardwindowEntropy : MonoBehaviour
{
    public GameObject PopUpCard;
    public GameObject EntropyCard;
    public EntropyCardDisplay EntropyDisplayUI;
    private EntropyCardScript pop_input_Entropy;
    public GameObject playCardButtonOBJ;
    public Button playCardButton;
    private bool canPlay = true;

    private void Start()
    {
        PopUpCard.SetActive(true);
    }
    private void Update()
    {
        playCardButton.enabled = canPlay;
    }
    public void openEntropyCard(EntropyCardScript input_EntropyCard)
    {
        Start();
        pop_input_Entropy = input_EntropyCard;
        EntropyDisplayUI.entropyData = pop_input_Entropy;
        EntropyDisplayUI.InfoSide.SetActive(true);
        EntropyDisplayUI.FrontSide.SetActive(false);
    }
    public void closePopup()
    {
        PopUpCard.SetActive(false);
        EntropyDisplayUI.InfoSide.SetActive(false);
    }
    public EntropyCardScript GetEntropyCardScript()
    {
        return pop_input_Entropy;
    }
}