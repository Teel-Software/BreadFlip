using System.Collections;

// #if UNITY_WEBGL && !UNITY_EDITOR
// using Agava.YandexGames;
// #endif

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BreadFlip
{
    public class Boot : MonoBehaviour
    {
        private IEnumerator Start()
        {
// #if UNITY_WEBGL && !UNITY_EDITOR
//             yield return YandexGamesSdk.Initialize();
// #endif
            SceneManager.LoadScene(sceneBuildIndex: 1);
            yield break;
        }
    }
}