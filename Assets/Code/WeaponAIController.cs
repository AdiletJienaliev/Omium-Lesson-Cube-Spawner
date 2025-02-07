using System;
using TMPro;
using UnityEngine;

namespace Cube
{
    public class WeaponAIController : MonoBehaviour
    {
        [SerializeField]private Transform target; 
        [SerializeField]private GameObject bulletPrefab; 
        [SerializeField]private Transform pivotYOrbit; 
        [SerializeField]private Transform pivotZOrbit; 
        [SerializeField]private float fireRate = 1.0f; 
        [SerializeField]private float fireForce = 500f;
        [SerializeField]private float bulletDestroyTime = 5.0f;
        [SerializeField]private Transform bulletPivot;
        [SerializeField]private CubeMover mover;
        [SerializeField]private TextMeshProUGUI textNum;

        private float lastFireTime = 0f;
        private bool HuntingStart= false;

        private void Awake()
        {
            textNum.text = "";
            CubeMover.startHunting += (bool v) => HuntingStart = v;
        }
        void Update()
        {
            if (target != null && HuntingStart == false)
            {
                AimAtTarget();
                TryFire();
                PositionMe();
            }
        }
        private void PositionMe()
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime * 5);
        }

        void AimAtTarget()
        {
            Vector3 directionToTarget = target.position - pivotYOrbit.position;

            Quaternion targetRotationY = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            pivotYOrbit.rotation = Quaternion.Lerp(pivotYOrbit.rotation, targetRotationY, Time.deltaTime * 5f);

            Quaternion targetRotationZ = Quaternion.LookRotation(directionToTarget);
            //pivotZOrbit.rotation = Quaternion.Lerp(pivotZOrbit.rotation, targetRotationZ, Time.deltaTime * 5f); 
            pivotZOrbit.LookAt(target);
        }

        void TryFire()
        {
            if (Time.time >= lastFireTime + 1f / fireRate)
            {
                Fire();
                lastFireTime = Time.time;
            }
        }

        void Fire()
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletPivot.position, bulletPivot.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.AddForce(bulletPivot.forward * fireForce, ForceMode.Impulse);
            }
            Destroy(bullet, bulletDestroyTime);
        }

        public void SetTarget(CubeController target)
        {
            this.target = target.transform;
            SetIndexText(target.GetListIndex());
        }

        private void SetIndexText(int index)
        {
            index += 1;
            textNum.text = index.ToString();
        }
    }
}