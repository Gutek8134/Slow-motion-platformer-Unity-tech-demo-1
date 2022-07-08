using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not used anywhere ATM
public class BillboardController : MonoBehaviour
{
    [SerializeField] Transform cam;

    //In late update make object you are attached to look at the camera
    //Called in late to minimize jabbering
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
