using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;

    [Range(0f, 1f)]
    [SerializeField] private float _slowDownMultiplier;

    [SerializeField] private ResourceStorage _inventory = null;

    private Rigidbody _rb;
    private Vector2 _moveDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        _moveDirection = new Vector3(vertical, -horizontal);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            _inventory.Add(ResourceType.First);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _inventory.Add(ResourceType.Second);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            _inventory.Add(ResourceType.Third);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            _inventory.Remove(ResourceType.First);
        }
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector2.zero)
        {
            Move(_moveDirection);
        } else
        {
            SlowDown(_slowDownMultiplier);
        }
        
    }

    private void Move(Vector2 direction)
    {
        var velocity = _rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x + direction.x * _acceleration * Time.deltaTime, -_maxSpeed, _maxSpeed);
        velocity.z = Mathf.Clamp(velocity.z + direction.y * _acceleration * Time.deltaTime, -_maxSpeed, _maxSpeed);
        _rb.velocity = velocity;
    }

    private void SlowDown(float speedMultiplier)
    {
        var velocity = _rb.velocity;
        velocity.x = Mathf.Lerp(velocity.x, 0, speedMultiplier);
        velocity.z = Mathf.Lerp(velocity.z, 0, speedMultiplier);
        _rb.velocity = velocity;
    }
}
