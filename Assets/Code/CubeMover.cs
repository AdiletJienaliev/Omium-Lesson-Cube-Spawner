using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Cube
{
    public class CubeMover : MonoBehaviour
    {
        private List<CubeController> cubeControllers = new List<CubeController>();
        
        static public UnityAction<bool> startMoving = new UnityAction<bool>((bool v) => {});
        static public UnityAction<bool> startHunting = new UnityAction<bool>((bool v) => {});
        static public UnityAction startShoot = new UnityAction(() => {});

        [SerializeField] private WeaponAIController aiWeapon;

        private CubeController hunter = null;
        private void Awake()
        {
            startMoving += MoveActivate;
            startHunting += HuntActivate;
            startShoot += ActivateWeapon;
        }

        private void ActivateWeapon()
        {
            aiWeapon.gameObject.SetActive(true);
            UpdateTargetForShoot();
        }

        public void UpdateTargetForShoot()
        {
            aiWeapon.SetTarget(GetTarget());
        }
        public void UpdateTargetForHunt()
        {
            if (hunter != null) 
            {
                CubeController target = GetTarget();
                hunter.SetTargetForHunt(target);
            }
        }
        private void MoveActivate(bool condition)
        {
            for (int i = 0; i < cubeControllers.Count; i++)
                if (cubeControllers[i] != null)
                {
                    if(condition)
                        cubeControllers[i].StartMove();
                    else 
                        cubeControllers[i].StopMove();
                }
        }

        private void HuntActivate(bool condition)
        {
            if (cubeControllers.Count < 2) return;
            int hunterIndex = Random.Range(0, cubeControllers.Count);
            
            CubeController hunter = cubeControllers[hunterIndex];
            this.hunter = hunter;

            RemoveIndex(hunterIndex);
            hunter.StartHunt();
            UpdateTargetForHunt();
        }

        public void AddItemCube(CubeController cubeController)
        {
            cubeController.SetMover(this);
            cubeController.SetListIndex(cubeControllers.Count);
            cubeControllers.Add(cubeController);
        }

        public void RemoveIndex(int index)
        {
            if (index >= 0 && index < cubeControllers.Count)
            {
                cubeControllers.RemoveAt(index);
                UpdateListIndexes();
                CubeController newTarget = GetTarget();
                if(newTarget != null)
                    aiWeapon.SetTarget(newTarget);
            }
        }

        private void UpdateListIndexes()
        {
            for (int i = 0; i < cubeControllers.Count; i++)
            {
                if(cubeControllers[i] != null)
                    cubeControllers[i].SetListIndex(i);
            }
        }

        public CubeController GetTarget()
        {
            if (cubeControllers.Count <= 0) return null;
            int randomIndex = Random.Range(0, cubeControllers.Count);
            var cubeController = cubeControllers[randomIndex];
            return cubeController;
        }

    }
}