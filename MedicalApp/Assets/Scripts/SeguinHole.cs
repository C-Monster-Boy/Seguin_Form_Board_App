using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedicalApp.Core
{
    public class SeguinHole : MonoBehaviour
    {
        public SeguinObjectType holeType;


        public bool IsInside(PolygonCollider2D holeCollider, PolygonCollider2D objectCollider)
        {
            Bounds enterableBounds = holeCollider.bounds;
            Bounds enteringBounds = objectCollider.bounds;

            Vector2 center = enteringBounds.center;
            Vector2 extents = enteringBounds.extents;
            Vector2[] enteringVerticles = new Vector2[4];

            enteringVerticles[0] = new Vector2(center.x + extents.x, center.y + extents.y);
            enteringVerticles[1] = new Vector2(center.x - extents.x, center.y + extents.y);
            enteringVerticles[2] = new Vector2(center.x + extents.x, center.y - extents.y);
            enteringVerticles[3] = new Vector2(center.x - extents.x, center.y - extents.y);

            foreach (Vector2 verticle in enteringVerticles)
            {
                if (!enterableBounds.Contains(verticle))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

