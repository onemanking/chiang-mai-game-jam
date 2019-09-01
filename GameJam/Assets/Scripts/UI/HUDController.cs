
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDController : MonoBehaviour
{
    public GameObject PauseScreen;
    public TextMeshProUGUI m_hResumeText;



    public void puase(){
        PauseScreen.SetActive(true);

        if (GameManager.Instance.gameState == GameManager.GameState.Over)
        {
            m_hResumeText.text = "Try Again";
        }

        Time.timeScale = 0;
    }
    public void resume(){
        Time.timeScale = 1;

        PauseScreen.SetActive(false);

        if(GameManager.Instance.gameState == GameManager.GameState.Over)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
    }
    public void MainMenu(){
        SceneManager.LoadScene("mainMenu");

        Time.timeScale = 1;
    }
}
