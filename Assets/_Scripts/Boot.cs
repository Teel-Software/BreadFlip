using System;
using System.Collections;
using Agava.YandexGames;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BreadFlip
{
    public class Boot : MonoBehaviour
    {
        private IEnumerator Start()
        {
#if !UNITY_EDITOR
            yield return YandexGamesSdk.Initialize();
#endif
            SceneManager.LoadScene(sceneBuildIndex: 1);
            yield break;
        }
    }
}