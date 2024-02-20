using UnityEngine.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering.Universal.Utility;

namespace NKStudio
{
    public class CAARenderFeature : ScriptableRendererFeature
    {
        private Shader _shader;
        private CAARenderPass _caaPass;
        private UniversalRendererData _universalRendererData;

        public override void Create()
        {
            name = "Chromatic Aberration Advanced";
            _caaPass = new CAARenderPass();
            _caaPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;

            _shader = Shader.Find("Hidden/Universal Render Pipeline/CAA");
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!_universalRendererData)
                _universalRendererData = URPRendererUtility.GetUniversalRendererData();

            if (!URPRendererUtility.IsPostProcessEnabled(_universalRendererData, ref renderingData))
                return;

            if (renderingData.cameraData.cameraType != CameraType.Game)
                return;

            if (_caaPass.Setup(_shader))
                renderer.EnqueuePass(_caaPass);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _caaPass?.Cleanup();
            }
        }
    }
}