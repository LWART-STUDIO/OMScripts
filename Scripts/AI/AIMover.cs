using UnityEngine;
using UnityEngine.AI;

public class AIMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    private NavMeshAgent _navMeshAgent;
    [SerializeField]private Vector3 _pointToMove;
    [SerializeField] private bool _isMoving;
    public bool IsMoving => _isMoving;

    private void Awake()
    {
        _navMeshAgent=GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if(_navMeshAgent.remainingDistance>=0.01)
        {
            _isMoving=true;
        }
        else
        {
            _isMoving=false;
        }
        _navMeshAgent.speed = _speed;
        _navMeshAgent.destination=_pointToMove;
    }
    public void SetPoint(Vector3 position)
    {
        _pointToMove=position;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    
}
