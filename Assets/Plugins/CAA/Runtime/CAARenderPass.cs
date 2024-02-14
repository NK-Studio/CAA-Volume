using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace NKStudio
{
    public class CAARenderPass : ScriptableRenderPass
    {
        private readonly Material _material;
        private RTHandle _colorTargetHandle;
        private RTHandle _tempHandle;
        
        private const string BufferName = "CAA Pass";
        private readonly ProfilingSampler _profilingSampler = new("Chromatic Aberration Advanced");
        
        private static class CAAShaderParams
        {
            public static readonly int Intensity = Shader.PropertyToID("_Intensity");
            public static readonly int RedOffset = Shader.PropertyToID("_RedOffset");
            public static readonly int GreenOffset = Shader.PropertyToID("_GreenOffset");
            public static readonly int BlueOffset = Shader.PropertyToID("_BlueOffset");
        }
        
        private void RefreshShaderParams(CAA caa)
        {
            _material.SetFloat(CAAShaderParams.Intensity, caa.GetIntensity());
            _material.SetVector(CAAShaderParams.RedOffset, caa.GetRedOffset());
            _material.SetVector(CAAShaderParams.GreenOffset, caa.GetGreenOffset());
            _material.SetVector(CAAShaderParams.BlueOffset, caa.GetBlueOffset());
        }
        
        public CAARenderPass(Material material)
        {
            _material = material;
        }
        
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = (int)DepthBits.None;
            
            // 임시 렌더 텍스쳐를 생성합니다.
            RenderingUtils.ReAllocateIfNeeded(ref _tempHandle, desc, name: "_TemporaryColorTexture");
            
            // 렌더링 대상으로 설정
            ConfigureTarget(_colorTargetHandle);
        }
        
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null)
                return;
            
            CAA caa = VolumeManager.instance.stack.GetComponent<CAA>();

            if (!caa.IsActive())
                return;

            CommandBuffer cmd = CommandBufferPool.Get(BufferName);
            using (new ProfilingScope(cmd, _profilingSampler))
            {
                RefreshShaderParams(caa);
                Blitter.BlitCameraTexture(cmd, _colorTargetHandle, _tempHandle, _material, 0);
                Blitter.BlitCameraTexture(cmd, _tempHandle, _colorTargetHandle);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            CommandBufferPool.Release(cmd);
        }

        // 이 렌더 패스 실행 중에 생성된 할당된 리소스를 정리합니다.
        // public override void OnCameraCleanup(CommandBuffer cmd)
        // {
        // }
        
        public void SetTarget(RTHandle colorHandle)
        {
            _colorTargetHandle = colorHandle;
        }
        public void Dispose()
        {
            _colorTargetHandle?.Release();
            _tempHandle?.Release();
        }
    }
}
