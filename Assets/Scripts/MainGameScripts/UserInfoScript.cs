using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class UserInfoScript : MonoBehaviour
{
    public popupcardwindowChar PopUp;
    public CharCardScript chosed_character;
    private string user_name;
    public Text usernameOBJ;
    public Image AvartarImage;
    public Button allow_click;

    public void Input_UserInfo(CharCardScript chosed_char)
    {
        chosed_character = chosed_char;
        AvartarImage.sprite = chosed_character.image_Avertar;
    }
    private void Update()
    {
        if (AvartarImage == null)
        {
            allow_click.enabled = false;
        }
        else
        {
            allow_click.enabled = true;
        }
    }
    public void Input_Username(string username)
    {
        user_name = username;
        usernameOBJ.text = user_name;
    }
    public void ClickOn()
    {
        PopUp.openCharacterCard(chosed_character,false);
    }
}

