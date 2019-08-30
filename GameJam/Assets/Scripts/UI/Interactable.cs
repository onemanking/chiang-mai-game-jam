using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    void OnMouseDown() {
        RadialMenuSpawner.spawner.spawnMenu();
        Debug.Log("clicked  ");
    }
}
