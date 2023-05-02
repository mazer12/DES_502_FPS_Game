using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEditor;
using System.Linq;
using System.Globalization;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller1;
    GameObject controller2;
    GameObject controller;
    //public GameObject deathScreen;
    //public TextMeshProUGUI txt;
    DeathScreen deathScreen;

    public float DamageDone;
    
    public Animator anim;

    public static PlayerManager instance;
    void Awake()
    {
        deathScreen = GetComponent<DeathScreen>();
        //deathScreen.deathCanvas.SetActive(false);
        instance = this;    
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            //updatePlayer();
            CreateController();
            

        }

    }

    private void Update()
    {
        if (GameEnd.ends)
        {
            if (controller1)
            {
                PhotonNetwork.Destroy(controller1);
                // Unlock Curser
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                PhotonNetwork.Destroy(controller2);
                // Unlock Curser
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    void CreateController()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            Transform spawn = SpawnManager.instance.GetTeamSpawn(0);
            controller1 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player 1"), spawn.position, spawn.rotation, 0, new object[] { PV.ViewID });
            //number.Add("Blue");
        }
        else
        {
            Transform spawn = SpawnManager.instance.GetTeamSpawn(1);
            controller2 = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player 2"), spawn.position, spawn.rotation, 0, new object[] { PV.ViewID });
        }
        //controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player 1"), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });
        //anim = controller.GetComponentInChildren<Animator>();


    }

    public void Die()
    {
        if (controller1)
        {
            PhotonNetwork.Destroy(controller1);
            StartCoroutine("penalty");
            //CreateController();
            // Unlock Curser
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            PhotonNetwork.Destroy(controller2);
            StartCoroutine("penalty");
            //CreateController();
            // Unlock Curser
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }       

    }

    IEnumerator penalty()
    {
        //wait();
        //deathScreen.deathCanvas.SetActive(true);
        yield return new WaitForSeconds(5);     
        CreateController();
        StopCoroutine("penalty");
    }

    public void wait()
    {
        deathScreen.deathCanvas.SetActive(true);
        int i = 5;
        while (i > 0)
        {
            deathScreen.txt.text = "RESPAWNING IN: " + i.ToString();
            i--;
        }
        deathScreen.deathCanvas.SetActive(false);
    }
    //public void updatePlayer()
    //{
    //    PV.RPC("RPC_GetPlayer", RpcTarget.OthersBuffered, currentPlayer);
    //}

    //[PunRPC]

    //public void RPC_GetPlayer(int currentPlayer)
    //{
    //    if (currentPlayer == 1)
    //    {
    //        currentPlayer = 2;
    //    }
    //    else
    //    {
    //        currentPlayer = 1;
    //    }
    //}

    public void GetDamage(float damage)
    {
        PV.RPC("RPC_GetDamage", PV.Owner, damage);
    }

    [PunRPC]
    public void RPC_GetDamage(float damage)
    {
        DamageDone += damage; 

        Hashtable hash = new Hashtable();
        hash.Add("DamageDone", DamageDone);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

    }
    public static PlayerManager Find(Player player)
    {
        return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.PV.Owner == player);
    }
}