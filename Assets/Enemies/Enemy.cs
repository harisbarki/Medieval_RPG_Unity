﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable
{

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float chaseRadius = 6f;

    [SerializeField] float attackRadius = 4f;
    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float secondsBetweenShots = 1f;

    [SerializeField] GameObject projectileToUse; 
    [SerializeField] GameObject projectileSocket;
    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

    bool isAttacking = false;
    float currentHealthPoints;
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

    public float healthAsPercentage
    {
        get
        {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if(currentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(damagePerShot);
        Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position + aimOffset).normalized;

        float projectileSpeed = projectileComponent.projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }

    IEnumerator ResetIsAttacking()
    {
        yield return new WaitForSeconds(secondsBetweenShots);
        isAttacking = false;
    }

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
        currentHealthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(ResetIsAttacking());
            SpawnProjectile();
        }

        if (distanceToPlayer <= chaseRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255f, 0, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = new Color(0, 0, 255f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
