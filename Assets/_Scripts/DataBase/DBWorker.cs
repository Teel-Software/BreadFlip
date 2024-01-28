using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using System.Threading.Tasks;

namespace BreadFlip
{
    public class DBWorker : MonoBehaviour
    {
        private static string endpoint = "";//http://localhost:8080
        private DBTopRecords rec;

        public void GetRecords(int offset, int count, Action<DBTopRecords> action)
        {
            initEndpoint();
            StartCoroutine(getRecords(offset, count, action));
        }

        private IEnumerator getRecords(int offset, int count, Action<DBTopRecords> action)
        {
            var request = UnityWebRequest.Get(endpoint + "/getrecords");
            request.SetRequestHeader("offset", offset.ToString());
            request.SetRequestHeader("count", count.ToString());

            yield return request.SendWebRequest();

            if (!(request.error == null || request.error == ""))
            {
                Debug.Log("error");
                Debug.LogError(request.error);
            }
            else
            {
                rec = JsonUtility.FromJson<DBTopRecords>(request.downloadHandler.text);
                action(rec);
            }
            request.Dispose();
        }

        public void PutRecord(DBPlayer player, Action<int> action)
        {
            StartCoroutine(putRecord(player, action));
        }

        public IEnumerator putRecord(DBPlayer player, Action<int> action)
        {
            initEndpoint();
            var str = JsonUtility.ToJson(player);

            var request = UnityWebRequest.Put(endpoint + "/add", Encoding.UTF8.GetBytes(str));
            yield return request.SendWebRequest();

            var res = -1;
            if (request.error == null || request.error == "")
                res = int.Parse(request.downloadHandler.text);
            request.Dispose();
            action(res);
        }

        public void UpdateRecord(DBRegPlayer player)
        {
            StartCoroutine(updateRecord(player));
        }

        private IEnumerator updateRecord(DBRegPlayer player)
        {
            initEndpoint();
            var str = JsonUtility.ToJson(player);
            Debug.Log(str);

            var request = UnityWebRequest.Put(endpoint + "/change", Encoding.UTF8.GetBytes(str));
            yield return request.SendWebRequest();

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
