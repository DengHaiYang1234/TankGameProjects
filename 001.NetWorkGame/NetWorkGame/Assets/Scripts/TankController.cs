using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public Tank targetTank;

    TankCamera tankCamera;

	// Use this for initialization
	void Start ()
    {
        tankCamera = this.GetComponent<TankCamera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        KeyController();
	}

    void KeyController()
    {
        Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        targetTank.Move(moveVector);
    }

    
}
