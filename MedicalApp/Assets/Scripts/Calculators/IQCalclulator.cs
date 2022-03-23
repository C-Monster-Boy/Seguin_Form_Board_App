using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedicalApp.Core
{
    public class IQCalclulator
    {
        public float GetIq(float mentalAge, float chronologicalAge)
        {
            return (mentalAge / chronologicalAge) * 100f;
        }
    }
}

