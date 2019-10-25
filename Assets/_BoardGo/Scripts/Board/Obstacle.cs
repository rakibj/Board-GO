using System;
using UnityEngine;

namespace _BoardGo.Scripts.Board
{
    [RequireComponent(typeof(BoxCollider))]
    public class Obstacle : MonoBehaviour
    {
        private BoxCollider boxCollider;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0f,0f,.5f);
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
}
