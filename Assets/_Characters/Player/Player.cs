using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

using RPG.Core;
using RPG.CameraUI;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;

        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Weapon weaponInUse;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        // Temporarilily serialized for dubbing
        [SerializeField] SpecialAbility[] abilities;

        const string DEATH_TRIGGER = "Death";
        const string ATTACK_TRIGGER = "Attack";

        AudioSource audioSource;
        Animator animator;
        float currentHealthPoints;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupAnimatorController();
            abilities[0].AttachComponentTo(gameObject);
            audioSource = GetComponent<AudioSource>();
        }


        public void TakeDamage(float damage)
        {
            if (currentHealthPoints - damage <= 0)  // player dies
            {
                StartCoroutine(KillPlayer());   //  kill player
            }
            ReduceHealth(damage);
        }

        IEnumerator KillPlayer()
        {
            animator.SetTrigger(DEATH_TRIGGER);

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            var currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }

        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length)];
            audioSource.Play();
        }


        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupAnimatorController()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimationClip();
        }

        private void PutWeaponInHand()
        {
            GameObject weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);

            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDimanentHands = dominantHands.Length;
            Assert.IsFalse(numberOfDimanentHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDimanentHands > 1, "Multiple DominantHand scripts on Player, please remove one");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                AttackTarget(enemy);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0, enemy);
            }
        }

        private void AttemptSpecialAbility(int abilityIndex, Enemy enemy)
        {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].EnergyCost;
            if (energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

        private void AttackTarget(Enemy enemy)
        {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())

            {
                animator.SetTrigger(ATTACK_TRIGGER);
                enemy.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            // Check enemy is in range
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }

    }
}