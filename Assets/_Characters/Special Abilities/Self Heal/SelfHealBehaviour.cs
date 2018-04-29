using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        HealthSystem characterHealth = null;

        void Start()
        {
            characterHealth = GetComponent<HealthSystem>();
        }

		public override void Use(GameObject target)
		{
            PlayAbilitySound();
            characterHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
		}
    }
}