using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed;
        GameObject shooter;
        float damageCaused;
        const float DESTORY_DELAY = 0.01f;

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (shooter && collision.gameObject.layer != shooter.layer)
            {
                DamageIfDamageables(collision);
            }
        }

        private void DamageIfDamageables(Collision collision)
        {
            Component damagableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damagableComponent)
            {
                (damagableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject, DESTORY_DELAY);
        }
    }
}
