using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuSpawner : MonoBehaviour
{
    public static RadialMenuSpawner spawner;
    public RadialMenu menuPrefab;
    void Awake()
    {
        spawner = this;
        Debug.Log("ins created");
    }

    public void spawnMenu(Transform owner){
        RadialMenu newMenu = Instantiate(menuPrefab) as RadialMenu;
        Debug.Log("Spawned");
        newMenu.transform.SetParent(transform,false);
        newMenu.transform.position = owner.position;

        // Set click owner.
        newMenu.owner = owner;
    }
}
