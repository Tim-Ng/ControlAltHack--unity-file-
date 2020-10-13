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
    private CharCardScript pop_input_CharacterCard;
    public GameObject PopUpCard;
    public GameObject SelectChar;
    public GameObject playerArea;
    public GameObject playerAvartar;

    private void Start()
    {
        PopUpCard.SetActive(true);
    }
    public void openCharacterCard(CharCardScript input_CharacterCard,bool yesNOselect)
    {
        Start();
        pop_input_CharacterCard = input_CharacterCard;
        if (yesNOselect)
        {
            SelectChar.SetActive(true);
        }
        CharactercardUI.CharCard = input_CharacterCard;
        CharactercardUI.FrontSide.SetActive(false);
        CharactercardUI.InfoSide.SetActive(true);
    }
    public void closePopup()
    {
        PopUpCard.SetActive(false);
        SelectChar.SetActive(false);
        CharactercardUI.InfoSide.SetActive(false);
    }
    public CharCardScript GetCharCardScript()
    {
        return pop_input_CharacterCard;
    }

    // for select character

}
