using System.Collections.Generic;
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
