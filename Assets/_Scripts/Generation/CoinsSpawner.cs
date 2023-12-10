using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BreadFlip.Generation
{
    public class CoinsSpawner : MonoBehaviour
    {
        /// <summary>
        /// Позиция относительно положения игрока. Это стартовая позиция дуги
        /// </summary>
        [SerializeField] private Vector3 _startPos;

        /// <summary>
        /// Финальная позиция дуги
        /// </summary>
        [SerializeField] private Vector3 _finalPos;

        // public void SpawnCoins(int coinsAmount)
        // {
            
        //     int coinsDistance = points.Length / coinsAmount;

        //     for(var i = 10; i < points.Length; i += coinsDistance)
        //     {
                
        //     }
        // }

        // private Vector3[] GetCoinPoints(Vector3 origin, float minY)
        // {
        //     var points = new Vector3[100];

        //     for (var i = 0; i < points.Length; i++)
        //     {
        //         var time = i * 0.1f;

        //         points[i] = origin + speed * time + Physics.gravity * time * time / 2f;

        //         if (points[i].y < minY)
        //         {
        //             break;
        //         }
        //     }
        //     return points;
        // }
        
#region Unnecessary
        // [SerializeField] private Vector3 _rangeBetweenCoins = new Vector3 (0, 2, 3);

        // private List<Vector3> GetArcPoints(Vector3 start, Vector3 end, int coinsAmount)
        // {
        //     var angle = Vector3.Angle(start, end);

        //     List<Vector3> points = new List<Vector3>();

        //     // float radAngle = angle * (Mathf.PI / 180);

        //     for(Vector3 i = start; i.magnitude < end.magnitude; i += _rangeBetweenCoins)
        //     {
        //         points.Add(i);
        //     }
            
        //     return points;
        // }
#endregion
    }
}
