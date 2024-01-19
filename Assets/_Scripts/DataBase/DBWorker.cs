using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using System.Threading.Tasks;

namespace BreadFlip
{
    public static class DBWorker
    {
        private static string endpoint = "http://localhost:8080";//http://localhost:8080
        public static DBTopRecords GetRecords(int offset, int count)
        {
            initEndpoint();
            var request = UnityWebRequest.Get(endpoint + "/getrecords");
            request.SetRequestHeader("offset", offset.ToString());
            request.SetRequestHeader("count", count.ToString());
            request.SendWebRequest();
            while (!request.isDone) { }
            var a = new DBTopRecords();
            if (request.error == null || request.error == "")
                a = JsonUtility.FromJson<DBTopRecords>(request.downloadHandler.text);
            request.Dispose();
            return a;
        }

        public static int PutRecord(DBPlayer player)
        {
            initEndpoint();
            var str = JsonUtility.ToJson(player);
            Debug.Log(str);

            var request = UnityWebRequest.Put(endpoint + "/add", Encoding.UTF8.GetBytes(str));
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            request.SetRequestHeader("size", bytes.Length.ToString());
            request.SendWebRequest();
            while (!request.isDone) { }
            //var res = request.isError || request.isNetworkError || request.isHttpError;
            Debug.Log(request.downloadHandler.text);

            var res = -1;
            Debug.Log(request.error);
            if (request.error == null || request.error == "")
                res = int.Parse(request.downloadHandler.text);
            request.Dispose();
            return res;
        }

        public static async Task UpdateRecord(DBRegPlayer player)
        {
            initEndpoint();
            var str = JsonUtility.ToJson(player);
            Debug.Log(str);

            var request = UnityWebRequest.Put(endpoint + "/change", Encoding.UTF8.GetBytes(str));
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            request.SetRequestHeader("size", bytes.Length.ToString());
            request.SendWebRequest();
            while (!request.isDone) {
                await Task.Yield();
            }

            if (request.error != null) PlayerPrefs.SetInt("RetryRecord", 1);
            else PlayerPrefs.SetInt("RetryRecord", 0);

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
        public int id;
        public string player;
        public int record;
    }

    [Serializable]
    public struct DBRegPlayer
    {
        public int player;
        public int record;
    }

    [Serializable]
    public struct DBTopRecords
    {
        public List<DBPlayer> record_list;
    }
}
