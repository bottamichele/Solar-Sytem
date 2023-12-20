using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : SphericalCelestialObject
{
    [SerializeField] float temperature;     //Temperature (in Kelvin) of Sun.

    protected new void Start()
    {
        base.Start();
        this.transform.localScale /= 5;
    }
}
