using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A comet.
/// </summary>
public class Comet : SphericalCelestialObject
{
    /* ==================================================
     * =================== CONSTANTS ====================
     * ================================================== */

    /// <summary>
    /// Maximum length of tail possible (in km) can be done by a comet.
    /// </summary>
    public const float MAXIMUM_LENGTH_TAIL_POSSIBLE = 6378.137f * 2 * 15.0f;    //15 times bigger than Earth's diameter.

    /// <summary>
    /// Minimim distance (in km) from sun that appears tail.
    /// </summary>
    const float MIN_DISTANCE_TAIL_APPEARS = 4.0f * 149597887.5f;   // = 4 U.A.

    /* ==================================================
     * ============= INSPECTOR'S PROPERTIES =============
     * ================================================== */

    /* ---------- "Orbital charateristics" Section ---------- */
    [Header("Orbital charateristics")]

    [SerializeField]
    [Tooltip("Semi-major axis of orbit (in km).")]
    float semiMajorAxis;                                    //Semi-major axis of orbital (in km).

    [SerializeField]
    [Tooltip("Eccentricity of orbit.")]
    float eccentricity;                                     //Eccentricity of orbital.

    [SerializeField]
    [Tooltip("Longitudine of ascending node (in degree) of orbit.")]
    float ascendingNode;                                    //Longitudine of ascending node (in degree).

    [SerializeField]
    [Tooltip("Argument of perihelion (in degree) of orbit.")]
    float argumentPerihelion;                               //Argument of perihelion (in degree).

    [SerializeField]
    [Tooltip("Oribital inclination (in degree).")]
    float inclination;                                      //Orbital inclination (in degree).

    [SerializeField]
    [Tooltip("Star this comet orbits.")]
    CelestialObject star;                                   //Star this comet orbits.

    /* ---------- "Comet charateristics" Section ---------- */
    [Header("Comet charateristics")]

    [SerializeField]
    [Tooltip("Maximum length of tail of this comet (in km).")]
    float maximumLengthTail;                                //Maximum length of tail of this comet (in km).

    /* ================================================== 
     * =================== VARIABLES ====================
     * ================================================== */
    float maxLengthTail;                                    //Maximum length of tail of this comet.
    float currentLengthTail;                                //Current length of tail of this comet.
    GameObject cometTail;                                   //Comet tail.

    /* ==================================================
     * ==================== METHODS =====================
     * ================================================== */

    protected new void Start()
    {
        base.Start();

        maxLengthTail = ScaleConverter.ScaleSingleAxisOfCelestialObject(maximumLengthTail);
        currentLengthTail = 0.0f;
        
        cometTail = Instantiate(GameObject.FindGameObjectWithTag("TemplateTailComet"), this.transform);
        cometTail.tag = "Untagged";

        if (eccentricity < 1.0f)
            Orbit.GenerateEllipseOrbit(this, ScaleConverter.ScaleLength(semiMajorAxis), eccentricity, ascendingNode, argumentPerihelion, inclination, star);

        ParticleSystem p = cometTail.GetComponent<ParticleSystem>();
        var mmm = p.main;
        mmm.startLifetime = maxLengthTail;
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        cometTail.transform.rotation = Quaternion.LookRotation((this.transform.position - star.transform.position).normalized);
        //cometTail.transform.rotation = Quaternion.RotateTowards(cometTail.transform.rotation, star.transform.rotation, 360);
        //cometTail.transform.rotation = Quaternion.FromToRotation(this.transform.position, star.transform.position);
    }

    void aaa()
    {
        ParticleSystem psComet = cometTail.GetComponent<ParticleSystem>();

        ParticleSystem.ShapeModule shapeComponent = psComet.shape;
        shapeComponent.enabled = true;
        shapeComponent.shapeType = ParticleSystemShapeType.Cone;
        shapeComponent.radius = 0.5f;
        shapeComponent.radiusThickness = 1.0f;
        shapeComponent.angle = 5.0f;
    }
}
