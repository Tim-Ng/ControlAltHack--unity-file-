using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class play_card_button : MonoBehaviour
{
    private bool show_play=true; // remember to change 
    public GameObject play_button;
    public GameObject parent;
    public GameObject childOBJ_parent;
    public popupcardwindowEntropy PopUpEntropy;
    private EntropyCardDisplay parent_script;
    void Update()
    {
        play_button.SetActive(show_play);
    }
    public void setShowPlay(bool play_button)
    {
        show_play = play_button;
    }
    public void when_clicked()
    {
        parent_script = parent.GetComponent<EntropyCardDisplay>();
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in childOBJ_parent.transform)
        {
            if (child.GetComponent<EntropyCardDisplay>().entropyData== parent_script.entropyData)
            {
                Destroy(child.gameObject);
                break;
            }
        }
        PopUpEntropy.closePopup();
    }
}
