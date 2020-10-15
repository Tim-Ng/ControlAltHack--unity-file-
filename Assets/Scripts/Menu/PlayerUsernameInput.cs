using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class PlayerUsernameInput : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField nameInputFeild = null;
        [SerializeField] private Button continueButton = null;

        private const string PlayerPrefsNameKey = "PlayerName";

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            SetUpInputFeild();
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("We are now connected to " + PhotonNetwork.CloudRegion + "sever!");
        }
        private void SetUpInputFeild()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }
            string defultName = PlayerPrefs.GetString(PlayerPrefsNameKey);
            nameInputFeild.text = defultName;
            SetPlayerName(defultName);
        }

        public void SetPlayerName(string name)
        {
            continueButton.interactable = !string.IsNullOrEmpty(nameInputFeild.text);
        }
        public void SavePlayerName()
        {
            string playerName = nameInputFeild.text;
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
            SceneManager.LoadScene("Host_Join", LoadSceneMode.Single);
        }
    }
}
