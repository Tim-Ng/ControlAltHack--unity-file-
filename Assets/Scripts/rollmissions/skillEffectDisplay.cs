using DrawCards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rollmissions
{
    public class skillEffectDisplay : MonoBehaviour
    {
        [SerializeField] private Text set=null;
        private SkillEffector SkillID;
        public void setSkillID(SkillEffector skillID) 
        { 
            SkillID = skillID;
            if (SkillID.amount < 0)
            {
                set.text = "Skill Name: " + GetStringOfTask.get_string_of_job(SkillID.skillName) + "\nAmount Change: " + SkillID.amount.ToString() + "\nIn turn:" + SkillID.RoundNumber.ToString();
            }
            else
            {
                set.text = "Skill Name: " + GetStringOfTask.get_string_of_job(SkillID.skillName) + "\nAmount Change: +" + SkillID.amount.ToString() + "\nIn turn:" + SkillID.RoundNumber.ToString();
            }
        }
        public int getSkillTurn()
        {
            return SkillID.RoundNumber;
        }
        public SkillEffector getSkillEffector()
        {
            return SkillID;
        }
    }
}
