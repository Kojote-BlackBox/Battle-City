using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    public class Utils
    {
        private static System.Random _random = new System.Random();

        public static Vector2 CalculcateRandomDirection2D()
        {
            var possibleDirections = new List<Vector3>
            {
                new Vector3(0.0f, 0.0f),
                new Vector3(0.0f, -1.0f),
                new Vector3(0.0f, 1.0f),
                new Vector3(-1.0f, 0.0f),
                new Vector3(-1.0f, 1.0f),
                new Vector3(-1.0f, -1.0f),
                new Vector3(1.0f, 0.0f),
                new Vector3(1.0f, 1.0f),
                new Vector3(1.0f, -1.0f),
            };

            return possibleDirections[_random.Next(0, possibleDirections.Count)];
        }

        public static float CalculcateRandomDirection1D()
        {
            var possibleDirections = new List<float> {
                0.0f,
                -1.0f,
                1.0f
            };

            return possibleDirections[_random.Next(0, possibleDirections.Count)];
        }

        public static double CalculateRandomWithMinMax(float min, float max)
        {
            return _random.NextDouble() * max + min;
        }

        public static Vector2 CalculateDirectionToTarget(Vector2 origin, Vector2 target)
        {
            var directionToTarget = target - origin;

            return directionToTarget.normalized;
        }

        public static float CalculateRotationToTarget(Vector2 directionToTarget, Vector2 facingDirection)
        {
            var crossProduct = Vector3.Cross(directionToTarget.normalized, facingDirection);

            if (Math.Abs(crossProduct.z) <= 0.225f) return 0;

            return crossProduct.z; // 0 - straight # < 0 - left # > 0 - right
        }

        public static int GetNumberInRange(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}