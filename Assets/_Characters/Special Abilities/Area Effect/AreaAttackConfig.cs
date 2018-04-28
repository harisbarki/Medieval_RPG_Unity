using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Area Attack"))]
    public class AreaAttackConfig : SpecialAbility
    {
        [Header("Area Attack Specific")]
        [SerializeField] float radius = 5f;
        [SerializeField] float damageToEachTarget = 15f;

        public float DamageToEachTarget
        {
            get
            {
                return damageToEachTarget;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
        }

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<AreaAttackBehaviour>();
            behaviourComponent.Config = this;
            behaviour = behaviourComponent;
        }
    }
}
