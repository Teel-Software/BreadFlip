using UnityEngine;

namespace BreadFlip.Generation.Props
{
    public class PaintingPropImageChanger : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Texture[] _imageVariant;
        
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        private void OnValidate()
        {
            _meshRenderer ??= GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            var randomIndex = Random.Range(0, _imageVariant.Length);
            var imageMaterial = _meshRenderer.materials[1];
            
            imageMaterial.SetTexture(MainTex, _imageVariant[randomIndex]);
        }
    }
}