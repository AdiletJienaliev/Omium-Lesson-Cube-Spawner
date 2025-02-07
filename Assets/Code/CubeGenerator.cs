using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Cube
{
    public class CubeGenerator : MonoBehaviour
    {
        [Header("Links")] 
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private Transform parent;
        [SerializeField] private CubeMover cubeMover;
        
        [FormerlySerializedAs("cubeNumber")]
        [Header("Settings")]
        [SerializeField] private int cubeNumberMax;
        [SerializeField] private int cubeNumberMin;
        [SerializeField] private float yPosCubes;
        [SerializeField] private Vector3 minPos;
        [SerializeField] private Vector3 maxPos;
        
        private List<CubeController> createdItems = new List<CubeController>();
        
        public static UnityAction<bool> ActivateCubes = new UnityAction<bool>((bool v) => { });

        private void Awake()
        {
            CreateCubes();
            ActivateCubes += ActivateCubeItems;
        }

        private void ActivateCubeItems(bool v)
        {
            for (int i = 0; i < createdItems.Count; i++)
                if (createdItems[i] is not null)
                    createdItems[i].gameObject.SetActive(v);
        }
        private void CreateCubes()
        {
            for (int i = 0; i < Random.Range(cubeNumberMin, cubeNumberMax); i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(minPos.x, maxPos.x),
                    yPosCubes,
                    Random.Range(minPos.z, maxPos.z)
                );
                
               CubeController cube = CreateCubeItem(randomPosition, i);
               cube.gameObject.SetActive(false);
               AddItemCube(cube);
            }
        }

        private CubeController CreateCubeItem(Vector3 position, int index)
        {
            CubeController cubeController = Instantiate(cubePrefab, parent).GetComponent<CubeController>();
            
            cubeController.SetColor(new Color(Random.Range(0,1f), Random.Range(0,1f),Random.Range(0,1f)));
            cubeController.transform.position = position;
            cubeController.SetName("Cube_" + index);    
            cubeController.SetTextIndex(index + 1);
            return cubeController;
        }
        
        private void AddItemCube(CubeController cubeController)
        {
            createdItems.Add(cubeController);
            cubeMover.AddItemCube(cubeController);
        }
    }
}