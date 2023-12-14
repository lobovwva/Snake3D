using StaticTags;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement values")]
        [SerializeField] private float speed = 4;

        private FixedJoystick _joystick;
        private Rigidbody _rb;
        private GameObject _planet;
        private float _rotationSpeed = 150;
        private float _gravity = 100;
        private Vector3 _groundNormal;

        [Header("Add Body")]
        [SerializeField] private GameObject bodyPrefab; // префаб для создания нового тела змейки
        [SerializeField] private int _gap = 12; // расстояние между телами
        [SerializeField] private float bodySpeed = 4; // скорость тел

        private List<GameObject> _bodyParts = new List<GameObject>(); // список тел змейки
        private List<Vector3> _positionSnake = new List<Vector3>(); // список расположения тел змейки

        [Header("UI")]
        [SerializeField] private GameObject panelGO;
        [SerializeField] private GameObject panelGame;

        [SerializeField] private ScoreScript scoreScript;

        private void Start()
        {
            GrowSnake(); 
            _planet = GameObject.FindGameObjectWithTag(Tags.Planet);
            _joystick = GameObject.FindGameObjectWithTag(Tags.Joystick).GetComponent<FixedJoystick>();
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            panelGO.SetActive(false);
            panelGame.SetActive(true);
        }

        private void Update()
        {
            GroundCheck();
            UseGravity();
            Rotate();
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.forward * speed;
            BodyMove();
        }

        private void GroundCheck()
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
            {
                _groundNormal = hit.normal;
            }
        }

        private void Rotate()
        {
            Quaternion toRotation = Quaternion.FromToRotation(transform.up, _groundNormal) * transform.rotation;
            transform.rotation = toRotation;
            float rotationInput = _joystick.Horizontal;
            transform.Rotate(0, rotationInput * _rotationSpeed * Time.deltaTime, 0);
        }

        private void UseGravity()
        {
            Vector3 gravDirection = (transform.position - _planet.transform.position).normalized;
            _rb.AddForce(gravDirection * -_gravity);
        }

        private void GrowSnake() 
        {
            if (_bodyParts.Count == 0)
            {
                // для первого тела используем текущую позицию
                GameObject body = Instantiate(bodyPrefab, transform.position - transform.forward * _gap, Quaternion.identity);
                _bodyParts.Add(body);
                _positionSnake.Add(body.transform.position);
            }
            else
            {
                // для последующих тел рассчитываем новую позицию с учетом расстояния и направления движения
                GameObject lastBody = _bodyParts[_bodyParts.Count - 1];
                Vector3 newPosition = lastBody.transform.position - lastBody.transform.forward * _gap;

                GameObject body = Instantiate(bodyPrefab, newPosition, Quaternion.identity);
                _bodyParts.Add(body);
                _positionSnake.Add(body.transform.position);
            }
        }

        private void BodyMove()
        {
            _positionSnake.Insert(0, transform.position);
            int index = 0;
            foreach (var body in _bodyParts)
            {
                Vector3 point = _positionSnake[Mathf.Clamp(index * _gap, 0, _positionSnake.Count - 1)];

                Vector3 moveDirection = point - body.transform.position;
                body.transform.position = Vector3.Lerp(body.transform.position, point, bodySpeed * Time.deltaTime);

                body.transform.LookAt(point);

                index++;
            }
        }

        private void OnTriggerEnter(Collider other) 
        {
            if (other.CompareTag("Apple"))
            {
                GrowSnake();
                scoreScript.IncreaseScore(); // увеличиваем счет
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("BodyEnemy") || other.gameObject.CompareTag("Enemy"))
            {
                Time.timeScale = 0;
                panelGO.SetActive(true);
                panelGame.SetActive(false);
            }
        }
    }
}
