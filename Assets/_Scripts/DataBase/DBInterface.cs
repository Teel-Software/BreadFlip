using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace BreadFlip
{
    public static class DBInterface
    {
        private static DBWorker worker;

        public static void RegisterPlayer(string name)
        {
            initWorker();
            var a = new DBPlayer();
            a.player = name;
            worker.PutRecord(a, (int a) => {
                Debug.Log(a);
                PlayerPrefs.SetInt("PlayerId", a);
                PlayerPrefs.SetString("PlayerName", name);
                PlayerPrefs.SetInt("PlayerRecord", 0);
            });
        }

        public static bool UpdateRecord(int record)
        {
            initWorker();
            Debug.Log("updating record");
            var prevRecord = PlayerPrefs.GetInt("PlayerRecord", 0);
            Debug.Log("current record " + prevRecord.ToString());
            if (prevRecord > record) return false;
            Debug.Log("continue changing");
            var playerReq = new DBRegPlayer();
            PlayerPrefs.SetInt("PlayerRecord", record);
            playerReq.player = PlayerPrefs.GetInt("PlayerId", 404);
            playerReq.record = record;

            worker.UpdateRecord(playerReq);
            return true;
        }

        public static void GetRecords(int offset, int count, Action<DBTopRecords> action)
        {
            initWorker();
            worker.GetRecords(offset, count, action);
        }

        private static void initWorker()
        {
            if (worker != null) return;
            var a = GameObject.FindGameObjectsWithTag("db");
            worker = a[0].GetComponent<DBWorker>();
        }
    }
}
