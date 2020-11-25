using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCardButton : MonoBehaviour
{
    public GameObject FrontSide;
    public GameObject InfoSide;
    public MissionCardDisplay myparent;
    private GameObject MainScript;
    private DrawCharacterCard drawCharacterCard;
    private GameObject PopUp;
    private popupcardwindowMission popUpScript;
    private CharCardScript CharCardInfo;
    private MissionCardScript MissionCardInfo;
    [SerializeField] rollTime RollTime;

    private void Start()
    {
        MainScript = GameObject.Find("/MainGame/Game Interface/MainScript");
        PopUp = GameObject.Find("/MainGame/Game Interface/PopUpAllAttendOrNot");
        popUpScript = PopUp.GetComponent<popupcardwindowMission>();
        drawCharacterCard = MainScript.GetComponent<DrawCharacterCard>();
    }
    public void whenClickOnFront()
    {
        FrontSide.SetActive(false);
        InfoSide.SetActive(true);
    }
    public void whenClickOnInfo()
    {
        CharCardInfo = drawCharacterCard.getMyCharScript();
        MissionCardInfo = myparent.mission_script;
        drawCharacterCard.setCurrentMissionScript(MissionCardInfo);
        popUpScript.openMissionCard(MissionCardInfo, CharCardInfo);
    }
}
