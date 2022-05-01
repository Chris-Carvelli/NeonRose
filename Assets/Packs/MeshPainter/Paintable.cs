using UnityEngine;

namespace Packs.MeshPainter
{
    public class Paintable : MonoBehaviour
    {
        private const int TEXTURE_SIZE = 1024;
        
        private static int maskTextureID = Shader.PropertyToID("_MaskTex");
        
        public float extendsIslandOffset = 1;
        
        public RenderTexture _extendIslandsRenderTexture;
        public RenderTexture _uvIslandsRenderTexture;
        public RenderTexture _maskRenderTexture;
        public RenderTexture _supportTexture;

        private Renderer _renderer;

        public RenderTexture GetMask()      => _maskRenderTexture;
        public RenderTexture GetUVIslands() => _uvIslandsRenderTexture;
        public RenderTexture GetExtend()    => _extendIslandsRenderTexture;
        public RenderTexture GetSupport()   => _supportTexture;
        public Renderer      GetRenderer()  => _renderer;
        
        private void Start()
        {
            _maskRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            _maskRenderTexture.filterMode = FilterMode.Bilinear;

            _extendIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            _extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

            _uvIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            _uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

            _supportTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
            _supportTexture.filterMode =  FilterMode.Bilinear;

            _renderer = GetComponent<Renderer>();
            _renderer.material.SetTexture(maskTextureID, _extendIslandsRenderTexture);

            MeshPainterManager.Instance.InitTextures(this);
        }
        
        private void OnDisable(){
            _maskRenderTexture.Release();
            _uvIslandsRenderTexture.Release();
            _extendIslandsRenderTexture.Release();
            _supportTexture.Release();
        }
    }
}