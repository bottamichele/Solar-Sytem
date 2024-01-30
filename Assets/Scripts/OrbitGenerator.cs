using UnityEngine;

/// <summary>
/// Orbit generator for a celestial body of solar system.
/// </summary>
public class OrbitGenerator
{
    /// <summary>
    /// Generate celObj's orbit with a random initial position.
    /// </summary>
    /// <param name="celObj">Celestial object</param>
    /// <param name="semiMajorAxis">celObj's semi-major axis.</param>
    /// <param name="eccentricity">celObj's eccentricity.</param>
    /// <param name="ascendingNode">celObj's ascening node.</param>
    /// <param name="argumentPerihelion">celObj's argument perihelion.</param>
    /// <param name="inclination">celObj's orbital inclination.</param>
    /// <param name="celObjToOrbit">Celestial object celObj will orbit it.</param>
    public static void GenerateEllipseOrbit(CelestialObject celObj, float semiMajorAxis, float eccentricity, float ascendingNode, float argumentPerihelion, float inclination, CelestialObject celObjToOrbit)
    {
        //Ellipse characteristics.
        float semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2));
        Vector3 f1 = new Vector3(0.0f, 0.0f, 0.0f);                                         //Left focus (occupied by celObjToOrbit).
        //Vector3 f2 = new Vector3(2 * semiMajorAxis * eccentricity, 0.0f, 0.0f);           //Right focus (occupied by celObj).
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

    /// <summary>
    /// Generate orbit of a comet near at Sun with initial random position.
    /// </summary>
    /// <param name="comet">A comet.</param>
    /// <param name="semiMajorAxis">Semi-major axis of orbit.</param>
    /// <param name="eccentricity">Orbital eccentricity.</param>
    /// <param name="ascendingNode">Ascening node of orbit.</param>
    /// <param name="argumentPerihelion">Argument perihelion of orbit</param>
    /// <param name="inclination">Orbital inclination.</param>
    /// <param name="sun">Sun this comet will orbit.</param>
    public static void GenerateOrbitCometNearSun(Comet comet, float semiMajorAxis, float eccentricity, float ascendingNode, float argumentPerihelion, float inclination, CelestialObject sun)
    {
        //Ellipse characteristics.
        float semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2));
        Vector3 f1 = new Vector3(0.0f, 0.0f, 0.0f);                                         //Left focus (occupied by sun).
        //Vector3 f2 = new Vector3(2 * semiMajorAxis * eccentricity, 0.0f, 0.0f);           //Right focus (occupied by comet).
        Vector3 ellipseCenter = new Vector3(semiMajorAxis * eccentricity, 0.0f, 0.0f);      //Center position of ellipse.

        //Perihelion characteristics.
        float perihelionDistance = semiMajorAxis * (1 - eccentricity);
        float velocityPerihelionMagnitude = Mathf.Sqrt( Constants.gravity * sun.GetMass() * Mathf.Pow(1 + eccentricity,2) / 
                                                        (semiMajorAxis * (1 - eccentricity * eccentricity)) );

        //An inistial random position from orbit is choosen.
        Vector3 position;       //Initial position.
        float distance;         //Distance between sun and initial position comet.

        do
        {
            //Choose a random position from ellipse.
            float randomAngle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
            position = ellipseCenter + new Vector3(semiMajorAxis * Mathf.Cos(randomAngle), 0.0f, semiMinorAxis * Mathf.Sin(randomAngle));

            //Calculate distance between comet and sun.
            distance = Vector3.Distance(position, f1);
        }
        while (distance >= ScaleConverter.ScaleLength(Comet.MIN_DISTANCE_TO_GENERATE));

        float velocityMagnitude = perihelionDistance * velocityPerihelionMagnitude / distance;      //Velocity at position.
        
        //Apply transformations.
        Matrix4x4 rotateEllipse = Matrix4x4.Rotate(Quaternion.Euler(0.0f, argumentPerihelion, 0.0f)) * 
                                  Matrix4x4.Rotate(Quaternion.Euler(0.0f, 0.0f, inclination)) * 
                                  Matrix4x4.Rotate(Quaternion.Euler(0.0f, ascendingNode, 0.0f));
        Matrix4x4 translateTowardsCelObjToOrbit = Matrix4x4.Translate(sun.transform.position);

        Vector3 distanceDir = rotateEllipse.MultiplyPoint3x4((position - f1).normalized);
        comet.transform.position = distance * translateTowardsCelObjToOrbit.MultiplyPoint3x4(distanceDir);
        comet.GetComponent<Rigidbody>().velocity = velocityMagnitude * Matrix4x4.Rotate(Quaternion.Euler(0.0f, -90.0f, 0.0f)).MultiplyPoint3x4(distanceDir);
    }

    /// <summary>
    /// Generate orbit of a comet created via script.
    /// </summary>
    /// <param name="comet">A comet.</param>
    /// <param name="semiMajorAxis">Semi-major axis of orbit.</param>
    /// <param name="eccentricity">Orbital eccentricity.</param>
    /// <param name="ascendingNode">Ascening node of orbit.</param>
    /// <param name="argumentPerihelion">Argument perihelion of orbit</param>
    /// <param name="inclination">Orbital inclination.</param>
    /// <param name="sun">Sun this comet will orbit.</param>
    public static void GenerateOrbitCometScript(Comet comet, float semiMajorAxis, float eccentricity, float ascendingNode, float argumentPerihelion, float inclination, CelestialObject sun)
    {
        //Ellipse characteristics.
        float semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2));
        Vector3 f1 = new Vector3(0.0f, 0.0f, 0.0f);                                         //Left focus (occupied by sun).
        //Vector3 f2 = new Vector3(2 * semiMajorAxis * eccentricity, 0.0f, 0.0f);           //Right focus (occupied by comet).
        Vector3 ellipseCenter = new Vector3(semiMajorAxis * eccentricity, 0.0f, 0.0f);      //Center position of ellipse.

        //Perihelion characteristics.
        float perihelionDistance = semiMajorAxis * (1 - eccentricity);
        float velocityPerihelionMagnitude = Mathf.Sqrt(Constants.gravity * sun.GetMass() * Mathf.Pow(1 + eccentricity, 2) /
                                                        (semiMajorAxis * (1 - eccentricity * eccentricity)));

        //An inistial position from orbit is choosen.
        float x = (-Mathf.Pow(semiMinorAxis, 2) + Mathf.Sqrt(Mathf.Pow(semiMinorAxis, 4) -
                                                             Mathf.Pow(semiMajorAxis * semiMinorAxis * eccentricity, 2) -
                                                             Mathf.Pow(semiMajorAxis * ScaleConverter.ScaleLength(Comet.MIN_DISTANCE_TO_GENERATE), 2)))
                  / (semiMajorAxis * eccentricity);
        float y = Mathf.Sqrt(Mathf.Pow(semiMinorAxis, 2) - Mathf.Pow(semiMinorAxis / semiMajorAxis, 2) * Mathf.Pow(x - semiMajorAxis * eccentricity, 2));

        Vector3 position = new Vector3(x, 0.0f, y);
        float distance = position.magnitude;
        float velocityMagnitude = perihelionDistance * velocityPerihelionMagnitude / distance;      //Velocity at position.

        //Apply transformations.
        Matrix4x4 rotateEllipse = Matrix4x4.Rotate(Quaternion.Euler(0.0f, argumentPerihelion, 0.0f)) *
                                  Matrix4x4.Rotate(Quaternion.Euler(0.0f, 0.0f, inclination)) *
                                  Matrix4x4.Rotate(Quaternion.Euler(0.0f, ascendingNode, 0.0f));
        Matrix4x4 translateTowardsCelObjToOrbit = Matrix4x4.Translate(sun.transform.position);

        Vector3 distanceDir = rotateEllipse.MultiplyPoint3x4((position - f1).normalized);
        comet.transform.position = distance * translateTowardsCelObjToOrbit.MultiplyPoint3x4(distanceDir);
        comet.GetComponent<Rigidbody>().velocity = velocityMagnitude * Matrix4x4.Rotate(Quaternion.Euler(0.0f, -90.0f, 0.0f)).MultiplyPoint3x4(distanceDir);
    }
}