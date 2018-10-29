using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCameraEffect : MonoBehaviour
{
    public float MaxDistance = 100;
    public Vector3 PositionShaker;

    private void Start()
    {
        CameraEffect.CameraFX = this;
    }

    Vector3 forcePower;

    public void Shake(Vector3 power,Vector3 position)
    {
        float distance = Vector3.Distance(this.transform.position, position);
        //阻尼   Mathf.Clamp:限制value的值在min和max之间， 如果value小于min，返回min。 如果value大于max，返回max，否则返回value
        float damping = (1.0f / MaxDistance) * Mathf.Clamp(MaxDistance - distance, 0, MaxDistance);
        forcePower = -power * damping;
    }

    private void Update()
    {
        forcePower = Vector3.Lerp(forcePower, Vector3.zero, Time.deltaTime * 5);
        PositionShaker = new Vector3(Mathf.Cos(Time.time * 80) * forcePower.x, Mathf.Cos(Time.time * 80) * forcePower.y, Mathf.Cos(Time.time * 80) * forcePower.z);
    }

}

public class CameraEffect : MonoBehaviour
{
    public static TankCameraEffect CameraFX;

    public static void Shake(Vector3 power, Vector3 position)
    {
        if (CameraFX != null)
        {
            CameraFX.Shake(power, position);
        }
    }
}


