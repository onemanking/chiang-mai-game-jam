
using UnityEngine;


public class RadialMenu : MonoBehaviour
{
    private int angleCount = 8;
    public RadialButton selected;
    public RadialButton[] PrefabArray;
    public void Start()
    {
        for (int i = 0; i < PrefabArray.Length; i++)
        {
            RadialButton newButton =  Instantiate(PrefabArray[i]) as RadialButton;
            newButton.transform.SetParent(transform,false);
            float theta = (2* Mathf.PI / angleCount) * (i-1);
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);
            newButton.transform.localPosition = new Vector3(xPos,yPos,0f) * 4.5f;
            newButton.myMenu = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0)){
            if(selected){
                Debug.Log(selected.name);
            }
           Destroy(gameObject);
        }
    }
}
