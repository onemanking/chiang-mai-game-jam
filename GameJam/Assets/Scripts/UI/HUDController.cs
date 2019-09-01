
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public GameObject PuaseScreen;
    public void puase(){
        PuaseScreen.SetActive(true);
    }
    public void resume(){
        PuaseScreen.SetActive(false);
    }
    public void MainMenu(){
        SceneManager.LoadScene("mainMenu");
    }
}
