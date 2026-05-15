using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 3f;

    private Vector3 direction;
    private float damage;

    private bool isCrit = false;

    public void Init(Vector3 dir, float dmg)
    {
        direction = dir.normalized;

        // GAB scales crit chance (each point above 1 adds 10% crit chance)
        if (PlayerStats.Instance != null)
        {
            float critChance = (PlayerStats.Instance.gab - 1) * 0.1f;
            if (Random.value < critChance)
            {
                dmg   *= 2f;
                isCrit = true;
            }
        }

        damage = dmg;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            DamagePopup.Create(other.ClosestPoint(transform.position), damage, isCrit);
            Destroy(gameObject);
            return;
        }

        Boss boss = other.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            DamagePopup.Create(other.ClosestPoint(transform.position), damage, isCrit);
            Destroy(gameObject);
        }
    }
}
