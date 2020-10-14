using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCardButtons : MonoBehaviour
{
    public GameObject cardFront;
    public CharacterCardDispaly myParent;
    private GameObject PopUp;
    private popupcardwindowChar popUpScript;
    private CharCardScript CharCardInfo;
    private void Start()
    {
        PopUp = GameObject.Find("/MainGame/Game Interface/PopUpBackGroundChar");
        popUpScript = PopUp.GetComponent<popupcardwindowChar>();
    }
    public void clickOnChar()
    {
        PopUp.SetActive(true);
        CharCardInfo = myParent.CharCard;
        popUpScript.openCharacterCard(CharCardInfo,true);
    }
}
