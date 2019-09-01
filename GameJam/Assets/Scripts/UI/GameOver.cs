using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Animator summary;
    private bool Clicked = false;
    void Update()
    {
        if(Input.anyKey && !Clicked){
            summary.SetBool("show",true);
            Clicked = true;
            Debug.Log("click");
        }
    }
}
