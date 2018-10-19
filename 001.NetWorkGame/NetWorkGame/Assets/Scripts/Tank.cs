using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    //炮塔
    public GameObject Turret;
    public GameObject MainGun;
    public Vector3 TurretBaseAxis = new Vector3(0, 1, 0);
    public Vector3 MainGunBaseAxis = new Vector3(1, 0, 0);

    public float TurretSpeed = 20;
    public float TankSpeed = 10;
    public float TurnSpeed = 20;

    public float MainGunMinTurnX = -10;
    public float MainGunMaxTurnX = 45;

    private Vector3 turrentEulerAngle;
    private Vector3 mainGunEulerAngle;
    private Vector3 positionTemp;

    public float FlipRatio = 0.7f;


    private void Start()
    {
        if(Turret != null)
        {
            turrentEulerAngle = Turret.transform.localEulerAngles;
        }

        if (MainGun != null)
            mainGunEulerAngle = MainGun.transform.localEulerAngles;
    }

    public void Move(Vector2 moveVector)
    {
        

        positionTemp = this.transform.position;
        this.transform.Rotate(new Vector3(0, moveVector.x * TurnSpeed * Time.deltaTime, 0));
        this.transform.position += this.transform.forward * moveVector.y * TankSpeed * Time.deltaTime;

    }




}
