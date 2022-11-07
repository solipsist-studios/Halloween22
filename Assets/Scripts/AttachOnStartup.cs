using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachOnStartup : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;

    private void Start()
    {
        transform.SetParent(this.parentTransform);
    }
}
