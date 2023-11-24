using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Meteors : MonoBehaviour
{
    public float fuseTime = 2;
    public int damage = 1;
    [SerializeField]
    private VisualEffect boomEffect;

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
        yield return new WaitForSeconds(fuseTime);
        boomEffect.Play();
        yield return null;
        while (boomEffect.HasAnySystemAwake())
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
