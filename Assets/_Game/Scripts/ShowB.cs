using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowB : MonoBehaviour
{
    MeshRenderer MR;
    public bool used=false;
    private void Start()
    {
        MR = GetComponent<MeshRenderer>();
    }
    public bool Hidee()
    {
        if (!used) 
        {
            MR.enabled = false;
            used = true;
        }
        return used;
    }
}
