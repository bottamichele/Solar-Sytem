using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit
{
    public static void GenerateEllipseOrbit(CelestialObject celObj, float semiMajorAxis, float eccentricity, float ascendingNode, float argumentPerihelion, float inclination, CelestialObject celObjToOrbit)
    {
        //Ellipse characteristics.
        float semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2));
        Vector3 f1 = new Vector3(0.0f, 0.0f, 0.0f);                                         //Left focus (occupied by celObjToOrbit).
        Vector3 f2 = new Vector3(2 * semiMajorAxis * eccentricity, 0.0f, 0.0f);             //Right focus (occupied by celObj).
        Vector3 ellipseCenter = new Vector3(semiMajorAxis * eccentricity, 0.0f, 0.0f);      //Center position of ellipse.

        //Perihelion characteristics.
        float perihelionDistance = semiMajorAxis * (1 - eccentricity);
        float velocityPerihelionMagnitude = Mathf.Sqrt( Constants.gravity * celObjToOrbit.GetMass() * Mathf.Pow(1 + eccentricity,2) / 
                                                        (semiMajorAxis * (1 - eccentricity * eccentricity)) );

        //Choose a random position from ellipse.
        float randomAngle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
        Vector3 position = ellipseCenter + new Vector3(semiMajorAxis * Mathf.Cos(randomAngle), 0.0f, semiMinorAxis * Mathf.Sin(randomAngle));

        //Calculate distance from celObjToOrbit and velocity.
        float distance = Vector3.Distance(position, f1);
        float velocityMagnitude = perihelionDistance * velocityPerihelionMagnitude / distance;

        //Apply transformations.
        Matrix4x4 rotateEllipse = Matrix4x4.Rotate(Quaternion.Euler(0.0f, argumentPerihelion, 0.0f)) * 
                                  Matrix4x4.Rotate(Quaternion.Euler(0.0f, 0.0f, inclination)) * 
                                  Matrix4x4.Rotate(Quaternion.Euler(0.0f, ascendingNode, 0.0f));
        Matrix4x4 translateTowardsCelObjToOrbit = Matrix4x4.Translate(celObjToOrbit.transform.position);

        Vector3 distanceDir = rotateEllipse.MultiplyPoint3x4((position - f1).normalized);
        celObj.transform.position = distance * translateTowardsCelObjToOrbit.MultiplyPoint3x4(distanceDir);
        celObj.GetComponent<Rigidbody>().velocity = velocityMagnitude * Matrix4x4.Rotate(Quaternion.Euler(0.0f, -90.0f, 0.0f)).MultiplyPoint3x4(distanceDir);
    }
}