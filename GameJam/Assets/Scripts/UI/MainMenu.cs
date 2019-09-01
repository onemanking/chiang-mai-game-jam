

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene("Test 3D");
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
