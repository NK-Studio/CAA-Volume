using UnityEngine;
using UnityEngine.Rendering;

namespace NKStudio
{
    public class CAAController : MonoBehaviour
    {
        public bool AutoVolume = true;
        public Volume Target;

        [Range(0f, 1f)]
        public float Intensity;
        public Vector2 RedOffset;
        public Vector2 GreenOffset;
        public Vector2 BlueOffset;

        private CAA _caa;

        private void Start()
        {
            if (AutoVolume)
                Target = FindAnyObjectByType<Volume>();

            Target.profile.TryGet(out _caa);
        }

        private void Update()
        {
            if (!_caa)
                return;

            _caa.SetIntensity(Intensity);
            _caa.SetRedOffset(RedOffset);
            _caa.SetGreenOffset(GreenOffset);
            _caa.SetBlueOffset(BlueOffset);
        }

        /// <summary>
        /// Gets the intensity value of the CAAController.
        /// </summary>
        /// <returns>The intensity value between 0 and 1.</returns>
        public float GetIntensity()
        {
            return Intensity;
        }

        /// <summary>
        /// Gets the red offset value for the CAAController.
        /// </summary>
        /// <returns>The red offset as a Vector2.</returns>
        public Vector2 GetRedOffset()
        {
            return RedOffset;
        }

        /// <summary>
        /// Gets the green offset value for the CAAController.
        /// </summary>
        /// <returns>The green offset as a Vector2.</returns>
        public Vector2 GetGreenOffset()
        {
            return GreenOffset;
        }

        /// <summary>
        /// Gets the blue offset value for the CAAController.
        /// </summary>
        /// <returns>The blue offset as a Vector2.</returns>
        public Vector2 GetBlueOffset()
        {
            return BlueOffset;
        }

        /// <summary>
        /// Sets the intensity value for the CAAController.
        /// </summary>
        /// <param name="value">The intensity value to be set. Should be between 0 and 1.</param>
        public void SetIntensity(float value)
        {
            Intensity = value;
        }

        /// <summary>
        /// Sets the red offset value for the CAAController.
        /// </summary>
        /// <param name="value">The red offset value to be set.</param>
        public void SetRedOffset(Vector2 value)
        {
            RedOffset = value;
        }

        /// <summary>
        /// Sets the green offset value for the CAAController.
        /// </summary>
        /// <param name="value">The green offset value to be set.</param>
        public void SetGreenOffset(Vector2 value)
        {
            GreenOffset = value;
        }

        /// <summary>
        /// Sets the blue offset value for the CAAController.
        /// </summary>
        /// <param name="value">The blue offset value to be set.</param>
        public void SetBlueOffset(Vector2 value)
        {
            BlueOffset = value;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (!Application.isPlaying)
            {
                Volume volume = FindAnyObjectByType<Volume>();

                if (volume.sharedProfile.TryGet(out CAA caa))
                {
                    RedOffset = caa.RedOffset.value;
                    GreenOffset = caa.GreenOffset.value;
                    BlueOffset = caa.BlueOffset.value;
                }
                else
                {
                    string msg = Application.systemLanguage == SystemLanguage.Korean
                        ? "CAA Volume이 없습니다."
                        : "No CAA Volume Found";
                    Debug.LogError(msg);
                }
            }
        }
#endif
    }
}