using Cinemachine;
using UnityEngine;

namespace ZZZ
{
    public class CameraHitFeel : SingletonMono<CameraHitFeel>
    {
        [SerializeField] private CinemachineImpulseSource _cinemachineImpulseSource;

        #region 震屏

        public void CameraShake(float shakeForce)
        {
            if (shakeForce == 0)
            {
                return;
            }

            _cinemachineImpulseSource.GenerateImpulseWithForce(shakeForce);
        }

        #endregion


        #region 时间控制

        public void StartSlowTime(float timeScale)
        {
            Time.timeScale = timeScale;
        }

        public void EndSlowTime()
        {
            Time.timeScale = 1f;
        }

        #endregion
    }
}