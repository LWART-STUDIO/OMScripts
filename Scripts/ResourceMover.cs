using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ResourceMover : MonoBehaviour
{
    [SerializeField] private Transform _pointToMove;
    [SerializeField] private bool _moveToPlayer;
    [SerializeField] private float _defaultMoveSpeed;
    private float _moveTime;
    [SerializeField] private Transform _playerBackPack;
    public string ID;
    [SerializeField]
    private AnimationCurve curve;

    [SerializeField] private bool _remove = false;

    [SerializeField] private bool _isStarted;
    private float _time=1;
    private Vector3 _start;
    private Vector3 _end;
    private bool _useDG;
    private float _currentTime;
    private void Start()
    {
        _moveTime = _defaultMoveSpeed;
    }
    public void MoveResource(Transform pointToMove, Transform parentObject, bool remove = false,bool useDG =false)
    {
        _useDG = useDG;
        transform.SetParent(parentObject);
        if (!_useDG)
        {

            if (_pointToMove != pointToMove)
            {
                _moveTime = _defaultMoveSpeed;
                _pointToMove = pointToMove;
                _remove = remove;
                _start = transform.localPosition;
                _end = _pointToMove.localPosition;
                Tween newRotation= transform.DOLocalRotateQuaternion(_pointToMove.localRotation, _moveTime).SetEase(Ease.Linear); 
                _currentTime = _time;
                //StartCoroutine(MoveWithoutDg());
                _isStarted = true;
            }
                
                
            
        }
        else
        {
            _moveTime = _defaultMoveSpeed;
            _pointToMove = pointToMove;
            _remove = remove;
            _start = transform.localPosition;
            _end = _pointToMove.localPosition;
            gameObject.transform.SetParent(parentObject);
            transform.DOKill();
            Tween newRotation= transform.DOLocalRotateQuaternion(_pointToMove.localRotation, _moveTime).SetEase(Ease.Linear);
            Tween newPosition= transform.DOLocalMove(_pointToMove.localPosition, _moveTime).SetEase(Ease.Linear);
            
            if (remove)
            {
                newPosition.OnKill(DestroyObject);
    
            }
            
        }
        
        
    }

    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    private void Update()
    {
        if (!_useDG)
        {
            if (!_isStarted) return;
            _currentTime -= Time.deltaTime * _defaultMoveSpeed*15;
            float t = _currentTime /_time ;
            Vector3 pos = Vector3.Lerp(_end, _start,t);
            pos.y += curve.Evaluate(t);
            transform.localPosition = pos;
            if (_currentTime <= 0)
            {
                _isStarted = false;
                if (_remove)
                {
                    Destroy(this.gameObject);
                }
            }
        }
        
    }

    /*private IEnumerator MoveWithoutDg()
    {
        
      
    }*/
}
