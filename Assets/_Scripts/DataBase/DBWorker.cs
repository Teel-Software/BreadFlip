using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

namespace BreadFlip
{
    public static class DBWorker
    {
        private static string endpoint = "";
        public static DBTopRecords GetRecords()
        {
            initEndpoint();
            var request = UnityWebRequest.Get(endpoint + "/db");
            request.SendWebRequest();
            while (!request.isDone) { }
            var a = JsonUtility.FromJson<DBTopRecords>(request.downloadHandler.text);
            Debug.Log(a.record_list[0].player);
            request.Dispose();
            return a;
        }

        public static bool PutRecord(DBPlayer player)
        {
            initEndpoint();
            var str = JsonUtility.ToJson(player);
            Debug.Log(str);

            var request = UnityWebRequest.Put(endpoint + "/add", Encoding.UTF8.GetBytes(str));
            request.SetRequestHeader("size", str.Length.ToString());
            request.SendWebRequest();
            while (!request.isDone) { }
            //var res = request.isError || request.isNetworkError || request.isHttpError;
            var res = true;
            request.Dispose();
            return res;
        }

        public static void UpdateRecord(DBPlayer player)
        {
            initEndpoint();
            var str = JsonUtility.ToJson(player);
            Debug.Log(str);

            var request = UnityWebRequest.Put(endpoint + "/change", Encoding.UTF8.GetBytes(str));
            request.SetRequestHeader("size", str.Length.ToString());
            request.SendWebRequest();
            while (!request.isDone) { }
            request.Dispose();
        }

        private static void initEndpoint()
        {
            if (endpoint != "") return;
            TextAsset text = Resources.Load("endpoint") as TextAsset;
            endpoint = text.text.Trim();
            Debug.Log(endpoint);
        }
    }

    [Serializable]
    public struct DBPlayer
    {
        public string player;
        public int record;
    }

    [Serializable]
    public struct DBTopRecords
    {
        public List<DBPlayer> record_list;
    }
}
