
using UnityEngine;

public class justDying : MonoBehaviour
{
    // Start is called before the first frame update
    private bool temp;
    public void omaewa_shindeiru(){
        for (int i = 0; i < 10000; i++)
        {
            Debug.Log(i);
            if(i%1000 == 0){
                temp = !temp;
            }
            Blink();
        }
    }
    public void Blink(){
        if(temp){
            White();
        }else{
            Black();
        }
    }
    public void White(){
        foreach(MeshRenderer rend in GetComponentsInChildren<MeshRenderer>()){
            foreach(Material mat in rend.materials){
               mat.color = Color.white;
            }
        }
    }
    public void Black(){
        foreach(MeshRenderer rend in GetComponentsInChildren<MeshRenderer>()){
            foreach(Material mat in rend.materials){
               mat.color = Color.white;
            }
        }
    }
}