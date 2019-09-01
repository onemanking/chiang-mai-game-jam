
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject PuaseScreen;
    public void puase(){
        PuaseScreen.SetActive(true);
    }
    public void resume(){
        PuaseScreen.SetActive(false);
    }
}
