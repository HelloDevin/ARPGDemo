using System.Collections.Generic;
using UnityEngine;

namespace ZZZ
{
    public class VFXManager : SingletonMono<VFXManager>
    {
        [SerializeField] private List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        [SerializeField, Header("特效播放倍率")] private float _speedMult;
        
        public List<ParticleSystem> allParticleSystems => particleSystems;

        public void AddVFX(ParticleSystem particleSys, float speedMult)
        {
            particleSystems.Add(particleSys);
            foreach (var particle in particleSystems)
            {
                var main = particle.main;
                main.simulationSpeed = speedMult;
            }
        }

        public void PauseVFX()
        {
            foreach (var particle in allParticleSystems)
            {
                var main = particle.main;
                main.simulationSpeed = 0f;
            }
        }

        public void SetVFXSpeed(float speedMult)
        {
            foreach (var particle in allParticleSystems)
            {
                var main = particle.main;
                main.simulationSpeed = speedMult;
            }
        }

        public void ResetVXF()
        {
            foreach (var particle in allParticleSystems)
            {
                var main = particle.main;
                main.simulationSpeed = _speedMult;
            }
        }
    }
}