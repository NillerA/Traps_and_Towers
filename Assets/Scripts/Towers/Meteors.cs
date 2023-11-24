using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Meteors : MonoBehaviour
{
    public float fallSpeed = 2;
    public int damage = 1;
    public float explotionRange = 1;
    [SerializeField]
    private VisualEffect boomEffect;
    [SerializeField]
    private GameObject meteorVisual;

    private void Start()
    {
        //Explode();
        StartCoroutine(Explode());
    }

    void OnCollisionEnter(Collision coll)
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        while (transform.position.y > 0.1f)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }
        boomEffect.Play();
        for (int i = 0; i < WaveManager.Instance.activeEnemies.Count; i++)
        {
            if(Vector3.Distance(transform.position, WaveManager.Instance.activeEnemies[i].transform.position) < explotionRange)
                WaveManager.Instance.activeEnemies[i].GetComponent<EnemyMovement>().TakeDamage(damage);
        }
        yield return null;
        meteorVisual.SetActive(false);
        while (boomEffect.HasAnySystemAwake())
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
