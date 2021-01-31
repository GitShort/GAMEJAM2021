using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int playerState;

    [SerializeField] float moveSpeed = 5f;
    private Rigidbody2D _rb;
    SpriteRenderer _rend;

    Vector2 _movement;

    bool _isNearHairClipper = false;
    bool _pickupAllowed = false;
    bool _isNearDoor = false;
    bool _isNearFridge = false;
    bool _isNearComputer = false;
    bool _isNearDirtyClothes = false;
    bool _isClothesPickedUp = false;
    bool _isNearWashingMachine = false;

    bool _isNearSofa = false;
    bool _isNearTrashCan = false;
    bool _isNearTrash = false;
    bool _isNearNewClothes = false;

    bool _isNearGym = false;

    GameObject _pickedUpObject = null;
    GameObject _openedDoor = null;

    int trashCount = 0;

    [SerializeField] GameObject clothesShop;
    [SerializeField] TextMeshPro text;
    [SerializeField] GameObject dirtyClothes;
    [SerializeField] GameObject trashBag;

    Animator _anim;
    [SerializeField] Animator _smokeAnimation;

    void Start()
    {
        playerState = 1;
        _rb = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        dirtyClothes.SetActive(false);
        trashBag.SetActive(false);
    }

    
    void Update()
    {
        _anim.SetInteger("playerState", playerState);

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

        //FOR DEBUGGING PURPOSES ONLY
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.UpdateDay();
            Debug.Log(GameManager.CurrentDay);
        }
        //

        if (_isNearFridge && Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<AudioManager>().Play("Computer");
            Debug.Log("Fridge window opened!");
        }

        if ((_isNearComputer && Input.GetKeyDown(KeyCode.E)) && (GameManager.CurrentDay == 1 || GameManager.CurrentDay == 6) && !GameManager.ComputerActionDone)
        {
            FindObjectOfType<AudioManager>().Play("Computer");
            clothesShop.SetActive(true);
            Debug.Log("Computer window opened!");
        }

        if ((_isNearHairClipper && Input.GetKeyDown(KeyCode.E)) && GameManager.CurrentDay == 0)
        {
            FindObjectOfType<AudioManager>().Play("Shaving");
            _smokeAnimation.Play("Smoke", -1, 0);
            playerState = 2;
            Debug.Log("SHAVED");
            Destroy(_pickedUpObject);

        }
        else if ((_isNearDirtyClothes && Input.GetKeyDown(KeyCode.E)) && GameManager.CurrentDay == 2 && !_isClothesPickedUp)
        {
            FindObjectOfType<AudioManager>().Play("Drop");
            Debug.Log("Picked up dirty clothes");
            _isClothesPickedUp = true;
            dirtyClothes.SetActive(true);
            Destroy(_pickedUpObject);
        }
        else if (_isNearWashingMachine && Input.GetKeyDown(KeyCode.E) && GameManager.CurrentDay == 2 && _isClothesPickedUp)
        {
            FindObjectOfType<AudioManager>().Play("Drop");
            _isClothesPickedUp = false;
            dirtyClothes.SetActive(false);
            _pickedUpObject.GetComponent<ObjectSwitchManager>().ActionDone();
            GameManager.LaundryDone = true;
            Debug.Log("Laundry done");
        }
        else if (_isNearSofa && Input.GetKeyDown(KeyCode.E) && GameManager.CurrentDay == 3 && !GameManager.SofaCleaned)
        {
            GameManager.SofaCleaned = true;
            FindObjectOfType<AudioManager>().Play("Shaving");
            _pickedUpObject.GetComponent<ObjectSwitchManager>().ActionDone();
            if (!trashBag.activeInHierarchy)
                trashBag.SetActive(true);
            trashCount++;
            Debug.Log("Sofa cleaned");
        }
        else if (_isNearTrash && Input.GetKeyDown(KeyCode.E) && GameManager.CurrentDay == 3)
        {
            FindObjectOfType<AudioManager>().Play("Drop");
            trashCount++;
            if (!trashBag.activeInHierarchy)
                trashBag.SetActive(true);
            Pickup();
            Debug.Log("Trash picked up");
        }
        else if (_isNearTrashCan && Input.GetKeyDown(KeyCode.E) && GameManager.CurrentDay == 3 && trashCount >= 3)
        {
            FindObjectOfType<AudioManager>().Play("Drop");
            GameManager.TrashThrownOut = true;
            trashBag.SetActive(false);
            Debug.Log("Trash thrown out!");
        }
        else if (_isNearGym && Input.GetKeyDown(KeyCode.E) && (GameManager.CurrentDay == 4 || GameManager.CurrentDay == 5 || GameManager.CurrentDay == 6) && !GameManager.WorkedOut)
        {
            FindObjectOfType<AudioManager>().Play("Shaving");
            Debug.Log("Working out");
            _smokeAnimation.Play("Smoke", -1, 0);
            // Stop player movement while animation is playing
            GameManager.WorkedOut = true;
        }
        else if (_isNearNewClothes && Input.GetKeyDown(KeyCode.E) && GameManager.CurrentDay == 7)
        {
            FindObjectOfType<AudioManager>().Play("Shaving");
            moveSpeed = 5f;
            _smokeAnimation.Play("Smoke", -1, 0);
            playerState = 3;
            Debug.Log("FINAL");
            Destroy(_pickedUpObject);
        }



        //else if (_pickupAllowed && Input.GetKeyDown(KeyCode.E))
        //{
        //    Debug.Log("Picked up " + _pickedUpObject.name);
        //    Pickup();
        //}



        if (_isNearDoor && Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<AudioManager>().Play("Door");
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
        if (collision.gameObject.tag.Equals("HairClipper"))
        {
            _isNearHairClipper = true;
            _pickedUpObject = collision.gameObject;
            text.text = "Press E to pick up " + _pickedUpObject.name;
        }
        else if (collision.gameObject.tag.Equals("Pickupable"))
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
        else if (collision.gameObject.tag.Equals("Fridge"))
        {
            _isNearFridge = true;
            if(!GameManager.AteFood)
                text.text = "Press E to open the fridge";
        }
        else if (collision.gameObject.tag.Equals("PC"))
        {
            _isNearComputer = true;
            if(!GameManager.ComputerActionDone && (GameManager.CurrentDay == 1 || GameManager.CurrentDay == 6))
                text.text = "Press E to open up the computer screen";
        }
        else if (collision.gameObject.tag.Equals("DirtyClothes"))
        {
            _isNearDirtyClothes = true;
            _pickedUpObject = collision.gameObject;
            if(GameManager.CurrentDay == 2)
                text.text = "Press E to pick up " + _pickedUpObject.name;
        }
        else if (collision.gameObject.tag.Equals("WashingMachine"))
        {
            _isNearWashingMachine = true;
            _pickedUpObject = collision.gameObject;
            if (_isClothesPickedUp)
                text.text = "Press E to place clothes into washing machine";
        }
        else if (collision.gameObject.tag.Equals("Sofa"))
        {
            _isNearSofa = true;
            _pickedUpObject = collision.gameObject;
            if (GameManager.CurrentDay == 3 && !GameManager.SofaCleaned)
                text.text = "Press E to clean the couch";
        }
        else if (collision.gameObject.tag.Equals("Trash"))
        {
            _isNearTrash = true;
            _pickedUpObject = collision.gameObject;
            if(GameManager.CurrentDay == 3)
                text.text = "Press E to pick up " + _pickedUpObject.name;
        }
        else if (collision.gameObject.tag.Equals("TrashCan"))
        {
            _isNearTrashCan = true;
            if(trashCount >=3)
                text.text = "Press E to throw out the trash ";
        }
        else if (collision.gameObject.tag.Equals("GymSet"))
        {
            _isNearGym = true;
            if(!GameManager.WorkedOut)
                text.text = "Press E to exercise";
        }
        else if (collision.gameObject.tag.Equals("NewClothes"))
        {
            _isNearNewClothes = true;
            _pickedUpObject = collision.gameObject;
            if(GameManager.CurrentDay == 7)
                text.text = "Press E to wear new clothes";
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("HairClipper"))
        {
            _isNearHairClipper = false;
            _pickedUpObject = null;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("Pickupable"))
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
        else if (collision.gameObject.tag.Equals("Fridge"))
        {
            _isNearFridge = false;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("PC"))
        {
            _isNearComputer = false;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("DirtyClothes"))
        {
            _isNearDirtyClothes = false;
            _pickedUpObject = null;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("WashingMachine"))
        {
            _isNearWashingMachine = false;
            _pickedUpObject = null;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("Sofa"))
        {
            _isNearSofa = false;
            _pickedUpObject = null;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("Trash"))
        {
            _isNearTrash = false;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("TrashCan"))
        {
            _isNearTrashCan = false;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("GymSet"))
        {
            _isNearGym = false;
            text.text = null;
        }
        else if (collision.gameObject.tag.Equals("NewClothes"))
        {
            _isNearNewClothes = false;
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
