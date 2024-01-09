using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Mesh of Saturn's rings.
/// </summary>
public class SaturnRings : MonoBehaviour
{
    /* ================================================== 
     * ============= INSPECTOR'S PROPERTIES =============
     * ================================================== */
    [SerializeField]
    [Tooltip("Radius of rings")]
    float radius;                 //Radius of rings.

    [SerializeField]
    [Tooltip("Tickness of rings")] 
    float tickness;               //Tickness of rings.

    [Range(3, 64)]
    [Tooltip("Number of sides for cicle mesh")]
    public int numSides;         //Number of sides for circle mesh.

    /* ==================================================
     * ==================== VARIABLE ====================
     * ==================================================*/
    Mesh mesh;                   //Mesh of Saturn's rings.

    /* ================================================== 
     * ==================== COMPONENT =================== 
     * ================================================== */
    MeshFilter meshFilter;


    void Start()
    {
        mesh = new Mesh();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        CreateMesh();
    }

    void CreateMesh()
    {
        int numOfQuads = 4 * numSides * 2;
        Vector3[] meshVertices = new Vector3[numOfQuads];
        Vector3[] meshNormals = new Vector3[numOfQuads];
        int[] meshIndices = new int[numOfQuads];
        Vector2[] meshUvs = new Vector2[numOfQuads];
        tickness = Mathf.Clamp01(tickness);

        for (int i = 0; i < numSides; i++)
        {
            //Create quad.
            float angle1 = i * (2*Mathf.PI / numSides);
            float angle2 = (i + 1) * (2*Mathf.PI / numSides);

            Vector3 dir1 = new Vector3(Mathf.Cos(angle1), 0.0f, Mathf.Sin(angle1));
            Vector3 dir2 = new Vector3(Mathf.Cos(angle2), 0.0f, Mathf.Sin(angle2));

            //Create top quad.
            meshVertices[8 * i] = radius * dir1;
            meshNormals[8 * i]  = Vector3.up;
            meshUvs[8 * i]      = new Vector2(1.0f, 0.0f);
            meshIndices[8 * i]  = 8 * i;

            meshVertices[8 * i + 1] = (1 - tickness) * radius * dir1;
            meshNormals[8 * i + 1]  = Vector3.up;
            meshUvs[8 * i + 1]      = new Vector2(0.0f, 0.0f);
            meshIndices[8 * i + 1]  = 8 * i + 1;

            meshVertices[8 * i + 2] = radius * dir2;
            meshNormals[8 * i + 2]  = Vector3.up;
            meshUvs[8 * i + 2]      = new Vector2(1.0f, 1.0f);
            meshIndices[8 * i + 2]  = 8 * i + 3;

            meshVertices[8 * i + 3] = (1 - tickness) * radius * dir2;
            meshNormals[8 * i + 3]  = Vector3.up;
            meshUvs[8 * i + 3]      = new Vector2(0.0f, 1.0f);
            meshIndices[8 * i + 3]  = 8 * i + 2;

            //Create bottom quad.
            meshVertices[8 * i + 4] = radius * dir1;
            meshNormals[8 * i + 4]  = Vector3.down;
            meshUvs[8 * i + 4]      = new Vector2(1.0f, 0.0f);
            meshIndices[8 * i + 4]  = 8 * i + 5;

            meshVertices[8 * i + 5] = (1 - tickness) * radius * dir1;
            meshNormals[8 * i + 5]  = Vector3.down;
            meshUvs[8 * i + 5]      = new Vector2(0.0f, 0.0f);
            meshIndices[8 * i + 5]  = 8 * i + 4;

            meshVertices[8 * i + 6] = radius * dir2;
            meshNormals[8 * i + 6]  = Vector3.down;
            meshUvs[8 * i + 6]      = new Vector2(1.0f, 1.0f);
            meshIndices[8 * i + 6]  = 8 * i + 6;

            meshVertices[8 * i + 7] = (1 - tickness) * radius * dir2;
            meshNormals[8 * i + 7]  = Vector3.down;
            meshUvs[8 * i + 7]      = new Vector2(0.0f, 1.0f);
            meshIndices[8 * i + 7]  = 8 * i + 7;
        }

        //Set mesh.
        mesh.Clear();
        mesh.SetVertices(meshVertices);
        mesh.SetNormals(meshNormals);
        mesh.SetUVs(0, meshUvs);
        mesh.SetIndices(meshIndices, MeshTopology.Quads, 0);
    }
}
