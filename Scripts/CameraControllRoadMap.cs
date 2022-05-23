using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraControllRoadMap : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Vector3 _touchStart;
    [SerializeField] private float _corrector;
    [SerializeField] private float Timer;
    [SerializeField] private Transform _ship;
    [SerializeField] private Animator _cameraAnimtor;
    private bool _corutineRun;
    private void Update()
    {
        
        Timer += Time.unscaledDeltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            if (_corutineRun)
            {
                StopAllCoroutines();
                _corutineRun = false;
                gameObject.transform.position = _ship.transform.position;
            }
            
            _cameraAnimtor.Play("ControllCamera");
            _touchStart = _camera.ScreenToViewportPoint((Input.mousePosition));
            Debug.Log(_camera.ScreenToViewportPoint((Input.mousePosition)));
        }

        if (Input.GetMouseButton(0))
        {
            Timer = 0;
            Vector3 direction = _touchStart - _camera.ScreenToViewportPoint(Input.mousePosition);
            Vector3 newDirection = new Vector3(direction.x*_corrector, 0, direction.y*_corrector);
            gameObject.GetComponent<Rigidbody>().velocity=newDirection;

        }

        if (Input.GetMouseButtonUp(0))
        {
            gameObject.GetComponent<Rigidbody>().velocity=Vector3.zero;
        }

        if (Timer > 3&& gameObject.transform.position != _ship.transform.position)
        {
            _cameraAnimtor.Play("MainCamera");
            StartCoroutine(CameraBack());
        }
        
    }

    private IEnumerator CameraBack()
    {
        _corutineRun = true;
        yield return new WaitForSecondsRealtime(2f);
        gameObject.transform.position = _ship.transform.position;
        _corutineRun = false;
    }
}
