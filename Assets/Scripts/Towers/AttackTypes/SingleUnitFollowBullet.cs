using System.Collections;
using UnityEngine;

public class SingleUnitFollowBullet : MonoBehaviour
{

    private Transform target;
    public float speed = 70f;
    public int damage = 1;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;

        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        target.GetComponent<EnemyMovement>().TakeDamage(damage);
        Destroy(gameObject);
    }
}
