using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {

        PowerAttackConfig config;

        public PowerAttackConfig Config
        {
            set
            {
                config = value;
            }
        }

        // Use this for initialization
        void Start()
        {
            print("Power attack behaviour attached to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
            PlayParticleEffect();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + config.ExtraDamage;
            useParams.target.TakeDamage(damageToDeal);
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = config.ParticlePrefab;
            var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            // TODO deicde if particle system attaches to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}
