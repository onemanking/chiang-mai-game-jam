
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public GameObject PuaseScreen;
    public void puase(){
        PuaseScreen.SetActive(true);

        Time.timeScale = 0;
    }
    public void resume(){
        PuaseScreen.SetActive(false);

        Time.timeScale = 1;
    }
    public void MainMenu(){
        SceneManager.LoadScene("mainMenu");

        Time.timeScale = 1;
    }
}
