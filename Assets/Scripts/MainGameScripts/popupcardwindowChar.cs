using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class popupcardwindowChar : MonoBehaviour
{
    public GameObject CharactercardOBJ;
    public CharacterCardDispaly CharactercardUI;
    private CharCardScript pop_input_CharacterCard;
    public GameObject PopUpCard;
    public GameObject SelectChar;


    //allow popup gameobjet to open 
    private void Start()
    {
        PopUpCard.SetActive(true);
    }


    //CHARACTER CARD
    public void openCharacterCard(CharCardScript input_CharacterCard, bool yesNOselect)
    {
        Start();
        pop_input_CharacterCard = input_CharacterCard;
        if (yesNOselect)
        {
            SelectChar.SetActive(true);
        }
        else
        {
            SelectChar.SetActive(false);
        }
        CharactercardUI.CharCard = pop_input_CharacterCard;
        CharactercardUI.FrontSide.SetActive(false);
        CharactercardUI.InfoSide.SetActive(true);
    }
    //get the character script
    public CharCardScript GetCharCardScript()
    {
        return pop_input_CharacterCard;
    }

    //end popup
    public void closePopup()
    {
        PopUpCard.SetActive(false);
        SelectChar.SetActive(false);
        CharactercardUI.InfoSide.SetActive(false);
    }

}
