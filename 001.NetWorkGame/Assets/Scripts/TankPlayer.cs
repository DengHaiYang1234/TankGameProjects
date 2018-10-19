using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : MonoBehaviour
{
    public string Name;
    public bool IsMine;

    [HideInInspector]
    public Tank tank;

    private void Awake()
    {
        tank = this.GetComponent<Tank>();
    }
}
