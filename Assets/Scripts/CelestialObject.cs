using UnityEngine;

/// <summary>
/// A celestial object.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CelestialObject : MonoBehaviour
{
    /* ==================================================
     * ============= INSPECTOR'S PROPERTIES ============= 
     * ================================================== */
    [Header("Charateristics")]

    [SerializeField] 
    [Tooltip("Mass (in kg)")]
    float mass;                                     //Mass (in kg) of celestial object.

    [SerializeField]
    [Tooltip("Rotation period (in day)")]
    float rotationPeriod;                           //Period rotation (in days) of celestial object.

    [SerializeField]
    [Tooltip("Axial tilt (in degree)")]
    float axialTilt;                                //Axial tilt (in degree) of celestial object.

    /* ==================================================
     * =================== COMPONENTS ===================
     * ================================================== */
    protected Rigidbody rigidbd;                    //Rigid body of celestial object.

    /* ================================================== 
     * ==================== VARIABLES ===================
     * ================================================== */
    protected float rotationVelocity;               //Rotation velocity (in degrees per seconds or °/s) of celestial object.


    /* ==================================================
     * ==================== METHODS =====================
     * ================================================== */

    protected void Start()
    {
        rigidbd = gameObject.GetComponent<Rigidbody>();
        rigidbd.mass = ScaleConverter.ScaleMass(mass);
        rigidbd.useGravity = false;

        transform.Rotate(0.0f, 0.0f, -axialTilt);

        rotationVelocity = 360.0f / ScaleConverter.ScaleTime(rotationPeriod * 86400);
    }

    protected void FixedUpdate()
    {
        //Do a rotation.
        transform.Rotate(Vector3.up, rotationVelocity * Time.fixedDeltaTime);

        //Update move by gravity force.
        CelestialObject[] celestialObjects = FindObjectsOfType<CelestialObject>();
        for(int i = 0; i < celestialObjects.Length; i++)
        {
            if (celestialObjects[i].gameObject != this.gameObject)
            {
                float distance = Vector3.Distance(celestialObjects[i].transform.position, transform.position);
                float forceGravityMagnitude = Constants.gravity * this.GetMass() * celestialObjects[i].GetMass() / (distance * distance);
                Vector3 forceDirection = Vector3.Normalize(celestialObjects[i].transform.position - this.transform.position);

                rigidbd.AddForce(forceGravityMagnitude * forceDirection, ForceMode.Force);
            }
        }
    }

    /* ==================================================
     * ================= PUBLIC METHODS =================
     * ================================================== */

    /// <summary>
    /// Mass of celestial object.
    /// </summary>
    /// <returns>Mass.</returns>
    public float GetMass()
    {
        return ScaleConverter.ScaleMass(this.mass);
    }

    /// <summary>
    /// Rotation period of celestial object.
    /// </summary>
    /// <returns>Rotation period.</returns>
    public float GetRotationPeriod()
    {
        return ScaleConverter.ScaleTime(86400 * this.rotationPeriod);
    }
}
