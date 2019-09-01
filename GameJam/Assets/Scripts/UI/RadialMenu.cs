
using UnityEngine;


public class RadialMenu : MonoBehaviour
{
    private int angleCount = 9;
    private float offsetAboveHead = 0.3f;
   
    public RadialButton selected;
    public RadialButton[] PrefabArray;
    Camera m_MainCamera;

    public Transform owner;

    public void Start()
    {
        m_MainCamera = Camera.main;
        float rotationToCam = Quaternion.LookRotation(m_MainCamera.transform.position).y *60 ;
        transform.rotation =  Quaternion.Euler(0, -rotationToCam, 0);

        for (int i = 0; i < PrefabArray.Length; i++)
        {
            RadialButton newButton =  Instantiate(PrefabArray[i]) as RadialButton;
            newButton.transform.SetParent(transform,false);
            float theta = (2* Mathf.PI / angleCount) * (i);
            float xPos = Mathf.Sin(theta-0.523599f);
            float yPos = Mathf.Cos(theta-0.523599f);
            newButton.transform.localPosition = new Vector3(xPos,yPos + offsetAboveHead ,0f) * 2f;
            newButton.myMenu = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0)){
            if(selected){
                SelectedEvent();
            }
           Destroy(gameObject);
        }
    }

    void SelectedEvent(){
        //Call upgrade police here

        // Run selected button event add on.
        selected.RunEventAddOn(owner);
    }
}
