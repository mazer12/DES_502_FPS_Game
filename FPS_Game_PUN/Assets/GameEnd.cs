using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Threading;

public class GameEnd : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerUsername;
    public GameObject gameEndScreen;

    public static GameEnd instance;

    //Scoreboard scoreboard;

    PhotonView PV;

    float maxDamage;
    float maxDamage1;

    public static bool ends = false;

    bool p1w = false;
    bool p2w = false;
    bool pd = false;

    private void Awake()
    {
        //scoreboard = GetComponent<Scoreboard>();
        PV = GetComponent<PhotonView>();
        instance = this;
    }

    private void Update()
    {
        //Debug.Log(maxDamage);
        //Debug.Log(maxDamage1);
        GetScreen();

        if (p2w == true && p1w == false)
        {
            playerUsername.text = PhotonNetwork.PlayerList[1].NickName + " WINS";
        }
        else if (p1w == true && p2w == false)
        {
            playerUsername.text = PhotonNetwork.PlayerList[0].NickName + " WINS";
        }
        //else
        //{
        //    playerUsername.text = "It's a Draw";
        //}

    }

    public void GetScreen()
    {

        PV.RPC("RPC_GetScreen", RpcTarget.All);
    }

    [PunRPC]

    public void RPC_GetScreen()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //if (PlayerManager.Find(PhotonNetwork.PlayerList[0]).DamageDone > maxDamage)
            //{
            //    maxDamage = PlayerManager.Find(PhotonNetwork.PlayerList[0]).DamageDone;

            //}
            maxDamage = PlayerManager.Find(PhotonNetwork.PlayerList[0]).DamageDone;
        }
        else
        {
            //if (PlayerManager.Find(PhotonNetwork.PlayerList[1]).DamageDone > maxDamage1)
            //{
            //    maxDamage1 = PlayerManager.Find(PhotonNetwork.PlayerList[1]).DamageDone;

            //}
            maxDamage1 = PlayerManager.Find(PhotonNetwork.PlayerList[1]).DamageDone;
        }

        this.GetComponent<PhotonView>().RPC("RPC_Win", RpcTarget.AllBufferedViaServer, maxDamage, maxDamage1);

    }

    [PunRPC]

    public void RPC_Win(float p1, float p2)
    {
        Debug.Log("p1 " + p1);
        Debug.Log("p2 " + p2);
        if (p1 > p2) { Debug.Log("yes"); }
        else { Debug.Log("no"); }

        if (EnemyAiTutorial.deathCount == 2)
        {
            StartCoroutine(wait());
            compare(p1, p2);
            
        }
    }

    public void compare(float a, float b)
    {
        if (a > b)
        {
            //playerUsername.text = PhotonNetwork.PlayerList[0].NickName + " WINS";
            p1w = true;

        }
        else if (b > a)
        {
            //playerUsername.text = PhotonNetwork.PlayerList[1].NickName + " WINS";
            p2w = true;
        }
        else { pd = true; }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        ends = true;
        gameEndScreen.SetActive(true);
        StopCoroutine(wait());
    }
}
