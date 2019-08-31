
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Image[] Background;

    [SerializeField] private Image[] skillImage;

    private float[] cooldown = {0f,0f,0f,0f};
    private float[] cooldownRate = {0.75f,0.5f,0.42f,0.35f};
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cooldown.Length; i++)
        {
           if(cooldown[i] > 1){
               skillImage[i].color = new Color32(255,255,255,255);
           }
           cooldown[i] += cooldownRate[i]*Time.deltaTime/2;
        }
        for (int i = 0; i < cooldown.Length; i++)
        {
            Background[i].fillAmount = cooldown[i];
        }
    }
}
