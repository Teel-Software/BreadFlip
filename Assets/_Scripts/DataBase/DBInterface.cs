using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace BreadFlip
{
    public static class DBInterface
    {
        public static void RegisterPlayer(string name)
        {
            var a = new DBPlayer();
            a.player = name;
            var id = DBWorker.PutRecord(a);
            Debug.Log(id);
            PlayerPrefs.SetInt("PlayerId", id);
            PlayerPrefs.SetString("PlayerName", name);
            if(id != -1)
                PlayerPrefs.SetInt("PlayerRecord", 0);
        }

        public static bool UpdateRecord(int record)
        {
            Debug.Log("updating record");
            var prevRecord = PlayerPrefs.GetInt("PlayerRecord", 0);
            Debug.Log("current record" + prevRecord.ToString());
            if (prevRecord > record) return false;
            Debug.Log("continue changing");
            var playerReq = new DBRegPlayer();
            PlayerPrefs.SetInt("PlayerRecord", record);
            playerReq.player = PlayerPrefs.GetInt("PlayerId", 404);
            playerReq.record = record;
            Task.Run(() =>
            {
                DBWorker.UpdateRecord(playerReq);
            });
            return true;
        }

        public static DBTopRecords GetRecords(int offset, int count)
        {
            return DBWorker.GetRecords(offset, count);
        }
    }
}
