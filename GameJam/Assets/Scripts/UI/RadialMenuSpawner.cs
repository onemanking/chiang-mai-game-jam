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

    public void spawnMenu(){
        RadialMenu newMenu = Instantiate(menuPrefab) as RadialMenu;
        Debug.Log("Spawned");
        newMenu.transform.SetParent(transform,false);
        newMenu.transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));

    }
}
