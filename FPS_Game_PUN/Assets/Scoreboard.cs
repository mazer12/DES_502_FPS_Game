using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform container;
    [SerializeField] GameObject DamageDoneTextPrefab;
    [SerializeField] GameObject ScoreboardItemPrefab;
    Scoreboard instance;
    [SerializeField] TMP_Text playerUsername;
    public GameObject gameEndScreen;

    Dictionary<Player, ScoreboardItems> scoreboardItems = new Dictionary<Player, ScoreboardItems>();


    float temp;
    private void Awake()
    {
        instance = this; 
    }
    private void Start()
    {
        ScoreboardItems item = Instantiate(DamageDoneTextPrefab, container).GetComponent<ScoreboardItems>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }

        temp = Time.time;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    void AddScoreboardItem(Player player)
    {
        ScoreboardItems item = Instantiate(ScoreboardItemPrefab, container).GetComponent<ScoreboardItems>();
        item.Initialize(player);
    }


}
