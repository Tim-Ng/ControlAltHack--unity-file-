using UnityEngine;

namespace DrawCards
{
    /// <summary>
    /// This script for a scriptableObject to hold the data of entropycards.
    /// </summary>
    /// <remarks>
    /// To create a scroptableObject right click and hover on create find the fileName "New Card" and click on EntorpyCard
    /// </remarks>
    [CreateAssetMenu(fileName = "New Card", menuName = "EntropyCard")]
    public class EntropyCardScript : ScriptableObject
    {
        /// <summary>
        /// The value of the Entropy ID 
        /// </summary>
        [Header("Entropy Card ID")]
        public int EntropyCardID;

        /// <summary>
        /// The title of the card 
        /// </summary>
        [Header("Title")]
        public string Title;

        /// <summary>
        /// Image/Sprite of the front of the card [info]
        /// </summary>
        [Header("Artwork for this card")]
        public Sprite artwork_info;
        /// <summary>
        /// Image/Sprite of the back of the card 
        /// </summary>
        public Sprite artwork_back;

        /// <summary>
        /// The cost of the entropy card
        /// </summary>
        [Header("The Cost")]
        public int Cost;

        /// <summary>
        /// If this entropy card has a skill effector
        /// </summary>
        [Header("Skill Effector number 1")]
        public bool SkillEffecter;
        /// <summary>
        /// The skill being effected for the first effector
        /// </summary>
        public AllJobs whichSkillIncrease1;
        /// <summary>
        /// How much this skill is being effected by for the first effector
        /// </summary>
        public int byHowMuchSkillIncrease1;

        /// <summary>
        /// If this entropy card has a second skill effector
        /// </summary>
        [Header("Skill Effector number 2")]
        public bool AnotherSecondSkill;
        /// <summary>
        /// The skill being effected for the second effector
        /// </summary>
        public AllJobs whichSkillIncrease2;
        /// <summary>
        /// How much this skill is being effected by for the second effector
        /// </summary>
        public int byHowMuchSkillIncrease2;

        /// <summary>
        /// If this is entropy card is for lighting stick this will be true else false
        /// </summary>
        [Header("If Lightning")]
        public bool UseSucFailLighting;
        /// <summary>
        /// Which job the Lightning strick is rolling for
        /// </summary>
        public AllJobs RollVSWhich;

        /// <summary>
        /// Text for lightning success
        /// </summary>
        [Header("If lighting roll success")]
        [Multiline(3)]
        public string Success;
        /// <summary>
        /// How much cred is effected when success
        /// </summary>
        public int add_how_much_cred;

        /// <summary>
        /// Text for lightning fail
        /// </summary>
        [Header("If lighting roll failed")]
        [Multiline(3)]
        public string Failure;
        /// <summary>
        /// How much cred is effected when fail
        /// </summary>
        public int minus_how_much_cred;

        /// <summary>
        /// If this card have other uses other than lighting or skill effect
        /// </summary>
        [Header("If use usage")]
        public bool use_usage;
        /// <summary>
        /// Text for the usage Discription
        /// </summary>
        [Multiline(5)]
        public string usageDiscription;

        /// <summary>
        /// If this card is removed after play
        /// </summary>
        [Space(10)]
        public bool removeAfterPlay;
    }
}
