using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace NKStudio
{
    public class CAARenderPass : ScriptableRenderPass
    {
        private Material _material;
        private CAA _caa;
        
        private readonly ProfilingSampler kProfilingSampler = new("Chromatic Aberration Advanced");
        
        private static class ShaderParams
        {
            public static readonly int Intensity = Shader.PropertyToID("_Intensity");
            public static readonly int RedOffset = Shader.PropertyToID("_RedOffset");
            public static readonly int GreenOffset = Shader.PropertyToID("_GreenOffset");
            public static readonly int BlueOffset = Shader.PropertyToID("_BlueOffset");
        }
        
        public bool Setup(Shader shader)
        {
            if (_material == null)
            {
                if (shader == null) {
                    Debug.LogWarning("Could not load shader. Please make sure shader is present.");
                    return false;
                }
                
                _material = CoreUtils.CreateEngineMaterial(shader);
            }
            return true;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null)
                return;

            VolumeStack stack = VolumeManager.instance.stack;
            _caa = stack.GetComponent<CAA>();
            
            CommandBuffer cmd = CommandBufferPool.Get();
            
            if (_caa.IsActive())
                DoChromaticAberration(cmd, ref renderingData);
            
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            CommandBufferPool.Release(cmd);
        }

        private void DoChromaticAberration(CommandBuffer cmd, ref RenderingData renderingData)
        {
            _material.SetFloat(ShaderParams.Intensity, _caa.GetIntensity());
            _material.SetVector(ShaderParams.RedOffset, _caa.GetRedOffset());
            _material.SetVector(ShaderParams.GreenOffset, _caa.GetGreenOffset());
            _material.SetVector(ShaderParams.BlueOffset, _caa.GetBlueOffset());
            
            using (new ProfilingScope(cmd, kProfilingSampler))
                Blit(cmd, ref renderingData, _material);
        } 
            
        public void Cleanup()
        {
             // 렌더링 종료시 호출
             CoreUtils.Destroy(_material);
        }
    }
}
