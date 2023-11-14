using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace BreadFlip
{
    public class DbCheck : MonoBehaviour
    {
        [SerializeField] TMP_InputField player;
        [SerializeField] TMP_InputField record;
        [SerializeField] TMP_Text output;

        public void RegPlayer()
        {
            var text = player.text;
            //PlayerPrefs.Se
            Debug.Log(text);
            Debug.Log(DBInterface.RegisterPlayer(text));
        }

        public void UpdatePlayer()
        {
            var rec = int.Parse(record.text);
            DBInterface.UpdateRecord(rec);
        }

        public void UpdateList()
        {
            var a = DBInterface.GetRecords();
            if(a.record_list == null)
            {
                output.text = "no connection";
                return;
            }
            output.text = "";
            int i = 0;
            foreach (var p in a.record_list)
            {
                ++i;
                output.text += i.ToString() +" "+ p.player + ": " + p.record.ToString() + "\n";
            }
        }

        public void NetIsOn()
        {
            var player = PlayerPrefs.GetInt("PlayerId", 0);
            Debug.Log("needs player retry" + player.ToString());
            if (player == -1)
            {
                DBInterface.RegisterPlayer(PlayerPrefs.GetString("PlayerName"));
            }
            
            var ok = PlayerPrefs.GetInt("RetryRecord", 0);
            Debug.Log("needs record retry" + ok.ToString());
            if (ok == 1)
                DBInterface.UpdateRecord(PlayerPrefs.GetInt("PlayerRecord"));
        }
    }
}
