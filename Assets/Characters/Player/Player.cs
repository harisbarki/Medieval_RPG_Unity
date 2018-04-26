using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{

    [SerializeField] int enemyLayer = 10;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange = 2f;

    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;
    float currentHealthPoints;
    bool canAttack = true;

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
    }

    void Start()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClicked;
        currentHealthPoints = maxHealthPoints;
    }

    void OnMouseClicked(RaycastHit raycastHit, int layerHit)
    {
        if(layerHit == enemyLayer && canAttack)
        {
            canAttack = false;
            StartCoroutine(ResetCanAttack());
            GameObject enemy = raycastHit.collider.gameObject;

            // Check enemy is in range
            if((transform.position - enemy.transform.position).magnitude >= maxAttackRange)
            {
                return;
            }

            currentTarget = enemy;
            Enemy enemyComponent = currentTarget.GetComponent<Enemy>();
            enemyComponent.TakeDamage(damagePerHit);
        }
    }

    IEnumerator ResetCanAttack()
    {
        yield return new WaitForSeconds(minTimeBetweenHits);
        canAttack = true;
    }
}
