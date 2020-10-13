using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButtons : MonoBehaviour
{
    public GameObject cardFront;
    public CharacterCardDispaly myParent;
    public GameObject PopUp;
    private popupcardwindow popUpScript;
    private CharCardScript CharCardInfo;
    private void Start()
    {
        PopUp = GameObject.Find("/MainGame/Game Interface/PopUpBackGround");
        popUpScript = PopUp.GetComponent<popupcardwindow>();
    }
    public void clickOnChar()
    {
        
        PopUp.SetActive(true);
        CharCardInfo = myParent.CharCard;
        popUpScript.openCharacterCard(CharCardInfo);
    }
}
