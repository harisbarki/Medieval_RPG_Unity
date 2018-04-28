using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbility
    {
        [Header("Power Attack Specific")]
        [SerializeField] float extraDamage = 10f;

        public float ExtraDamage
        {
            get
            {
                return extraDamage;
            }
        }

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.Config = this;
            behaviour = behaviourComponent;
        }
    }
}
