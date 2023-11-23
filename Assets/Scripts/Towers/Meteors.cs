using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteors : MonoBehaviour
{
    public float fuseTime;

    void Explode()
    {
        var exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(gameObject, exp.duration);
    }
    private void Start()
    {
        Invoke("Explode", fuseTime);
    }


    void OnCollisionEnter(Collision coll)
    {
        Explode();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
