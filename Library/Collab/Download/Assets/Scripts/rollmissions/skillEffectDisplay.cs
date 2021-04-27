using DrawCards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace rollmissions
{
    /// <summary>
    /// This class is to hold the data of a skill effector
    /// </summary>
    public class skillEffectDisplay : MonoBehaviour
    {
        /// <summary>
        /// The text component of the UI game object of the text 
        /// </summary>
        [SerializeField] private Text set=null;
        /// <summary>
        /// This is to hold the value of the SkillEffector
        /// </summary>
        private SkillEffector SkillID;
        /// <summary>
        /// This is a constructor to input the required infomation as well as setting the text 
        /// </summary>
        /// <param name="skillID">The data for the text which is the SkillEffector</param>
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
        /// <summary>
        /// This is to get when in the round does this skill take effect 
        /// </summary>
        /// <returns>The round number of the value</returns>
        public int getSkillTurn()
        {
            return SkillID.RoundNumber;
        }
        /// <summary>
        /// This is to get the SkillEffector that this script holds
        /// </summary>
        /// <returns>The SkillEffector of this script</returns>
        public SkillEffector getSkillEffector()
        {
            return SkillID;
        }
    }
}
