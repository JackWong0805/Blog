using UnityEngine;

/// <summary>
/// 指示灯.
/// </summary>
public class HightLED : MonoBehaviour
{
    private Material mat;

    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetEmission(mat, true);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SetEmission(mat, false);
        }
    }

    private void SetEmission(Material mat, bool emissionOn)
    {
        if (emissionOn)
        {
            mat.EnableKeyword("_EMISSION");
        }
        else
        {
            mat.DisableKeyword("_EMISSION");
        }
    }
}
