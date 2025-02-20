using UnityEngine;

public class DisableMeshRenderer : MonoBehaviour
{
    void Start()
    {
        if(GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;
    }
}