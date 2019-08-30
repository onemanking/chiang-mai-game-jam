
using UnityEngine;
using UnityEngine.EventSystems;

public class RadialButton : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    public RadialMenu myMenu;
    private void Awake() {
    }
     public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Output to console the GameObject's name and the following message
        Debug.Log("Cursor Entering " + name + " GameObject");
        transform.localScale = new Vector3(2f,2f,2f);
        myMenu.selected = this;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        Debug.Log("Cursor Exiting " + name + " GameObject");
        transform.localScale = new Vector3(1f,1f,1f);
    }

    void Update()
    {
        
    }
}
