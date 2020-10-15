using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info_attendance_button : MonoBehaviour
{
    public GameObject Panel;
    public GameObject Attending_Image;
    public GameObject Skipping_Image;

    public void open_info_popup_attending()
    {
        Panel.SetActive(true);
        Attending_Image.SetActive(true);
    }
    public void open_info_popup_skipping()
    {
        Panel.SetActive(true);
        Skipping_Image.SetActive(true);
    }
    public void close_popup()
    {
        Panel.SetActive(false);
        Skipping_Image.SetActive(false);
        Attending_Image.SetActive(false);
    }
}
