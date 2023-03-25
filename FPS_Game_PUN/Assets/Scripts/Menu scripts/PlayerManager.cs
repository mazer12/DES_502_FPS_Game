using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
<<<<<<< Updated upstream:FPS_Game_PUN/Assets/Scripts/PlayerManager.cs
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player 2"), Vector3.zero, Quaternion.identity);
=======
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity, 0, new object[] {PV.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        CreateController();
>>>>>>> Stashed changes:FPS_Game_PUN/Assets/Scripts/Menu scripts/PlayerManager.cs
    }
}