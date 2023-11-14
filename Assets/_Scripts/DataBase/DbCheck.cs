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
        public void RegPlayer(string text)
        {
            //var text = GetComponent<TMP_Text>().text;
            Debug.Log(text);
            DBInterface.RegisterPlayer(text);
        }

        public void UpdateList()
        {
            var a = DBInterface.GetRecords();
            gameObject.GetComponent<TMP_Text>().text = "";
            foreach (var p in a.record_list)
            {
                gameObject.GetComponent<TMP_Text>().text += p.player + ": " + p.record.ToString() + "\n";
            }
        }
    }
}
