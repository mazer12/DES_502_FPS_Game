using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView PV;
    [SerializeField] TMP_Text text;

    void Start()
    {
        if (PV.IsMine)
        {
            gameObject.SetActive(false);
        }
        text.text = PV.Owner.NickName;
    }
}
