using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    void OnMouseDown() {
        RadialMenuSpawner.spawner.spawnMenu(transform.position);
        Debug.Log("clicked  ");
    }
}
