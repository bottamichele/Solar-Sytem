using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/// <summary>
/// A celestial object.
/// </summary>
public class CelestialObject : MonoBehaviour
{
    /* ==================================================
     * ============= INSPECTOR'S PROPERTIES ============= 
     * ================================================== */
    [SerializeField] float mass;                    //Mass (in kg) of celestial object.
    [SerializeField] float rotationPeriod;          //Period rotation (in days) of celestial object.
    [SerializeField] float axialTilt;               //Axial tilt (in degree) of celestial object.

    /* ==================================================
     * =================== COMPONENTS ===================
     * ================================================== */
    Rigidbody rigidbd;                              //Rigid body of celestial object.

    /* ================================================== 
     * ==================== VARIABLES ===================
     * ================================================== */
    float rotationVelocity;                         //Rotation velocity (in degrees per seconds or °/s) of celestial object.

    protected void Start()
    {
        rigidbd = GetComponent<Rigidbody>();
        rigidbd.mass = ScaleConverter.ScaleMass(mass);
        rigidbd.useGravity = false;

        transform.Rotate(0.0f, 0.0f, -axialTilt);

        rotationVelocity = 360.0f / ScaleConverter.ScaleTime(rotationPeriod * 86400);
    }

    protected void FixedUpdate()
    {
        //Do a rotation.
        transform.Rotate(Vector3.up, rotationVelocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Return mass of object celestial.
    /// </summary>
    /// <returns>Mass of object celestial.</returns>
    public float GetMass()
    {
        return rigidbd.mass;
    }
}
