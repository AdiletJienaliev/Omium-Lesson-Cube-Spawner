using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Cube
{
    public class CubeController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI numText;
        [SerializeField] private MeshRenderer renderer;
        [SerializeField] private Rigidbody rigidbody;
        
        [Header("Settings")]
        [SerializeField] private float maxRandomDirectionChangeTime= 1f;
        [SerializeField] private float minRandomDirectionChangeTime = 4f;
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float scaleSpeed = 1;
        [SerializeField] private Vector3 huntScale;
        [SerializeField] private int listIndex = -404;
        
        private CubeMover mover;
        private CubeController targetCube;
        
                
        public bool isMoving = false;
        public bool isHunting = false;
        public bool isDead = false;

        #region Public
        public void SetTextIndex (int index) => numText.text = index.ToString();
        public void SetName(string textName) => gameObject.name = textName;
        public void SetColor(Color color) => renderer.material.color = color;
        
        public void SetMover(CubeMover mover) => this.mover = mover;
        public void SetListIndex(int index) => listIndex = index;
        public int GetListIndex() => listIndex;

        public void StartMove()
        {
            UpdateRandomDirection();
            isMoving = true;
            isHunting = false;
        } 
        public void StopMove()  =>  isMoving = false;
        public void StartHunt()
        {
            isMoving = false;
            isHunting = true;
            SetColor(Color.red * 1.5f);
        }

        public void SetTargetForHunt(CubeController target)
        {
            targetCube = target;        
        }
        public void StopHunt() => isHunting = false;
        #endregion

        private void Awake()
        {
            renderer.material = Instantiate(renderer.material);
        }
        private void Update()
        {
            HandleMove();
            HandleHuntScale();
            HandleHunt();
        }

        private Vector3 direction = Vector3.zero;
        private float RandomTime => UnityEngine.Random.Range(minRandomDirectionChangeTime, maxRandomDirectionChangeTime);
        private Vector3 RandomDirection => new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        private float time = 0;
        
        private void HandleMove()
        {
            if (isMoving)
            {
                Vector3 newPosition = rigidbody.position + direction * Time.deltaTime * moveSpeed;
                rigidbody.MovePosition(newPosition);
        
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, Time.deltaTime * 5f);
                }

                time -= Time.deltaTime;
                if (time <= 0)
                {
                    UpdateRandomDirection();
                }
            }
        }

        private void HandleHuntScale()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, isHunting ? huntScale : Vector3.one, scaleSpeed * Time.deltaTime);
        }

        private void HandleHunt()
        {
            if(targetCube == null) return;
            
            transform.LookAt(targetCube.transform.position);
            Vector3 newPosition = rigidbody.position + (transform.forward * moveSpeed * Time.deltaTime);
            rigidbody.MovePosition(newPosition);
        }

        private void UpdateRandomDirection()
        {
            direction = RandomDirection;
            time = RandomTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(isHunting == true && isDead == true) return;
            
            if ((collision.gameObject.TryGetComponent<CubeController>(out CubeController cubeController) 
                 && cubeController.isHunting == true))
            {
                mover.RemoveIndex(GetListIndex());
                mover.UpdateTargetForHunt();
                Destroy(gameObject);
                isDead = true;
            }
            else if (collision.gameObject.tag == "Bullet")
            {
                mover.UpdateTargetForShoot();
                StopMove();
            }
            else if (collision.gameObject.tag == "Wall")
            {
                Vector3 backDirection = collision.gameObject.transform.forward;
                direction = backDirection;
                time = 3;
            }
        }
    }
}