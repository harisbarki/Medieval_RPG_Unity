using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.CameraUI;

namespace RPG.Characters
{
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [Range(.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalHitParticle;

        Character character;
        Enemy enemy;
        Animator animator;
        SpecialAbilities abilities;
        CameraRaycaster cameraRaycaster;
        WeaponSystem weaponSystem;

        void Start()
        {
            character = GetComponent<Character>();
            weaponSystem = GetComponent<WeaponSystem>();
            abilities = GetComponent<SpecialAbilities>();
            RegisterForMouseEvents();
        }

        void Update()
        {
            ScanForAbilityKeyDown();
        }

        void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                character.SetDestination(enemy.transform.position);
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                character.SetDestination(enemy.transform.position);
                abilities.AttemptSpecialAbility(0, enemy.gameObject);
            }
        }

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponSystem.GetCurrentWeapon().GetMaxAttackRange();
        }

    }
}