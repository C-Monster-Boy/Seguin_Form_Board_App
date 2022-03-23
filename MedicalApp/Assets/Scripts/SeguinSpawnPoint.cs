using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedicalApp.Core
{
    public class SeguinSpawnPoint : MonoBehaviour
    {
        public bool isSeguinObjectAttached;
        public GameObject attachedSeguinShape;

        public void AttachShape(GameObject attachedShape)
        {
            isSeguinObjectAttached = true;
            attachedSeguinShape = attachedShape;
        }

        public void DetachShape()
        {
            isSeguinObjectAttached = false;
            attachedSeguinShape = null;
        }
    }
}

