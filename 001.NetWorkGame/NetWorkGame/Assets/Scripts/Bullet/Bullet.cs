﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100f;
    public GameObject explode;
    public float maxLiftTime = 2f;
    public float instaniateTime = 0f;
    public float att = 30f;

    private void Start()
    {
        instaniateTime = Time.time;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Time.time - instaniateTime > maxLiftTime)
            Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explode, transform.position, transform.rotation);
        Destroy(gameObject);

        Tank tank = collision.gameObject.GetComponent<Tank>();
        if (tank != null)
        {
            float att = GetAtt();
            tank.BeAttacked(att);
        }

    }


    private float GetAtt()
    {
        float at = 100 - (Time.time - instaniateTime)*40;
        if (att < 1)
            att = 1;

        return att;
    }

}
