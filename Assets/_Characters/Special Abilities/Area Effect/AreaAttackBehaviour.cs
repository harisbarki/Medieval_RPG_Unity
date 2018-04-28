using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;
using System;

namespace RPG.Characters
{
    public class AreaAttackBehaviour : MonoBehaviour, ISpecialAbility
    {

        AreaAttackConfig config;

        public AreaAttackConfig Config
        {
            set
            {
                config = value;
            }
        }

        // Use this for initialization
        void Start()
        {
            print("Area attack behaviour attached to " + gameObject.name);
        }

        public void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayParticleEffect();
        }

        private void PlayParticleEffect()
        {
            var prefab = Instantiate(config.ParticlePrefab, transform.position, Quaternion.identity);
            // TODO deicde if particle system attaches to player
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                            transform.position,
                            config.Radius,
                            Vector3.up,
                            config.Radius
                        );

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float damageToDeal = useParams.baseDamage + config.DamageToEachTarget;
                    damageable.TakeDamage(damageToDeal);
                }
            }
        }

    }
}
