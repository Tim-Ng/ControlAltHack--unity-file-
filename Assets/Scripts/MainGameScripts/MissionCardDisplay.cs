using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionCardDisplay : MonoBehaviour
{
    public MissionCardScript mission_script;
    public Text MissonTitle;
    public Text MissionDiscription;

    public Image first_mission;
    public Text skill_name_1;
    public Text fist_change_hardnum;

    public Image second_mission;
    public Text skill_name_2;
    public Text second_change_hardnum;

    public GameObject third_mission_obj;
    public Image third_mission;
    public Text skill_name_3;
    public Text third_change_hardnum;

    public Text success_amount_hacker_cread;
    public Text success_discription;

    public Text failure_amount_hacker_cread;
    public Text failure_discription;

    public Image artwork_back;
    public Image artwork_front_info;

    public Text task_discription1;
    public Text task_discription2;
    public Text task_discription3;
    public Text Newb_job;

    public GameObject InfoSide;
    public GameObject FrontSide;
    public void setUpdate()
    {
        Start();
    }
    private void Start()
    {
        third_mission_obj.SetActive(false);
        MissonTitle.text = mission_script.MissonTitle;
        MissionDiscription.text = mission_script.MissionDiscription;

        first_mission.sprite = mission_script.first_mission;
        skill_name_1.text = mission_script.skill_name_1;
        fist_change_hardnum.text = mission_script.fist_change_hardnum.ToString();
        task_discription1.text = mission_script.task_discription1;

        second_mission.sprite = mission_script.second_mission;
        skill_name_2.text = mission_script.skill_name_2;
        second_change_hardnum.text = mission_script.second_change_hardnum.ToString();
        task_discription2.text = mission_script.task_discription2;

        success_amount_hacker_cread.text = mission_script.success_amount_hacker_cread.ToString();
        success_discription.text = mission_script.success_discription;

        failure_amount_hacker_cread.text = mission_script.failure_amount_hacker_cread.ToString();
        failure_discription.text = mission_script.failure_discription;

        artwork_back.sprite = mission_script.artwork_back;
        artwork_front_info.sprite = mission_script.artwork_front_info;

        if (mission_script.Newb_job)
        {
            Newb_job.text = "NEWB JOB";
        }

        if (mission_script.hasThirdMission)
        {
            third_mission_obj.SetActive(true);
            third_mission.sprite = mission_script.third_mission;
            skill_name_3.text = mission_script.skill_name_3;
            third_change_hardnum.text = mission_script.third_change_hardnum.ToString();
            task_discription3.text = mission_script.task_discription3;
        }

    }
}
