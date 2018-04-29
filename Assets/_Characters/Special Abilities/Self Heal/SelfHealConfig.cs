using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Self Heal"))]
    public class SelfHealConfig : SpecialAbility
    {
        [Header("Self Heal Specific")]
        [SerializeField] float extraHealth = 50f;

        public float ExtraHealth
        {
            get
            {
                return extraHealth;
            }
        }

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<SelfHealBehaviour>();
            behaviourComponent.Config = this;
            behaviour = behaviourComponent;
        }
    }
}
