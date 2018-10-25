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
    private float rotationX = 0;

    public float WheelRotateSpeed = 50f;
    private Transform wheels;
    private TankTrackAnimation tracks;

    private void Awake()
    {
        tracks = GetComponentInChildren<TankTrackAnimation>();
    }

    private void Start()
    {
        if (Turret != null)
        {
            turrentEulerAngle = Turret.transform.localEulerAngles;
        }

        if (MainGun != null)
            mainGunEulerAngle = MainGun.transform.localEulerAngles;

        
        wheels = transform.Find("wheels");
    }

    public void Move(Vector2 moveVector)
    {
        positionTemp = this.transform.position;
        this.transform.Rotate(new Vector3(0, moveVector.x * TurnSpeed * Time.deltaTime, 0));
        this.transform.position += this.transform.forward * moveVector.y * TankSpeed * Time.deltaTime;
        //履带滚动
        if (tracks)
            tracks.MoveTrack(new Vector2(Mathf.Clamp(moveVector.y + moveVector.x, -1, 1), 0));
    }

    public void Aim(Vector2 aimVector)
    {
        if (Turret != null)
        {
            float rotationY = Turret.transform.localEulerAngles.y + aimVector.x * TurretSpeed * Time.deltaTime;
            Turret.transform.localEulerAngles =
            new Vector3
                (
                    (TurretBaseAxis.x * rotationY) + ((1 - TurretBaseAxis.x) * turrentEulerAngle.x),
                    (TurretBaseAxis.y * rotationY) + ((1 - TurretBaseAxis.y) * turrentEulerAngle.y),
                    (TurretBaseAxis.z * rotationY) + ((1 - TurretBaseAxis.z) * turrentEulerAngle.z)
                );
        }
        if(MainGun != null)
        {
            rotationX += aimVector.y * TurretSpeed * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, MainGunMinTurnX, MainGunMaxTurnX);

            MainGun.transform.localEulerAngles =
                new Vector3(
                    (MainGunBaseAxis.x * -rotationX) + mainGunEulerAngle.x,
                    (MainGunBaseAxis.y * -rotationX) + mainGunEulerAngle.y,
                    (MainGunBaseAxis.z * -rotationX) + mainGunEulerAngle.z
                    );
        }
    }

    public void WheelsRotate(Vector2 moveVector)
    {
        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(moveVector.y * WheelRotateSpeed * Time.deltaTime, 0,0);
        }
    }
}


