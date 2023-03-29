using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;
    int numOfPlayers;
    public GameObject Player01;
    public GameObject Player02;
    bool spawn01 = true;
    bool spawn02 = true;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            //CreateController();
        }
    }
    
    void Update()
    {
        if (numOfPlayers == 1 && spawn01 == true)
        {
            SpawnPlayer01();
            numOfPlayers = 2;
            spawn01 = false;
        }
        else if (numOfPlayers == 2 && spawn02 == true)
        {
            SpawnPlayer02();
            spawn02 = false;
        }

        CheckPlayers();
    }

    void CreateController()
    {
//<<<<<<< Updated upstream
//<<<<<<< Updated upstream:FPS_Game_PUN/Assets/Scripts/PlayerManager.cs
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player 2"), Vector3.zero, Quaternion.identity);
//=======
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity, 0, new object[] {PV.ViewID});
//=======
////<<<<<<< Updated upstream:FPS_Game_PUN/Assets/Scripts/PlayerManager.cs
       //PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
//=======
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player 2"), Vector3.zero, Quaternion.identity, 0, new object[] {PV.ViewID});
    }

    void SpawnPlayer01()
    {
        if (PV.IsMine)
        {
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Player01.name), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });
        }
    }
    void SpawnPlayer02()
    {
        if (PV.IsMine)
        {
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", Player02.name), Vector3.zero, Quaternion.identity, 0, new object[] { PV.ViewID });
        }
    }

    void CheckPlayers()
    {
        numOfPlayers = PhotonNetwork.CountOfPlayers;

        for (int i = 0; i <= numOfPlayers; i++)
        {
            if (numOfPlayers > 2)
            {
                numOfPlayers -= 2;
            }

        }
//>>>>>>> Stashed changes
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
//>>>>>>> Stashed changes:FPS_Game_PUN/Assets/Scripts/Menu scripts/PlayerManager.cs
    }
}