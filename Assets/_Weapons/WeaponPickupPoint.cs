using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Characters;

namespace RPG.Weapons
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {

        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickupSFX;

        AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if(!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }
        }

        void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            audioSource.PlayOneShot(pickupSFX);
            FindObjectOfType<Player>().PutWeaponInHand(weaponConfig);
        }

        void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            weapon.transform.position = Vector3.zero;
            Instantiate(weapon, gameObject.transform);
        }
    }
}
