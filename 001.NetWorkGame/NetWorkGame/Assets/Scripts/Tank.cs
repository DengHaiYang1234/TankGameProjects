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


    #region 履带与车轮
    //履带
    private Transform tracks;
    //轮子
    private Transform wheels;
    //轮轴
    public List<AxleInfo> axleInfos;
    //马力
    private float motor;
    //最大马力
    private float maxMotorTorque;
    //制动
    private float brakeTorque;
    //最大制动
    private float maxBrakeTorque;
    //转向角
    private float steering;
    //最大转向角
    public float maxSteeringAngle;

    //private void WheelCtrl()
    //{
    //    motor = maxMotorTorque * Input.GetAxis("Vertical");
    //    steering = maxSteeringAngle * Input.GetAxis("Horizontal");

    //    brakeTorque = 0;
    //    foreach(AxleInfo axleInfo in axleInfos)
    //    {
    //        if (axleInfo.leftWheel.rpm > 5 && motor < 0)
    //        {
    //            brakeTorque = maxBrakeTorque;
    //        }
    //        else if (axleInfo.leftWheel.rpm < -5 && motor > 0)
    //            brakeTorque = maxBrakeTorque;
    //        continue;
    //    }
    //}

    //public void WheelUpdate()
    //{
    //    WheelCtrl();

    //    foreach(AxleInfo axleInfo in axleInfos)
    //    {
    //        if(axleInfo.steering)
    //        {
    //            axleInfo.leftWheel.steerAngle = steering;
    //            axleInfo.rightWheel.steerAngle = steering;
    //        }

    //        if(axleInfo.motor)
    //        {
    //            axleInfo.leftWheel.motorTorque = motor;
    //            axleInfo.rightWheel.motorTorque = motor;
    //        }

    //        if(true)
    //        {
    //            axleInfo.leftWheel.brakeTorque = brakeTorque;
    //            axleInfo.rightWheel.brakeTorque = brakeTorque;
    //        }

    //        if(axleInfos[1] != null && axleInfo == axleInfos[1])
    //        {
    //            WheelsRotation(axleInfos[1].leftWheel);
    //            TrackMove();
    //        }

            
            
    //    }

    //}

    //public void TrackMove()
    //{
    //    if (tracks == null)
    //        return;

    //    float offset = 0;
    //    if(wheels.GetChild(0) != null)
    //    {
    //        offset = wheels.GetChild(0).localEulerAngles.x / 90f;
    //    }

    //    foreach(Transform track in tracks)
    //    {
    //        MeshRenderer mr = tracks.gameObject.GetComponent<MeshRenderer>();
    //        if (mr == null) continue;
    //        Material mtl = mr.material;
    //        mtl.mainTextureOffset = new Vector2(0, offset);
    //    }
    //}

    //public void WheelsRotation(WheelCollider collider)
    //{
    //    if (wheels == null)
    //        return;
    //    Vector3 position;
    //    Quaternion rotation;
    //    collider.GetWorldPose(out position, out rotation);

    //    foreach (Transform wheel in wheels)
    //    {
    //        wheels.rotation = rotation;
    //    }
    //}
    #endregion

    private void Start()
    {
        if (Turret != null)
        {
            turrentEulerAngle = Turret.transform.localEulerAngles;
        }

        if (MainGun != null)
            mainGunEulerAngle = MainGun.transform.localEulerAngles;

        tracks = transform.Find("track");
        wheels = transform.Find("wheels");
    }

    public void Move(Vector2 moveVector)
    {
        positionTemp = this.transform.position;
        this.transform.Rotate(new Vector3(0, moveVector.x * TurnSpeed * Time.deltaTime, 0));
        this.transform.position += this.transform.forward * moveVector.y * TankSpeed * Time.deltaTime;
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
}


