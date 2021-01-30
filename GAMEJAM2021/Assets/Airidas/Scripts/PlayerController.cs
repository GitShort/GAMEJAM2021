using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int playerState = 1;

    [SerializeField] float moveSpeed = 5f;
    private Rigidbody2D _rb;
    SpriteRenderer _rend;

    Vector2 _movement;

    bool _pickupAllowed = false;
    bool _isNearDoor = false;
    GameObject _pickedUpObject = null;
    GameObject _openedDoor = null;

    [SerializeField] TextMeshPro text;

    Animator _anim;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        _anim.SetInteger("PlayerState", playerState);

        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");
        if (_movement.x != 0 || _movement.y != 0)
        {
            _anim.SetBool("isRunning", true);
        }
        else
            _anim.SetBool("isRunning", false);

        if (_movement.x > 0 && !_rend.flipX)
            _rend.flipX = true;
        else if (_movement.x < 0 && _rend.flipX)
            _rend.flipX = false;

        if (_pickupAllowed && Input.GetKeyDown(KeyCode.E) && _pickedUpObject.name == "Hair clipper")
        {
            Debug.Log("SHAVED");
            Destroy(_pickedUpObject);
            playerState = 2;
        }
        else if (_pickupAllowed && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Picked up " + _pickedUpObject.name);
            Pickup();
        }



        if (_isNearDoor && Input.GetKeyDown(KeyCode.E))
        {
            text.text = null;
            if (!_openedDoor.GetComponent<DoorController>().IsOpened)
            {
                _openedDoor.GetComponent<DoorController>().OpenDoor();
                _openedDoor.GetComponent<DoorController>().TurnOnLight();
            }
            else
            {
                _openedDoor.GetComponent<DoorController>().CloseDoor();
                _openedDoor.GetComponent<DoorController>().TurnOffLight();
            }
        }
        
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Pickupable"))
        {
            _pickupAllowed = true;
            _pickedUpObject = collision.gameObject;
            text.text = "Press E to pick up " + _pickedUpObject.name;
        }
        else if (collision.gameObject.tag.Equals("Door"))
        {
            _isNearDoor = true;
            _openedDoor = collision.gameObject;
            if (!_openedDoor.GetComponent<DoorController>().IsOpened)
                text.text = "Press E to open the door";
            else
                text.text = "Press E to close the door";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Pickupable"))
        {
            _pickupAllowed = false;
            _pickedUpObject = null;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("Door"))
        {
            _isNearDoor = false;
            _openedDoor = null;
            text.text = null;
        }
    }

    void Pickup()
    {
        if(_pickedUpObject != null)
            Destroy(_pickedUpObject);
    }
}
