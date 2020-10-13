using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class popupcardwindow : MonoBehaviour 
{
    public GameObject CharactercardOBJ;
    public CharacterCardDispaly CharactercardUI;
    public GameObject PopUpCard;
    public GameObject SelectChar;

    private void Start()
    {
        PopUpCard.SetActive(true);
    }
    public void openCharacterCard(CharCardScript input_CharacterCard)
    {
        CharactercardOBJ.SetActive(true);
        SelectChar.SetActive(true);
        CharactercardUI.CharCard = input_CharacterCard;
        CharactercardUI.InfoSide.SetActive(true);
    }
    public void closePopup()
    {
        PopUpCard.SetActive(false);
        CharactercardOBJ.SetActive(false);
        SelectChar.SetActive(false);
        CharactercardUI.InfoSide.SetActive(false);
    }
}
