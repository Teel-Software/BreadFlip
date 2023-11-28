using UnityEngine;

namespace BreadFlip.Generation
{
    public class PropsElement : MonoBehaviour
    {
        [field: SerializeField] public ChunkTypes ChunkTypes { get; private set; }
        [field: SerializeField] public bool CanRotate { get; private set; } = true;
    }
}