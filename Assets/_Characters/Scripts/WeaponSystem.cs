using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig currentWeaponConfig;

        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";
        float lastHitTime = 0;
        GameObject weaponObject;
        Animator animator;
        Character character;
        GameObject target;

        void Start()
        {
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
            character = GetComponent<Character>();

        }

        void Update()
        {

        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            // todo setup coroutine for attacking repeatedly
        }

        public WeaponConfig GetCurrentWeapon()
        {
            return currentWeaponConfig;
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        void SetAttackAnimation()
        {
            var animatorOverrideController = character.GetAnimatorOverrideController();
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
        }

        GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                print(currentWeaponConfig.GetAdditionalDamage());
                target.GetComponent<HealthSystem>().TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        float CalculateDamage()
        {
            return baseDamage + currentWeaponConfig.GetAdditionalDamage();
        }
    }
}
