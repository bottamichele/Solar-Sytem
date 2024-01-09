using UnityEngine;

public class Earth : Planet
{
    [Header("Earth characteristics")]

    [SerializeField]
    [Tooltip("Factor of clouds speed.")]
    float cloudsSpeed;

    protected new void Start()
    {
        base.Start();

        Material materialEarth = GetComponent<MeshRenderer>().materials[0];
        materialEarth.SetFloat("_CloudsSpeed", cloudsSpeed/GetRotationPeriod());
        //materials[0].name;
    }
}
