
using UnityEngine;

public class justDying : MonoBehaviour
{
    // Start is called before the first frame update
    private bool temp;
    public Material newMat;
    public void omaewa_shindeiru(){
        for (int i = 0; i < 10000; i++)
        {
            Debug.Log(i);
            if(i%1000 == 0){
                temp = !temp;
            }
            //Blink();
        }
    }
    public void Blink(Shader newShade){
        temp = !temp;
        if(temp){
            White(newShade);
        }else{
            Black(newShade);
        }
    }
    public void White(Shader newShade){
        foreach(MeshRenderer rend in GetComponentsInChildren<MeshRenderer>()){
            foreach(Material mat in rend.materials){
               mat.color = Color.white;
               mat.shader = newShade;
            }
        }
    }
    public void Black(Shader newShade){
        foreach(MeshRenderer rend in GetComponentsInChildren<MeshRenderer>()){
            foreach(Material mat in rend.materials){
            mat.shader = newShade;
               mat.color = Color.black;
               
            }
        }
    }
}