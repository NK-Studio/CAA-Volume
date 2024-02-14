using UnityEngine.Rendering.Universal;
using NKStudio;
using UnityEngine;
using UnityEngine.Rendering;

public class CAARenderFeature : ScriptableRendererFeature
{
    private Material _material;
    private CAARenderPass _caaPass;

    /// <inheritdoc/>
    public override void Create()
    {
        if (!_material)
            _material = CoreUtils.CreateEngineMaterial("Hidden/Universal Render Pipeline/CAA");

        if (_caaPass == null)
            _caaPass = new CAARenderPass(_material);
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!renderingData.postProcessingEnabled)
            return;

        if (renderingData.cameraData.cameraType != CameraType.Game)
            return;

        _caaPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        _caaPass.ConfigureInput(ScriptableRenderPassInput.Color);

        renderer.EnqueuePass(_caaPass);
    }
    
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        _caaPass.SetTarget(renderer.cameraColorTargetHandle);
    }

    protected override void Dispose(bool disposing)
    {
        _caaPass?.Dispose();
        _caaPass = null;

        CoreUtils.Destroy(_material);
        _material = null;
    }
}
