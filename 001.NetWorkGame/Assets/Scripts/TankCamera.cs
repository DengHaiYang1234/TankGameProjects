using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCamera : MonoBehaviour
{
    public GameObject Target;
    public Camera[] Cameras;

    public float Distance = 5;

    public Vector3 offest = new Vector3(-0.62f, 11.42f, -93.9f);

    public float Damping = 0.5f;

    void UpdateCamera()
    {
        if (!Target)
            return;

        Vector3 VectorUp = Target.transform.up;
        this.transform.LookAt(Target.transform.position - VectorUp * Distance);

        Vector3 positionTarget = (Target.transform.position + VectorUp * Distance) + offest;
        this.transform.position = Vector3.Lerp(this.transform.position, positionTarget, Damping) + CameraEffect.CameraFX.PositionShaker;
    }

    private void LateUpdate()
    {
        UpdateCamera();
    }

    private void Update()
    {
        if(Target == null)
        {
            TankPlayer tankTarget = (TankPlayer)GameObject.FindObjectOfType(typeof(TankPlayer));
            if(tankTarget != null && tankTarget.tank != null)
            {
                if(tankTarget.tank.MainGun)
                {
                    Target = tankTarget.tank.MainGun.gameObject;
                }
                else
                {
                    if (tankTarget.tank.Turret)
                        Target = tankTarget.tank.Turret.gameObject;
                    else
                        Target = tankTarget.tank.gameObject;
                }
            }
        }
    }
}
