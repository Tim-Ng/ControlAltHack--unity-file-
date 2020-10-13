using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class selectCharacterCard : MonoBehaviour
{
    public UserInfoScript user_input_info;
    public popupcardwindow Popup;
    public GameObject clone_to_delete;
    public GameObject select_button;

    public void clickOn()
    {
        user_input_info.Input_UserInfo(Popup.GetCharCardScript());
        select_button.SetActive(false);
        foreach (Transform child in clone_to_delete.transform)
        {
            Destroy(child.gameObject);
        }
        Popup.closePopup();
    }
}
