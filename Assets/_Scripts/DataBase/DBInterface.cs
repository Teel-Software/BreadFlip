using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip
{
    public static class DBInterface
    {
        public static int RegisterPlayer(string name)
        {
            var a = new DBPlayer();
            a.player = name;
            var id = DBWorker.PutRecord(a);
            Debug.Log(id);
            PlayerPrefs.SetInt("PlayerId", id);
            PlayerPrefs.SetString("PlayerName", name);
            return id;
        }

        public static bool UpdateRecord(int record)
        {
            Debug.Log("updating record");
            var prevRecord = PlayerPrefs.GetInt("PlayerRecord", 0);
            if (prevRecord > record) return false;
            var a = new DBRegPlayer();
            PlayerPrefs.SetInt("PlayerRecord", record);
            a.player = PlayerPrefs.GetInt("PlayerId", 404);
            a.record = record;
            var ok = DBWorker.UpdateRecord(a);
            if (!ok)
                PlayerPrefs.SetInt("RetryRecord", 1);
            else
                PlayerPrefs.SetInt("RetryRecord", 0);
            return true;
        }

        public static DBTopRecords GetRecords()
        {
            return DBWorker.GetRecords();
        }
    }
}
