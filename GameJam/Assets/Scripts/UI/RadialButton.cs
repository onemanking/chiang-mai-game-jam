
using UnityEngine;
using UnityEngine.EventSystems;

public class RadialButton : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    public RadialMenu myMenu;

    [Header("Event Add On")]
    public UI_BaseEventAddOn[] m_arrEventAddOn;
    
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

    #region Event Run

    /// <summary>
    /// Run all event add on in this button.
    /// </summary>
    public void RunEventAddOn(Transform owner)
    {
        if (m_arrEventAddOn == null || m_arrEventAddOn.Length <= 0)
            return;

        for(int i = 0; i < m_arrEventAddOn.Length; i++)
        {
            m_arrEventAddOn[i]?.Run(owner);
        }
    }

    #endregion
}
