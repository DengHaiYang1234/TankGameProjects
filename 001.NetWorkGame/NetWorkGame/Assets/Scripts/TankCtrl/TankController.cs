using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public Tank targetTank;

    TankCamera tankCamera;

    // Use this for initialization
    void Start()
    {
        tankCamera = this.GetComponent<TankCamera>();
    }

    // Update is called once per frame
    void Update()
    {

        KeyController();


    }

    void KeyController()
    {

        if (targetTank.cType != Tank.ControllType.none)
        {
            Vector2 moveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            targetTank.Move(moveVector);

            if (Input.GetMouseButton(0))
                targetTank.Shoot();
        }


        Vector2 aimVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        targetTank.Aim(aimVector);

        //targetTank.TargetSignPos();



    }




}
