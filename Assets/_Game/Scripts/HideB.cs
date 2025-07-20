using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideB : MonoBehaviour
{
    MeshRenderer MR;
    public bool used=false;
    private void Start()
    {
        MR = GetComponent<MeshRenderer>();
    }
    public bool Showw()
    {
        if (!used)
        {
            MR.enabled = true;
            used = true;
        }
        return used;
    }
}
