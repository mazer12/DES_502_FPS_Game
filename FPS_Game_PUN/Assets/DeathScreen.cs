using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathCanvas;
    public TextMeshProUGUI txt;
    public static DeathScreen instance;
    //PlayerManager playerManager;
    //PhotonView PV;

    private void Awake()
    {
        deathCanvas.SetActive(false);
        instance = this;
        ////playerManager = GetComponent<PlayerManager>();
        //playerManager = PhotonView.Find((int)1001).GetComponent<PlayerManager>();
        //playerManager.deathScreen = deathCanvas;
        //playerManager.txt = txt;
    }

    //private void Update()
    //{
    //    deathCanvas.SetActive(false);
    //    playerManager.wait();
    //    deathCanvas.SetActive(true);
    //}

}
