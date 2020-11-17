﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupcardwindowEntropy : MonoBehaviour
{
    public GameObject PopUpCard;
    public GameObject EntropyCard;
    public EntropyCardDisplay EntropyDisplayUI;
    private EntropyCardScript pop_input_Entropy;
    [SerializeField] private DrawCharacterCard drawCharacterCard;
    public GameObject playCardButtonOBJ;
    private bool canPlay = false;
    private bool canAttack = false;
    private bool canPlayBackOfTricks = false;

    private void Start()
    {
        PopUpCard.SetActive(true);
    }
    public void setBoolcanPlay(bool YesOrNo)
    {
        canPlay = YesOrNo;
    }
    public void setBoolcanAttack(bool YesOrNo)
    {
        canAttack = YesOrNo;
    }
    public void setBoolcanPlayBackOfTricks(bool YesOrNo)
    {
        canPlayBackOfTricks = YesOrNo;
    }
    public void openEntropyCard(EntropyCardScript input_EntropyCard)
    {
        Start();
        pop_input_Entropy = input_EntropyCard;
        EntropyDisplayUI.entropyData = pop_input_Entropy;
        EntropyDisplayUI.setUpdate();
        if (pop_input_Entropy.IsBagOfTricks && drawCharacterCard.IsMyTurn && canPlay && canPlayBackOfTricks)
        {
            playCardButtonOBJ.SetActive(true);
        }
        else if (pop_input_Entropy.IsLigthingStrikes && !drawCharacterCard.IsMyTurn && canAttack &&canPlay)
        {
            playCardButtonOBJ.SetActive(true);
        }
        else
        {
            playCardButtonOBJ.SetActive(false);
        }
        EntropyDisplayUI.InfoSide.SetActive(true);
        EntropyDisplayUI.FrontSide.SetActive(false);
    }
    public void closePopup()
    {
        PopUpCard.SetActive(false);
        playCardButtonOBJ.SetActive(false);
        EntropyDisplayUI.InfoSide.SetActive(false);
    }
    public EntropyCardScript GetEntropyCardScript()
    {
        return pop_input_Entropy;
    }
}