using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PostProcessingOutline
{
    [Serializable]
    [PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.BeforeStack, "Roystan/Post Process Outline")]
    public sealed class PostProcessOutline : PostProcessEffectSettings {
        public BoolParameter    depthEdge       = new ();
        public BoolParameter    normalEdge      = new ();
        public BoolParameter    useColor        = new ();
        public IntParameter     scale           = new () { value = 1 };
        public FloatParameter   edgeDepthFactor = new () { value = 100 };
        [Range(0, 1)]
        public FloatParameter   normalThreshold = new () { value = 0.4f };
    }

    public sealed class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
    {
        private static readonly int DeptEdge        = Shader.PropertyToID("_DeptEdge");
        private static readonly int NormalEdge      = Shader.PropertyToID("_NormalEdge");
        private static readonly int UseColor        = Shader.PropertyToID("_UseColor");
        private static readonly int Scale           = Shader.PropertyToID("_Scale");
        private static readonly int EdgeDepthFactor = Shader.PropertyToID("_EdgeDepthFactor");
        private static readonly int NormalThreshold = Shader.PropertyToID("_NormalThreshold");

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Roystan/Outline Post Process"));
        
            sheet.properties.SetInt(DeptEdge, settings.depthEdge ? 1 : 0);
            sheet.properties.SetInt(NormalEdge, settings.normalEdge ? 1 : 0);
            sheet.properties.SetInt(UseColor, settings.useColor ? 1 : 0);
            sheet.properties.SetFloat(Scale, settings.scale);
            sheet.properties.SetFloat(EdgeDepthFactor, settings.edgeDepthFactor);
            sheet.properties.SetFloat(NormalThreshold, settings.normalThreshold);
        
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
