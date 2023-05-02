using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;

public class ScoreboardItems : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public TMP_Text damageDoneText;

    Player player;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
        this.player = player;

        updateStats();
        
    }

    void updateStats()
    {
        if(player.CustomProperties.TryGetValue("DamageDone", out object damageDone))
        {
            damageDoneText.text = damageDone.ToString();
            
        }
    }

    

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == player)
        {
            if (changedProps.ContainsKey("DamageDone"))
            {
                updateStats();
            }
        }
    }

    
}
