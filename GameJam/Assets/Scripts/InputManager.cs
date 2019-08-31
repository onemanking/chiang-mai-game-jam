using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private static InputManager _instance;

	public static InputManager Instance
	{
		get
		{
			if (_instance == null)
			{
				var go = new GameObject();
				go.AddComponent<InputManager>();
				go.name = "InputManager";
			}
			return _instance;
		}
	}

	private CharacterBase currentSelected;

	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(this);
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "Officer")
			{
				Debug.LogError("Select : " + hitInfo.collider.name);
				currentSelected = hitInfo.collider.GetComponent<CharacterBase>();
			}
		}
	}
}
