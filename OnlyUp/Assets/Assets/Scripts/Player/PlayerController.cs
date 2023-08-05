using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float jumpForce = 2f;
    public float rotateSpeed = 50f;
    public float upJumpForce = 5f;

    private bool isTouchingLeft = false;
    private bool isTouchingRight = false;
    private float touchStartTime = 0f;
    private float touchEndTime = 0f;
    private bool _isGround = true;
    // Havadayken zıplayıp zıplamadığını tutuyor.
    private bool _isDoubleJump = false;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            JumpAndRotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            JumpAndRotate(1);
        }*/

        // Dokunma olaylarını kontrol et
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartTime = Time.time;
                isTouchingLeft = touch.position.x < Screen.width / 2;
                isTouchingRight = !isTouchingLeft;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {

                if (!_isDoubleJump)
                {
                    if (!_isGround)
                        _isDoubleJump = true;

                    touchEndTime = Time.time;

                    if (isTouchingLeft)
                    {
                        float roundedResult = Mathf.Round((touchEndTime - touchStartTime) * 100) / 100;
                        JumpAndRotate(-roundedResult, roundedResult);
                    }
                    else if (isTouchingRight)
                    {
                        float roundedResult = Mathf.Round((touchEndTime - touchStartTime) * 100) / 100;
                        JumpAndRotate(roundedResult, roundedResult);
                    }
                }
            }
        }
    }

    void JumpAndRotate(float x, float y)
    {
        _rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

        // Touch süresi 0.70f den büyükse süreyi 0.70f ayarlıyoruz
        if (Mathf.Abs(x) > 0.70f)
        {
            // X in hem negatif hem pozitif gelme durumu var.
            if (x < 0f)
                x = -0.70f;
            else
                x = 0.70f;
            y = 0.70f;
        }
        // ekrana tek bir kısa tıklama yapıldıysa sadece bir adım zıplatmadan ilerletiyoruz
        if (Mathf.Abs(x) <= 0.20f)
        {
            _rb.AddForce(new Vector2(x / Mathf.Abs(x), 0) * 2, ForceMode2D.Impulse);
            _rb.AddTorque(-x / Mathf.Abs(x) * 25);
        }
        else
        {
            _rb.AddForce(new Vector2(x, y * upJumpForce) * jumpForce, ForceMode2D.Impulse);
            _rb.AddTorque(-x * rotateSpeed);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
            _isDoubleJump = false;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if ((_rb.constraints & RigidbodyConstraints2D.FreezePositionX) == 0)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }
}

