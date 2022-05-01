using UnityEngine;
using UnityEngine.Rendering;

namespace Packs.MeshPainter
{
    public class MeshPainterManager : MonoBehaviour
    {
        private static int _prepareUVID = Shader.PropertyToID("_PrepareUV");
        private static int _positionID = Shader.PropertyToID("_PainterPosition");
        private static int _hardnessID = Shader.PropertyToID("_Hardness");
        private static int _strengthID = Shader.PropertyToID("_Strength");
        private static int _radiusID = Shader.PropertyToID("_Radius");
        private static int _blendOpID = Shader.PropertyToID("_BlendOp");
        private static int _colorID = Shader.PropertyToID("_PainterColor");
        private static int _textureID = Shader.PropertyToID("_MainTex");
        private static int _uvOffsetID = Shader.PropertyToID("_OffsetUV");
        private static int _uvIslandsID = Shader.PropertyToID("_UVIslands");
        
        public static MeshPainterManager Instance;
        
        public Shader sprayPaintMask;
        public Shader extendIslands;
        
        private CommandBuffer _buffer;
        private Material _paintMaterial;
        
        [Header("Debug")]
        // public RenderTexture mask;      
        // public RenderTexture uvIslands; 
        // public RenderTexture extend;    
        // public RenderTexture support;   
        // public Renderer rend;           
        private Material _extendMaterial;
        private void Awake()
        {
            Instance = this;
            
            _buffer = new CommandBuffer();
            _buffer.name = "CommandBuffer - " + name;
            
            _paintMaterial = new Material(sprayPaintMask);
            _extendMaterial = new Material(extendIslands);
        }

        public void InitTextures(Paintable paintable){
            RenderTexture mask = paintable.GetMask();
            RenderTexture uvIslands = paintable.GetUVIslands();
            RenderTexture extend = paintable.GetExtend();
            RenderTexture support = paintable.GetSupport();
            Renderer rend = paintable.GetRenderer();

            _buffer.SetRenderTarget(mask);
            _buffer.SetRenderTarget(extend);
            _buffer.SetRenderTarget(support);

            _paintMaterial.SetFloat(_prepareUVID, 1);
            _buffer.SetRenderTarget(uvIslands);
            _buffer.DrawRenderer(rend, _paintMaterial, 0);

            Graphics.ExecuteCommandBuffer(_buffer);
            _buffer.Clear();
        }
        
        public void Paint(
            Paintable paintable,
            Vector3 pos,
            float radius = 1f,
            float hardness = .5f,
            float strength = .5f,
            Color? color = null
        )
        {
            RenderTexture mask      = paintable.GetMask();
            RenderTexture uvIslands = paintable.GetUVIslands();
            RenderTexture extend    = paintable.GetExtend();
            RenderTexture support   = paintable.GetSupport();
            Renderer rend           = paintable.GetRenderer();
            
            _paintMaterial.SetFloat(_prepareUVID, 0);
            _paintMaterial.SetVector(_positionID, pos);
            _paintMaterial.SetFloat(_hardnessID, hardness);
            _paintMaterial.SetFloat(_strengthID, strength);
            _paintMaterial.SetFloat(_radiusID, radius);
            _paintMaterial.SetTexture(_textureID, support);
            _paintMaterial.SetColor(_colorID, color ?? Color.red);
            _extendMaterial.SetFloat(_uvOffsetID, paintable.extendsIslandOffset);
            _extendMaterial.SetTexture(_uvIslandsID, uvIslands);
            
            _buffer.SetRenderTarget(mask);
            _buffer.DrawRenderer(rend, _paintMaterial, 0);

            _buffer.SetRenderTarget(support);
            _buffer.Blit(mask, support);

            _buffer.SetRenderTarget(extend);
            _buffer.Blit(mask, extend, _extendMaterial);

            Graphics.ExecuteCommandBuffer(_buffer);
            _buffer.Clear();
        }
    }
}