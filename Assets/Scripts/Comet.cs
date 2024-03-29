using Unity.VisualScripting;
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
    const float MAXIMUM_LENGTH_TAIL_POSSIBLE = 2 * 6378.137f * 5.0f;    //5 times bigger of Earth's diameter.

    /// <summary>
    /// Minimim distance (in km) from sun that appears tail.
    /// </summary>
    const float MIN_DISTANCE_TAIL_APPEARS = 4.0f * 149597887.5f;   // = 4 U.A.

    /// <summary>
    /// Minimum distance (in km) to generate a comet via script.
    /// </summary>
    public const float MIN_DISTANCE_TO_GENERATE = 4.0f * 149597887.5f;

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

    [Header("Other options")]

    [SerializeField]
    [Tooltip("Is this comet created via editor?")]
    bool isCreatedByEditor = false;

    /* ================================================== 
     * =================== VARIABLES ====================
     * ================================================== */
    float minDistanceTailAppears;

    /* ---------- Comet tail charateristics ---------- */
    float maxLengthTail;                                    //Maximum length of tail of this comet.
    float currentLengthTail;                                //Current length of tail of this comet.
    GameObject cometTail;                                   //Comet tail.
    ParticleSystem psCometTail;                             //cometTail's particle system.

    /* ==================================================
     * ==================== METHODS =====================
     * ================================================== */

    protected new void Start()
    {
        base.Start();

        //Scaling.
        minDistanceTailAppears = ScaleConverter.ScaleLength(MIN_DISTANCE_TAIL_APPEARS);

        //Set comet tail charateristics.
        maxLengthTail = ScaleConverter.ScaleSingleAxisOfCelestialObject(MAXIMUM_LENGTH_TAIL_POSSIBLE);
        currentLengthTail = 0.0f;
        
        //Set comet tail to this game object.
        cometTail = Instantiate(GameObject.FindGameObjectWithTag("TemplateTailComet"), this.transform);
        cometTail.tag = "Untagged";
        cometTail.layer = gameObject.layer;
        psCometTail = cometTail.GetComponent<ParticleSystem>();
        psCometTail.Clear();

        if (isCreatedByEditor)
            GenerateOrbit();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

        //Update comet tail's direction towards Sun.
        cometTail.transform.rotation = Quaternion.LookRotation((this.transform.position - star.transform.position).normalized);

        //Update length of tail.
        float currentDistance = Vector3.Distance(this.transform.position, star.transform.position);
        if (currentDistance <= minDistanceTailAppears)
            currentLengthTail = maxLengthTail * (1.0f - currentDistance / minDistanceTailAppears);
        else
            currentLengthTail = 0.0f;

        var psTailMain = psCometTail.main;
        psTailMain.startLifetime = currentLengthTail / psTailMain.startSpeed.constant;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(this.tag != "GenericComet")
        {
            //Change some properties of cometTail's particle system.
            psCometTail.Stop();
            psCometTail.Clear();

            var psTailMain = psCometTail.main;
            psTailMain.duration = 1.0f;

            psCometTail.Play();
            
            psTailMain.prewarm = true;
            psTailMain.loop = false;

            //Change some properties of cometTail.
            cometTail.transform.parent = null;
            cometTail.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            cometTail.name = this.name + " Tail";
            cometTail.AddComponent<ParticleSystemAutodestroy>();

            if(collision.collider.gameObject.name.Contains("Sun"))
            {
                GameObject explosion = Instantiate(GameObject.FindGameObjectWithTag("TemplateExplosionCometSunCollision"), cometTail.transform.position, cometTail.transform.rotation);
                explosion.name = this.name + " Explosion";
                explosion.tag = "Untagged";
                explosion.layer = 0;
                explosion.AddComponent<ParticleSystemAutodestroy>();
            }

            //Destroy this comet and not tail.
            Destroy(this.gameObject);
        }
    }

    /* ================================================== 
     * ================= PUBLIC METHODS =================
     * ================================================== */

    /// <summary>
    /// Set orbital charateristics of this comet.
    /// </summary>
    /// <param name="semiMajorAxis">Semi-major axis of orbital (in km).</param>
    /// <param name="eccentricity">Eccentricity of orbital.</param>
    /// <param name="ascendingNode">Longitudine of ascending node (in degree).</param>
    /// <param name="argumentPerihelion">Argument of perihelion (in degree).</param>
    /// <param name="inclination">Orbital inclination (in degree).</param>
    /// <param name="star">Star this comet orbits.</param>
    public void SetOrbitalCharateristics(float semiMajorAxis, float eccentricity, float ascendingNode, float argumentPerihelion, float inclination, CelestialObject star)
    {
        this.semiMajorAxis = semiMajorAxis;
        this.eccentricity = eccentricity;
        this.ascendingNode = ascendingNode;
        this.argumentPerihelion = argumentPerihelion;
        this.inclination = inclination;
        this.star = star;
    }

    /// <summary>
    /// Generate orbit of this comet.
    /// </summary>
    /// <param name="isBeginState">Is begin state of scene for comet created via script?</param>
    public void GenerateOrbit(bool isBeginState=false)
    {
        if (isCreatedByEditor)
            OrbitGenerator.GenerateEllipseOrbit(this, ScaleConverter.ScaleLength(semiMajorAxis), eccentricity, ascendingNode, argumentPerihelion, inclination, star);
        else if(isBeginState)
            OrbitGenerator.GenerateOrbitCometNearSun(this, ScaleConverter.ScaleLength(semiMajorAxis), eccentricity, ascendingNode, argumentPerihelion, inclination, star);
        else
            OrbitGenerator.GenerateOrbitCometScript(this, ScaleConverter.ScaleLength(semiMajorAxis), eccentricity, ascendingNode, argumentPerihelion, inclination, star);
    }
}
