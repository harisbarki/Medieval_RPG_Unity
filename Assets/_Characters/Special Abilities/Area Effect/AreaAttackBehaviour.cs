using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

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

        public void Use(AbilityUseParams useParams)
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

        // Use this for initialization
        void Start()
        {
            print("Area attack behaviour attached to " + gameObject.name);
        }
    }
}
