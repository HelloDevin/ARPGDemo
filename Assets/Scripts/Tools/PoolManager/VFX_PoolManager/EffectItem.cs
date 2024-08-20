using UnityEngine;

namespace ZZZ
{
    public class EffectItem : PoolItemBase
    {
        //不具备参考性：因为会受到慢动作影响，所以loop取消掉最好
        [SerializeField, Header("特效播放时间")] private float playTime;

        [SerializeField, Header("特效播放的速度")] private float playSpeed;

        private ParticleSystem[] ParticleSystem;

        private void Awake()
        {
            ParticleSystem = GetComponentsInChildren<ParticleSystem>();

            for (int i = 0; i < ParticleSystem.Length; i++)
            {
                VFXManager.Instance.AddVFX(ParticleSystem[i], playSpeed);
            }
        }

        protected override void Spawn()
        {
            StartPlay();
        }

        private void StartPlay()
        {
            for (int i = 0; i < ParticleSystem.Length; i++)
            {
                ParticleSystem[i].Play();
            }

            TimerManager.Instance.AddTimer(playTime, StartReCycle);
        }

        private void StartReCycle()
        {
            gameObject.SetActive(false);
        }

        protected override void Recycle()
        {
            for (int i = 0; i < ParticleSystem.Length; i++)
            {
                ParticleSystem[i].Stop();
            }
        }
    }
}