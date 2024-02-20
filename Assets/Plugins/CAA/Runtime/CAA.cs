using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace NKStudio
{
    [Serializable, VolumeComponentMenu("Custom/CAA")]
    public class CAA : VolumeComponent, IPostProcessComponent
    {
        [SerializeField, Tooltip("화면이 중심점에서 멀어지는 정도를 설정합니다. 기본값은 0.0입니다.")]
        public ClampedFloatParameter Intensity = new ClampedFloatParameter(0f, 0f, 1f);

        [SerializeField, Tooltip("빨간색 오프셋, 기본값은 0.0입니다.")]
        public Vector2Parameter RedOffset = new Vector2Parameter(new Vector2(0.1f, -0.1f));

        [SerializeField, Tooltip("녹색 오프셋, 기본값은 0.0입니다.")]
        public Vector2Parameter GreenOffset = new Vector2Parameter(new Vector2(0f, 0f));

        [SerializeField, Tooltip("파란색 오프셋, 기본값은 0.0입니다.")]
        public Vector2Parameter BlueOffset = new Vector2Parameter(new Vector2(-0.1f, 0.1f));

        public bool IsActive()         => Intensity.value > 0;
        public bool IsTileCompatible() => false;

        public float   GetIntensity()   => Intensity.GetValue<float>();
        public Vector2 GetRedOffset()   => RedOffset.GetValue<Vector2>();
        public Vector2 GetGreenOffset() => GreenOffset.GetValue<Vector2>();
        public Vector2 GetBlueOffset()  => BlueOffset.GetValue<Vector2>();

        public void SetIntensity(float intensity)       => Intensity.Override(intensity);
        public void SetRedOffset(Vector2 redOffset)     => RedOffset.Override(redOffset);
        public void SetGreenOffset(Vector2 greenOffset) => GreenOffset.Override(greenOffset);
        public void SetBlueOffset(Vector2 blueOffset)   => BlueOffset.Override(blueOffset);
    }
}
