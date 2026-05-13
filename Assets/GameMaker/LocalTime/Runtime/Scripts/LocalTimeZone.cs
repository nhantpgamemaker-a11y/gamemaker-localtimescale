using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameMaker.LocalTime.Runtime
{
    [RequireComponent(typeof(Collider))]
    public class LocalTimeZone : MonoBehaviour
    {
        [SerializeField] private float _timeScale = 1f;

        public float TimeScale => _timeScale;

        private List<ILocalTimeZoneReceiver> _receivers = new List<ILocalTimeZoneReceiver>();
        
        #region  UnityEngine
        void OnValidate()
        {
            if (TryGetComponent<Collider>(out var collider))
            {
                if (collider is BoxCollider || collider is SphereCollider || collider is CapsuleCollider)
                    collider.isTrigger = true;
                else
                {
                    Debug.LogWarning($"LocalTimeZone requires a BoxCollider, SphereCollider, or CapsuleCollider set as trigger. Current collider: {collider.GetType().Name}", this);
                    Debug.LogWarning("Please replace the collider with a supported type and set it as trigger to ensure proper functionality of LocalTimeZone.", this); 
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            var receiver = other.GetComponentInParent<ILocalTimeZoneReceiver>();
            if(receiver != null )
            {
                _receivers.Add(receiver);
                receiver.OnAddZone(this);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            var receiver = other.GetComponentInParent<ILocalTimeZoneReceiver>();
            if (receiver != null && _receivers.Contains(receiver))
            {
                _receivers.Remove(receiver);
                receiver.OnRemoveZone(this);
            }
        }
        private void OnDrawGizmos()
        {
            if (TryGetComponent<Collider>(out var collider))
            {
                Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
                Gizmos.matrix = transform.localToWorldMatrix;
                if (collider is BoxCollider box)
                {
                    Gizmos.DrawWireCube(box.center, box.size);
                }
                else if (collider is SphereCollider sphere)
                {
                    Gizmos.DrawWireSphere(sphere.center, sphere.radius);
                }
                else if (collider is CapsuleCollider capsule)
                {
                    DrawCapsuleCollider(capsule);}
            }
        }
        private void DrawCapsuleCollider(CapsuleCollider capsule)
        {
            var oldMatrix = Gizmos.matrix;

            Gizmos.matrix =
                capsule.transform.localToWorldMatrix;

            Vector3 center = capsule.center;

            float radius = capsule.radius;
            float height = Mathf.Max(
                capsule.height,
                radius * 2f);

            int direction = capsule.direction;

            Vector3 axis = direction switch
            {
                0 => Vector3.right,
                1 => Vector3.up,
                2 => Vector3.forward,
                _ => Vector3.up
            };

            float cylinder =
                height * 0.5f - radius;

            Vector3 offset =
                axis * cylinder;

            Vector3 top =
                center + offset;

            Vector3 bottom =
                center - offset;

            // Sphere caps
            Gizmos.DrawWireSphere(top, radius);
            Gizmos.DrawWireSphere(bottom, radius);

            // Build perpendicular axes
            Vector3 sideA;
            Vector3 sideB;

            if (direction == 1)
            {
                sideA = Vector3.right * radius;
                sideB = Vector3.forward * radius;
            }
            else if (direction == 0)
            {
                sideA = Vector3.up * radius;
                sideB = Vector3.forward * radius;
            }
            else
            {
                sideA = Vector3.right * radius;
                sideB = Vector3.up * radius;
            }

            // Cylinder lines
            Gizmos.DrawLine(
                top + sideA,
                bottom + sideA);

            Gizmos.DrawLine(
                top - sideA,
                bottom - sideA);

            Gizmos.DrawLine(
                top + sideB,
                bottom + sideB);

            Gizmos.DrawLine(
                top - sideB,
                bottom - sideB);

            Gizmos.matrix = oldMatrix;
        }
        #endregion
    }
}