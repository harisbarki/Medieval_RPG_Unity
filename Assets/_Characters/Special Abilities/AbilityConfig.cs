using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

namespace RPG.Characters
{
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }

    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip audioClip = null;

        protected ISpecialAbility behaviour;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public float EnergyCost
        {
            get
            {
                return energyCost;
            }
        }

        public GameObject ParticlePrefab
        {
            get
            {
                return particlePrefab;
            }
        }

        public AudioClip GetAudioClip()
        {
            return audioClip;
        }

        public void Use(AbilityUseParams abilityUseParams)
        {
            behaviour.Use(abilityUseParams); 
        }
    }

    public interface ISpecialAbility
    {
        void Use(AbilityUseParams useParams);
    }
}
