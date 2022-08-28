﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    //Траектория движения выисляется путем линейной интерполяции
    // крифой Безье по двум и более точкам
    [Header("Set in Inspector: Enemy_3")]
    public float lifetTime = 5f;

    [Header("Set Dynamically: Enemy_3")]
    public float birthTime;
    public Vector3[] points;
    void Start()
    {
        points = new Vector3[3];

        //Начальная позиция уже определена в Main.SpawnEnemy()
        points[0] = pos;
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;
        Vector3 v;

        //Случайно выбрать среднюю точку ниже нижней границы экрана
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;

        //Случайно выбрать конечную точку выше верхней границы экрана
        v = Vector3.zero;
        v.x = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        birthTime = Time.time;
    }
    public override void Move()
    {
        //Кривые вычисляются на основе значения u между 0 и 1
        float u = (Time.time - birthTime) / lifetTime;
        if (u > 1) {
            Main.Delete(this.gameObject);
            Destroy(this.gameObject);
            return;
        }
        //Интерполировать Кривую безье тремя точками
        Vector3 p01, p12;
        u = u - 0.2f * Mathf.Sin(u * Mathf.PI*2);
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        pos = (1 - u) * p01 + u * p12;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (health <= 0) {
            Main.Delete(this.gameObject);
        }
        if (showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
    }
}