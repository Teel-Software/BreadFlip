using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BreadFlip.Generation.Props
{
    public class RandomSetActiveComponent : MonoBehaviour
    {
        private void Start()
        {
            var rnd = Random.Range(0, 1 + 1);
            gameObject.SetActive(rnd == 0);
        }
    }
}