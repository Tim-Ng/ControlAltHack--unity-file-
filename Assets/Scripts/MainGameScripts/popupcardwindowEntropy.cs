using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupcardwindowEntropy : MonoBehaviour
{
    public GameObject PopUpCard;
    public GameObject EntropyCard;
    public EntropyCardDisplay EntropyDisplayUI;
    private EntropyCardScript pop_input_Entropy;
    [SerializeField] private DrawCharacterCard drawCharacterCard;
    public GameObject playCardButtonOBJ;
    private bool canPlay = false;
    private bool canAttack = false;
    private bool ifbeforeRoll= true;
    public bool getBeforeRoll
    {
        set { ifbeforeRoll = value; }
        get { return ifbeforeRoll; }
    }
    private int[] beforeMissionRollTime ={ 10,19,24,9,6,33,5,18,22,21,3,2,25,23 ,20 ,26,27,28,29,30,2};
    private int[] afterMissionRollTime = { 8, 7, 4, 1 , 26, 27, 28, 29, 30 };
    private List<int> oneTimeRoll=new List<int>();//1,4,7,8,26,27,28,29,30
    private void Start()
    {
        PopUpCard.SetActive(true);
    }
    public void setBoolcanPlay(bool YesOrNo)
    {
        canPlay = YesOrNo;
    }
    public void setBoolcanAttack(bool YesOrNo)
    {
        canAttack = YesOrNo;
    }
    public void openEntropyCard(EntropyCardScript input_EntropyCard)
    {
        Start();
        pop_input_Entropy = input_EntropyCard;
        EntropyDisplayUI.entropyData = pop_input_Entropy;
        EntropyDisplayUI.setUpdate();
        if (pop_input_Entropy.IsSharedFate)
        {
            playCardButtonOBJ.SetActive(true);
        }
        else if (pop_input_Entropy.IsLigthingStrikes && !drawCharacterCard.IsMyTurn && canAttack)
        {
            playCardButtonOBJ.SetActive(true);
        }
        else if (checkbeforeMissionRollTime(pop_input_Entropy.EntropyCardID) &&drawCharacterCard.IsMyTurn && canPlay )
        {
            if ( checkOneTimeRoll(pop_input_Entropy.EntropyCardID))
            {
                playCardButtonOBJ.SetActive(false);
            }
            else
            {
                playCardButtonOBJ.SetActive(true);
            }
        }
        else if (checkafterMissionRollTime(pop_input_Entropy.EntropyCardID ) && drawCharacterCard.IsMyTurn && canPlay && !ifbeforeRoll)
        {
            if (checkOneTimeRoll(pop_input_Entropy.EntropyCardID))
            {
                playCardButtonOBJ.SetActive(false);
            }
            else
            {
                playCardButtonOBJ.SetActive(true);
            }
        }
        else
        {
            playCardButtonOBJ.SetActive(false);
        }
        EntropyDisplayUI.InfoSide.SetActive(true);
        EntropyDisplayUI.FrontSide.SetActive(false);
    }
    public void closePopup()
    {
        PopUpCard.SetActive(false);
        playCardButtonOBJ.SetActive(false);
        EntropyDisplayUI.InfoSide.SetActive(false);
    }
    public EntropyCardScript GetEntropyCardScript()
    {
        return pop_input_Entropy;
    }
    public void addToRollTimeList(int whichCard)
    {
        oneTimeRoll.Add(whichCard);
    }
    public bool checkOneTimeRoll(int whichCard)
    {
        foreach ( int checkCard in oneTimeRoll)
        {
            if (checkCard == whichCard)
            {
                return true;
            }
        }
        return false;
    }
    public bool checkbeforeMissionRollTime(int whichCard)
    {
        foreach (int checkCard in beforeMissionRollTime)
        {
            if (checkCard == whichCard)
            {
                return true;
            }
        }
        return false;
    }
    public bool checkafterMissionRollTime(int whichCard)
    {
        foreach (int checkCard in afterMissionRollTime)
        {
            if (checkCard == whichCard)
            {
                return true;
            }
        }
        return false;
    }
    public void resetPlayButton()
    {
        oneTimeRoll.Clear();
    }
}