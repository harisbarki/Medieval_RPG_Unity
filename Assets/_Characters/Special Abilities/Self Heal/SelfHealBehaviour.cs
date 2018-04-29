using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {

        SelfHealConfig config;
        Player player;

        public SelfHealConfig Config
        {
            set
            {
                config = value;
            }
        }

        // Use this for initialization
        void Start()
        {
            player = GetComponent<Player>();
        }

        public void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
            PlayParticleEffect();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            player.AdjustHealth(-config.ExtraHealth);
        }

        private void PlayParticleEffect()
        {
            var prefab = Instantiate(config.ParticlePrefab, transform.position, Quaternion.identity);
            // TODO deicde if particle system attaches to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}
