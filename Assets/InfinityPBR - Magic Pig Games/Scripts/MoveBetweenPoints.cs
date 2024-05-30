using System;
using UnityEngine;

namespace MagicPigGames
{
    [System.Serializable]
    public class MoveBetweenPoints : MonoBehaviour
    {
        public Vector3[] localPoints = Array.Empty<Vector3>();
        public float speed = 1f;

        private int _currentPoint = 0;
        private Vector3[] _worldPoints;
        
        private void Start()
        {
            _worldPoints = new Vector3[localPoints.Length];
            for (int i = 0; i < localPoints.Length; i++)
            {
                _worldPoints[i] = transform.TransformPoint(localPoints[i]);
            }
        }
        
        private void Update()
        {
            if (_worldPoints.Length == 0)
                return;

            transform.position = Vector3.MoveTowards(transform.position, _worldPoints[_currentPoint], speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _worldPoints[_currentPoint]) > 0.01f) return;
            _currentPoint++;
            if (_currentPoint >= _worldPoints.Length)
                _currentPoint = 0;
        }
        
        // Show gizmos for each point
        private void OnDrawGizmos()
        {
            if (localPoints.Length == 0)
                return;

            Gizmos.color = Color.red;
            foreach (var point in localPoints)
                Gizmos.DrawSphere(transform.TransformPoint(point), 0.1f);
        }
    }

}
