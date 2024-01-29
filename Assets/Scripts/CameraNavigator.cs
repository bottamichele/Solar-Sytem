using UnityEngine;

/// <summary>
/// A camera navigator to move the main camera of the scene around solar system.
/// </summary>
public class CameraNavigator : MonoBehaviour
{
    /* ==================================================
     * =================== CONSTANTS ====================
     * ================================================== */
    
    /// <summary>
    /// Camera's speed.
    /// </summary>
    const float SPEED = 2.0f;

    /// <summary>
    /// Sensitivity mouse to rotate camera.
    /// </summary>
    const float SENSITIVITY = 3.0f;

    /* ==================================================
     * =================== VARIABLES ====================
     * ================================================== */
    Vector2 currentRotation;

    /* ==================================================
     * ==================== METHODS =====================
     * ================================================== */

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        MoveCamera();
        RotateCamera();
    }

    void MoveCamera()
    {
        //Move camera foward.
        if (Input.GetKey(KeyCode.W))
            transform.position += transform.forward.normalized  * SPEED * Time.fixedDeltaTime;

        //Move camera backward.
        if (Input.GetKey(KeyCode.S))
            transform.position -= transform.forward.normalized  * SPEED * Time.fixedDeltaTime;

        //Strafe camera towards right.
        if (Input.GetKey(KeyCode.D))
            transform.position += transform.right.normalized    * SPEED * Time.fixedDeltaTime;

        //Strage camera towards left.
        if (Input.GetKey(KeyCode.A))
            transform.position -= transform.right.normalized    * SPEED * Time.fixedDeltaTime;

        //Move camera towards up.
        if (Input.GetKey(KeyCode.Space))
            transform.position += transform.up.normalized       * SPEED * Time.fixedDeltaTime;

        //Move camera towards down.
        if (Input.GetKey(KeyCode.LeftControl))
            transform.position -= transform.up.normalized       * SPEED * Time.fixedDeltaTime;
    }

    void RotateCamera()
    {
        currentRotation.x += SENSITIVITY * Input.GetAxis("Mouse X");
        currentRotation.y += SENSITIVITY * Input.GetAxis("Mouse Y");
        currentRotation.y = Mathf.Clamp(currentRotation.y, -89.5f, 89.5f);

        transform.localRotation = Quaternion.Euler(-currentRotation.y, currentRotation.x, 0.0f);
    }
}
