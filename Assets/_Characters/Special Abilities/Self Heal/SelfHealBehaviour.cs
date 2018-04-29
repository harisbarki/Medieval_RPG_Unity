using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {

        SelfHealConfig config = null;
        Player player = null;
        AudioSource audioSource = null;

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
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams useParams)
        {
            DealDamage(useParams);
            PlayParticleEffect();
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }

        private void DealDamage(AbilityUseParams useParams)
        {
            player.Heal(config.ExtraHealth);
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = config.ParticlePrefab;
            var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            prefab.transform.parent = transform;
            // TODO deicde if particle system attaches to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}
