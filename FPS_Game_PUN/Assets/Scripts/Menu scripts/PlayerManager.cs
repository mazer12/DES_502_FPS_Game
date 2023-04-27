using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEditor;
using System.Linq;
using System.Globalization;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller1;
    GameObject controller2;
    GameObject controller;
    public int currentPlayer;

    
    public Animator anim;

    public static PlayerManager instance;
    void Awake()
    {
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
            CreateController();
        }
        else
        {
            PhotonNetwork.Destroy(controller2);
            CreateController();
        }
        

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
}