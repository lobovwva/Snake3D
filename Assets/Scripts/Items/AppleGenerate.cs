using System;
using System.Collections;
using UnityEngine;

namespace Items
{
    public class AppleGenerate : MonoBehaviour
    {
        [SerializeField] private GameObject objectToSpawn; 
        [SerializeField] private int numberOfObjects; 
        [SerializeField] private float radius; 
        [SerializeField] private float spawnInterval; 

        void Start()
        {
            StartCoroutine(SpawnObjectsOnSphereWithDelay());
        }

        IEnumerator SpawnObjectsOnSphereWithDelay()
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                float u = UnityEngine.Random.Range(0f, 1f);
                float v = UnityEngine.Random.Range(0f, 1f);
                float theta = 2 * Mathf.PI * u;
                float phi = Mathf.Acos(2 * v - 1);
                float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                float z = radius * Mathf.Cos(phi);

                Vector3 spawnPosition = new Vector3(x, y, z);

                // спавним объект с заданной позицией и поворотом
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

                yield return new WaitForSeconds(spawnInterval); // ждем указанный интервал времени перед спавном следующего объекта
            }
        }
    }
}
