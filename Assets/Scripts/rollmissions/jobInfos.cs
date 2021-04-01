using DrawCards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rollmissions
{
    /// <summary>
    /// This is a class that holds the information of a task/job.
    /// </summary>
    /// <remarks>
    /// With variables position to mark first, second or third. <br/>
    /// skillName as the enum of the AllJobs <br/>
    /// amount is the minimum roll needed to pass <br/>
    /// passingOrNot is if this job/task has passed or failed true = pass , false = fail
    /// </remarks>
    public class jobInfos
    {
        /// <summary>
        /// This variable that hold the value to mark its position as first, second or third.
        /// </summary>
        public int position; // 1 , 2 or 3
        /// <summary>
        /// This variable holds the skillName and enum of AllJobs of this job/task
        /// </summary>
        public AllJobs skillName;
        /// <summary>
        /// The amount will need to roll to pass
        /// </summary>
        public int amount;
        /// <summary>
        /// This variable hold if this job/task has passed or failed true = pass , false = fail
        /// </summary>
        public bool passingOrNot { get; set; }
        /// <summary>
        /// This is the a constructor to input the value for this jobs
        /// </summary>
        /// <param name="Position">Its position</param>
        /// <param name="SkillName">Its skillname</param>
        /// <param name="Amount">The amount needed to roll</param>
        /// <param name="PassingOrNOt">Fail or pass, but at default is false</param>
        public jobInfos(int Position, AllJobs SkillName, int Amount, bool PassingOrNOt)
        {
            position = Position;
            skillName = SkillName;
            amount = Amount;
            passingOrNot = PassingOrNOt;
        }
        /// <summary>
        /// This is to increment its Amount need to roll to pass
        /// </summary>
        /// <param name="howMuch">The Amount to increase</param>
        public void addSkillAmount(int howMuch) => amount += howMuch;
        /// <summary>
        /// This is to deduct its Amount need to roll to pass
        /// </summary>
        /// <param name="howMuch">The Amount to deduct by</param>
        public void subSkillAmount(int howMuch) => amount -= howMuch;
    }
}
