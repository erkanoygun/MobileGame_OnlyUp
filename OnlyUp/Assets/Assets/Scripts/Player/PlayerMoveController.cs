using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMoveController : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float jumpForce = 2f;
    public float rotateSpeed = 50f;
    public float upJumpForce = 5f;

    private bool isTouchingLeft = false;
    private bool isTouchingRight = false;
    private float touchStartTime = 0f;
    private bool _isWalk;
    private bool _isDesh = false;
    private bool _isGround = true;
    private int _feetGroundTouch = 0;
    private bool _isTouchEnd;

    [SerializeField] private Image _forceBarImageFill;
    private GameManager _gameManagerScr;


    void Start()
    {
        _gameManagerScr = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.touchCount > 0 && !_gameManagerScr._isGamePause)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartTime = Time.time;
                isTouchingLeft = touch.position.x < Screen.width / 2;
                isTouchingRight = !isTouchingLeft;
                _isTouchEnd = false;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _forceBarImageFill.fillAmount = 0f;
                _isTouchEnd = true;

                float roundedResult = Mathf.Round((Time.time - touchStartTime) * 100) / 100;

                if (roundedResult <= 0.2f)
                    _isWalk = true;
                else
                    _isWalk = false;

                if (_isGround)
                {
                    if (isTouchingLeft)
                    {
                        JumpAndRotate(-roundedResult, roundedResult, _isWalk);
                    }
                    else if (isTouchingRight)
                    {
                        JumpAndRotate(roundedResult, roundedResult, _isWalk);
                    }
                }
                else
                {
                    if (!_isDesh)
                    {
                        if (isTouchingLeft)
                        {
                            _rb.AddForce(new Vector2(-roundedResult, 0) * 6, ForceMode2D.Impulse);
                            _rb.AddTorque(-roundedResult * 7);
                        }
                        else if (isTouchingRight)
                        {
                            _rb.AddForce(new Vector2(roundedResult, 0) * 6, ForceMode2D.Impulse);
                            _rb.AddTorque(roundedResult * 7);
                        }
                        _isDesh = true;
                    }

                }
            }
            // Dokunma sona erdiğinde Force Bar'ı sıfırlıyoruz.
            if (_isTouchEnd)
                ChangeForceBar(0f);
            else
                ChangeForceBar(Mathf.Round((Time.time - touchStartTime) * 100) / 100);
        }
    }

    void JumpAndRotate(float x, float y, bool isDesh)
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
        // ekrana tek bir kısa tıklama yapıldıysa sadece bir adım zıplatmadan ilerletiyoruz.
        if (_isWalk)
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
            _isDesh = false;
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
            if (_feetGroundTouch < 2)
                _feetGroundTouch++;
        }

        if (_feetGroundTouch == 2)
        {
            if ((_rb.constraints & RigidbodyConstraints2D.FreezePositionX) == 0)
            {
                _rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (_feetGroundTouch > 0)
                _feetGroundTouch--;
        }
    }

    private void ChangeForceBar(float time)
    {
        if (time <= 0.70f)
        {
            _forceBarImageFill.fillAmount = (time / 0.70f);
        }
    }
}

