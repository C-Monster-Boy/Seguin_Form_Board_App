using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedicalApp.Core
{
    [CreateAssetMenu(fileName = "New Seguin Object", menuName = "SO/Seguin Object")]
    public class SO_SeguinShape : ScriptableObject
    {
        public Sprite seguinImage;
        public SeguinObjectType objectType;
        public Vector3 scale;
    }
}

