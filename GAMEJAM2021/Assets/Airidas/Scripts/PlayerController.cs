using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    private Rigidbody2D _rb;

    Vector2 _movement;

    bool _pickupAllowed = false;
    GameObject _pickedUpObject = null;

    [SerializeField] TextMeshPro text;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        if (_pickupAllowed && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Picked up " + _pickedUpObject.name);
            Pickup();
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
            text.text = "Hit E to pick up " + _pickedUpObject.name;
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
    }

    void Pickup()
    {
        if(_pickedUpObject != null)
            Destroy(_pickedUpObject);
    }
}
