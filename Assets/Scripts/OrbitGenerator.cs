using System;
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
        float randomAngle = UnityEngine.Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
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
        celObj.transform.position = translateTowardsCelObjToOrbit.MultiplyPoint3x4(distance * distanceDir);
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
            float randomAngle = UnityEngine.Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
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
        comet.transform.position = translateTowardsCelObjToOrbit.MultiplyPoint3x4(distance * distanceDir);
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
        float distanceFocus = semiMajorAxis * eccentricity;                                 //Distance between ellipse center and one fo two focus.
        Vector3 f1 = new Vector3(-distanceFocus, 0.0f, 0.0f);                               //Left focus (occupied by sun).
        //Vector3 f2 = new Vector3(distanceFocus, 0.0f, 0.0f);                              //Right focus (occupied by comet).

        //Perihelion characteristics.
        float perihelionDistance = semiMajorAxis * (1 - eccentricity);
        float velocityPerihelionMagnitude = Mathf.Sqrt(Constants.gravity * sun.GetMass() * Mathf.Pow(1 + eccentricity, 2) /
                                                        (semiMajorAxis * (1 - eccentricity * eccentricity)));

        //Position desired from orbit is choosen.
        float distanceDesired = ScaleConverter.ScaleLength(Comet.MIN_DISTANCE_TO_GENERATE);
        bool is_y1_not_skipped = false;
        bool is_y2_not_skipped = false;
        bool is_y3_not_skipped = false;
        bool is_y4_not_skipped = false;

        Func<float, bool> check_x = x => { return (x >= -distanceFocus - distanceDesired) && (x <= -distanceFocus + distanceDesired); };
        Func<float, float> computeY = x => { return Mathf.Sqrt(distanceDesired * distanceDesired - Mathf.Pow(x + distanceFocus, 2)); };

        float x1_3 = -(semiMajorAxis + distanceDesired) / eccentricity;
        float x2_4 = -(semiMajorAxis - distanceDesired) / eccentricity;
        
        is_y1_not_skipped = check_x(x1_3);
        float y1 = is_y1_not_skipped? computeY(x1_3) : float.NaN;
        
        is_y2_not_skipped=check_x(x2_4);
        float y2 = is_y2_not_skipped? computeY(x2_4) : float.NaN;

        is_y3_not_skipped = check_x(x1_3);
        float y3 = is_y3_not_skipped ? -computeY(x1_3) : float.NaN;

        is_y4_not_skipped = check_x(x2_4);
        float y4 = is_y4_not_skipped ? -computeY(x2_4) : float.NaN;

        //Destroy comet if position choosen doesn't exist.
        if (!is_y1_not_skipped && !is_y2_not_skipped && !is_y3_not_skipped && !is_y4_not_skipped)
        {
            GameObject.Destroy(comet.gameObject);
            return;
        }

        //Compute positions and velocities.
        Vector3 invalid = new Vector3(float.NaN, float.NaN, float.NaN);
        Func<Vector3, Vector3> translate = x => { return Matrix4x4.Translate(new Vector3(distanceFocus, 0.0f, 0.0f)).MultiplyPoint3x4(x); };

        Vector3 position1 = !is_y1_not_skipped? invalid : translate(new Vector3(x1_3, 0.0f, y1));    //Position adjusted for f1.
        Vector3 position1Dir = !is_y1_not_skipped ? invalid : Vector3.Normalize(new Vector3(x1_3, 0.0f, y1) - f1);
        Vector3 position2 = !is_y2_not_skipped ? invalid : translate(new Vector3(x2_4, 0.0f, y2));    //Position adjusted for f1.
        Vector3 position2Dir = !is_y2_not_skipped? invalid : Vector3.Normalize(new Vector3(x2_4, 0.0f, y2) - f1);
        Vector3 position3 = !is_y3_not_skipped ? invalid : translate(new Vector3(x1_3, 0.0f, y3));    //Position adjusted for f1.
        Vector3 position3Dir = !is_y3_not_skipped ? invalid : Vector3.Normalize(new Vector3(x1_3, 0.0f, y3) - f1);
        Vector3 position4 = !is_y4_not_skipped ? invalid : translate(new Vector3(x2_4, 0.0f, y4));    //Position adjusted for f1.
        Vector3 position4Dir = !is_y4_not_skipped ? invalid : Vector3.Normalize(new Vector3(x2_4, 0.0f, y4) - f1);
        float velocityMagnitude = perihelionDistance * velocityPerihelionMagnitude / distanceDesired;

        //Apply transformations.
        Matrix4x4 rotateEllipse = Matrix4x4.Rotate(Quaternion.Euler(0.0f, argumentPerihelion, 0.0f)) *
                                  Matrix4x4.Rotate(Quaternion.Euler(0.0f, 0.0f, inclination)) *
                                  Matrix4x4.Rotate(Quaternion.Euler(0.0f, ascendingNode, 0.0f));
        Matrix4x4 translateTowardsSun = Matrix4x4.Translate(sun.transform.position);

        Func<Vector3, Vector3> computeFinalDistance = x => { return (translateTowardsSun * rotateEllipse).MultiplyPoint3x4(x); };
        Func<Vector3, Vector3> computeFinalVelocity = x_dir => { return velocityMagnitude * Matrix4x4.Rotate(Quaternion.Euler(0.0f, -90.0f, 0.0f)).MultiplyPoint3x4(rotateEllipse.MultiplyPoint3x4(x_dir)); };
        Func<Vector3, Vector3> computeFinalPositionDir = x_dir => { return rotateEllipse.MultiplyPoint3x4(x_dir); };
        Func<Vector3, Vector3> computeFinalVelocityDir = x_dir => { return (Matrix4x4.Rotate(Quaternion.Euler(0.0f, -90.0f, 0.0f)) * rotateEllipse).MultiplyPoint3x4(x_dir); };

        Vector3 finalF1Dir = rotateEllipse.MultiplyPoint3x4(Vector3.left);
        Vector3 finalPosition1 = is_y1_not_skipped ? computeFinalDistance(position1) : invalid;
        Vector3 finalVelocity1 = is_y1_not_skipped ? computeFinalVelocity(position1Dir) : invalid;
        Vector3 finalVelocityDir1 = is_y1_not_skipped? computeFinalVelocityDir(position1Dir) : invalid;
        Vector3 finalPosition2 = is_y2_not_skipped ? computeFinalDistance(position2) : invalid;
        Vector3 finalVelocity2 = is_y2_not_skipped ? computeFinalVelocity(position2Dir) : invalid;
        Vector3 finalVelocityDir2 = is_y2_not_skipped ? computeFinalVelocityDir(position2Dir) : invalid;
        Vector3 finalPosition3 = is_y3_not_skipped ? computeFinalDistance(position3) : invalid;
        Vector3 finalVelocity3 = is_y3_not_skipped ? computeFinalVelocity(position3Dir) : invalid;
        Vector3 finalVelocityDir3 = is_y3_not_skipped ? computeFinalVelocityDir(position3Dir) : invalid;
        Vector3 finalPosition4 = is_y4_not_skipped ? computeFinalDistance(position4) : invalid;
        Vector3 finalVelocity4 = is_y4_not_skipped ? computeFinalVelocity(position4Dir) : invalid;
        Vector3 finalVelocityDir4 = is_y4_not_skipped ? computeFinalVelocityDir(position4Dir) : invalid;

        if (is_y1_not_skipped && Vector3.Dot(finalVelocityDir1, finalF1Dir) > 0.0f)
        {
            comet.transform.position = finalPosition1;
            comet.GetComponent<Rigidbody>().velocity = finalVelocity1;
        }
        else if (is_y2_not_skipped && Vector3.Dot(finalVelocityDir2, finalF1Dir) < 0.0f)
        {
            comet.transform.position = finalPosition2;
            comet.GetComponent<Rigidbody>().velocity = finalVelocity2;
        }
        else if (is_y3_not_skipped && Vector3.Dot(finalVelocityDir3, finalF1Dir) > 0.0f)
        {
            comet.transform.position = finalPosition3;
            comet.GetComponent<Rigidbody>().velocity = finalVelocity3;
        }
        else
        {
            comet.transform.position = finalPosition4;
            comet.GetComponent<Rigidbody>().velocity = finalVelocity4;
        }
    }
}