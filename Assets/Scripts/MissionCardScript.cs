using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace DrawCards {
    [CreateAssetMenu(fileName = "New Card", menuName = "MissionCard")]
    public class MissionCardScript : ScriptableObject
    {
        public int Mission_code;
        public string MissonTitle;
        public bool Newb_job;

        public AllJobs skill_name_1;
        public int fist_change_hardnum;

        public bool hasSecondMission;
        public AllJobs skill_name_2;
        public int second_change_hardnum;

        public bool hasThirdMission;
        public AllJobs skill_name_3;
        public int third_change_hardnum;

        public int success_amount_hacker_cread;
        public string other_success_things;
        public int other_success_how_much;

        public int failure_amount_hacker_cread;
        public string other_failure_things;
        public int other_failure_how_much;

        public Sprite artwork_back;
        public Sprite artwork_front_info;
        [Multiline]
        public string ifpassText;
        [Multiline]
        public string iffailText;
    }
}
