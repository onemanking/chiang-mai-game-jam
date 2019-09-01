using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dying : MonoBehaviour
{
    public ParticleSystem prefabs;
    public void Spawn()
    {
        ParticleSystem bleeding = Instantiate(prefabs,transform.position, Quaternion.Euler(-90,0,0));
    }

}
