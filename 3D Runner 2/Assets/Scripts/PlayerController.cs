using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Cinemachine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float coinCount;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] public Vector3 playerScale = Vector3.one;
    private Vector3 _direction;
    private Rigidbody _physics;
    private Animator _animator;


    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject finishCollider;
    [SerializeField] private CinemachineVirtualCamera VirtualCam;
    [SerializeField] private CinemachineVirtualCamera FinishCam;

    void Start()
    {
        _physics = GetComponent<Rigidbody>();


        _animator = GetComponent<Animator>();
        _animator.SetBool("Twerk", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Run", true);

    }

    private void Update()
    {

        PlayerMove();
        // Debug.DrawRay();
        //_direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        //transform.localPosition += _direction * speed * Time.deltaTime;
        //float posX = Math.Clamp(transform.localPosition.x, -3.8f, 3.8f);

        //transform.localPosition =
        //    new Vector3(
        //        posX,
        //        transform.localPosition.y,
        //        transform.localPosition.z
        //        );
    }

    //void FixedUpdate()
    //{
    //    PlayerMove();
    //}

    void PlayerMove()
    {
        _direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.localPosition += _direction * speed * Time.deltaTime;
        float posX = Math.Clamp(transform.localPosition.x, -3.8f, 3.8f);

        transform.localPosition =
            new Vector3(
                posX,
                transform.localPosition.y,
                transform.localPosition.z
                );
    }
    //private void PlayerMove()
    //{
    //    _speed *= 1.0002f;
    //    transform.position += _direction * _speed * Time.deltaTime;
    //    _physics.MovePosition(transform.position + _direction * _speed * Time.deltaTime);
    //    transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4, 4), transform.position.y, transform.position.z);
    //}


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            CoinCollect();
            Destroy(collision.gameObject);


        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            FallPlayer();
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            FinishGame();

        }

    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(0);
        if (collision.gameObject.CompareTag("Ground"))
        {
            PlayerJump();
            _animator.SetBool("Jump", false);
        }
    }

    private void PlayerJump()
    {
        Debug.Log(1);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(2);

            if (Physics.Raycast(transform.position, Vector3.down, 5f, layer))
            {
                Debug.Log(3);

                _animator.SetBool("Jump", true);
                _physics.AddForce(Vector3.up * jumpForce);
            }
            if (Physics.Raycast(transform.position, -1 * transform.up, 0.4f, layer))
            {
                _animator.SetBool("Jump", false);
            }

        }
        if (Physics.Raycast(transform.position, -1 * transform.up, 0.4f, layer))
        {
            _animator.SetBool("Jump", false);
        }
    }
    private void FallPlayer()
    {
        Debug.Log("Uduzdunuz");
        _animator.SetBool("Run", false);
        _animator.SetBool("Fall", true);
        StartCoroutine(Died());
    }
    private void FinishGame()
    {
        Debug.Log("Bitdi");
        _animator.SetBool("Run", false);
        _animator.SetBool("Twerk", true);
        speed = 0;
        FinishCam.Priority = 15;
        //Destroy(collision.gameObject);

        //_physics.constraints = RigidbodyConstraints.FreezePositionZ;


    }

    private void CoinCollect()
    {

        coinCount++;
        Debug.Log(coinCount);
    }

    IEnumerator Died()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }


}
