using StaticTags;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float speedEnemy = 3;

    private GameObject _planetEnemy;
    private Rigidbody _rbEnemy;
    private Vector3 _groundNormalEnemy;
    private float _gravityEnemy = 100;


    [Header("Add Body")]
    [SerializeField] private int _gapEnemy = 12; // расстояние между телами
    [SerializeField] private float bodySpeedEnemy = 3; // скорость тел
    [SerializeField] private List<Transform> _bodyPartsEnemy;

    private List<Vector3> _positionSnakeEnemy = new List<Vector3>(); // список расположения тел змейки

    void Start()
    {
        if (!GetComponent<Rigidbody>())
        {
            gameObject.AddComponent<Rigidbody>();
        }
        GetComponent<Rigidbody>().isKinematic = false;

        _planetEnemy = GameObject.FindGameObjectWithTag(Tags.Planet);
        _rbEnemy = GetComponent<Rigidbody>();
    }


    void Update()
    {
        GroundCheckEnemy();
        UseGravityEnemy();
    }

    private void FixedUpdate()
    {
        MoveEnemy();
        BodyMoveEnemy();
    }

    private void GroundCheckEnemy()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {
            _groundNormalEnemy = hit.normal;
        }
    }

    private void UseGravityEnemy()
    {
        Vector3 gravDirection = (transform.position - _planetEnemy.transform.position).normalized;
        _rbEnemy.AddForce(gravDirection * -_gravityEnemy);
    }

    private void BodyMoveEnemy()
    {
        _positionSnakeEnemy.Insert(0, transform.position);
        int index = 0;
        foreach (var body in _bodyPartsEnemy)
        {
            Vector3 point = _positionSnakeEnemy[Mathf.Clamp(index * _gapEnemy, 0, _positionSnakeEnemy.Count - 1)];

            Vector3 moveDirection = point - body.transform.position;
            body.transform.position = Vector3.Lerp(body.transform.position, point, bodySpeedEnemy * Time.deltaTime);

            body.transform.LookAt(point);

            index++;
        }
    }

    private void MoveEnemy()
    {
        _rbEnemy.velocity = transform.forward * speedEnemy;
    }
}
