using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupcard : MonoBehaviour
{
    public GameObject cardpopuptemplate;
    public GameObject cardtemplate;
    public GameObject canvas;
    public void clickOn()
    {
        GameObject characterPlayerCard1 = Instantiate(cardpopuptemplate, new Vector3(0, 0, 0), Quaternion.identity);
        characterPlayerCard1.transform.SetParent(canvas.transform, false);
        Destroy(characterPlayerCard1,5.0f);
    }
}
