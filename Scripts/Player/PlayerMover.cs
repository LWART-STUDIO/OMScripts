using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Joystick _joystick;
    private Vector3 _movement;
    [Range(0, 10)]
    [SerializeField] private float _rotationSmooth;
    private float _yValue;
    public float plusYValue;
    private PlayerInventory _inventory;
    private Rigidbody _playerRigbody;

    public float MovementVectorMagnitude
    {
        get
        {
            return _movement.magnitude;
        }
    }
    public Vector3 MovementVector
    {
        get { return _movement; }
    }
    private void Start()
    {
        _playerRigbody = GetComponent<Rigidbody>();
        _inventory= GetComponent<PlayerInventory>();
    }
    void FixedUpdate()
    {
        if (gameObject.transform.position.y < -0.78)
        {
            gameObject.transform.position = new Vector3(-0.52f, 0.72f, 4.25f);
        }
        
        _speed = _inventory.MoveSpeed;
        _movement = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        //transform.position 
            _playerRigbody.velocity=_movement*50*_speed*Time.fixedDeltaTime;
                // transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (Mathf.Abs(_joystick.Vertical) > 0f || Mathf.Abs(_joystick.Horizontal) > 0f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, (Mathf.Atan2(_movement.normalized.z, -_movement.normalized.x) * 180 / Mathf.PI) - 90, 0), _rotationSmooth * Time.fixedDeltaTime);
        }
        //Debug.Log(_playerRigbody.velocity);
        // Physics.SyncTransforms();
    }
}
