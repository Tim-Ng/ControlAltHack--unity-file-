using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "MissionCard")]
public class MissionCardScript : ScriptableObject
{
    public string Mission_code;
    public string MissonTitle;
    public string MissionDiscription;
    public bool Newb_job;

    public Sprite first_mission;
    public string skill_name_1;
    public int fist_change_hardnum;
    public string task_discription1;


    public Sprite second_mission;
    public string skill_name_2;
    public int second_change_hardnum;
    public string task_discription2;

    public bool hasThirdMission;
    public Sprite third_mission;
    public string skill_name_3;
    public int third_change_hardnum;
    public string task_discription3;

    public int success_amount_hacker_cread;
    public string other_success_things;
    public int other_success_how_much;
    public string success_discription;

    public int failure_amount_hacker_cread;
    public string failure_discription;
    public string other_failure_things;
    public int other_failure_how_much;

    public Sprite artwork_back;
    public Sprite artwork_front_info;

}
