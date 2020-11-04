using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class popupcardwindowMission : MonoBehaviour
{
    public GameObject PopUpCard;
    public MissionCardDisplay missionCardDisplay;
    public CharacterCardDispaly charCardDisplay;
    private MissionCardScript pop_input_mission;
    private CharCardScript pop_input_character;

    public GameObject selectMissionCard;
    public GameObject UserMissionCardArea;

    public GameObject AttendOrNot;
    public GameObject textAttend;
    public GameObject textNotAttending;
    private bool LOOKONLY = false;
    private bool cardselected = false;
    private void Start()
    {
        PopUpCard.SetActive(true);
    }
    public void openMissionCard(MissionCardScript input_MissionCard, CharCardScript input_charCard)
    {
        Start();
        pop_input_mission = input_MissionCard;
        pop_input_character = input_charCard;
        if (input_charCard.character_code == 9 && !(cardselected))
        {
            AttendOrNot.SetActive(false);
            selectMissionCard.SetActive(true);
        }
        else
        {
            AttendOrNot.SetActive(true);
            selectMissionCard.SetActive(false);  
        }
        if (LOOKONLY)
        {
            AttendOrNot.SetActive(false);
        }
        missionCardDisplay.mission_script = pop_input_mission;
        missionCardDisplay.setUpdate();
        missionCardDisplay.InfoSide.SetActive(true);
        missionCardDisplay.FrontSide.SetActive(false);

        charCardDisplay.CharCard = pop_input_character;
        charCardDisplay.setUpdate();
        charCardDisplay.InfoSide.SetActive(true);
        charCardDisplay.FrontSide.SetActive(false);
    }
    public void closePopup()
    {
        PopUpCard.SetActive(false);
    }
    public MissionCardScript GetMissionCardScript()
    {
        return pop_input_mission;
    }
    public void clickOnSelectMission()
    {
        foreach (Transform child in UserMissionCardArea.transform)
        {
            if (!(child.GetComponent<MissionCardDisplay>().mission_script == pop_input_mission))
            {
                Destroy(child.gameObject);
            }
        }
        AttendOrNot.SetActive(true);
        selectMissionCard.SetActive(false);
        cardselected = true;
    }
    public void setcardselectedFalse()
    {
        cardselected = false;
    }
    public void setONLYLOOK(bool yesNO)
    {
        LOOKONLY = yesNO;
    }
    public bool getONLYLOOK()
    {
        return LOOKONLY;
    }
}