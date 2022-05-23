using System.Collections;
using UnityEngine;

public class CameraControllManager : MonoBehaviour
{
    [SerializeField] private Animator _camerasAnimator;
    public bool CameraMove;



    public void SwitchCamera(string name)
    {
        if (name == "OilPump")
        {
            SwitchToMainCamera();
        }
        else if (name == "Tanker")
        {
            SwitchToTankerCamera();
        }
        else if (name == "PlasticCreator")
        {
            SwitchToPlasticCreatorCamera();
        }
        else if (name == "FoodCreator")
        {
            SwitchToFoodCreator();
        }
        else if (name == "OilPump")
        {
            SwitchToOilPumpCamera();
        }
        else if (name == "WaterPump")
        {
            SwitchToWaterPumpCamera();
        }
        else if (name == "Bur")
        {
            SwitchToBurCamera();
        }
        else if (name == "Shop")
        {
            SwitchToShopCamera();
        }
        else if (name == "Mechanic")
        {
            SwitchToMechanicSpwnerCamera();
        }
        else if (name == "Transporter")
        {
            SwitchToTransporterCamera();
        }
        else if (name == "FoodWorker1")
        {
            SwitchToFoodWorker1Camera();
        }
        else if (name == "FoodWorker2")
        {
            SwitchToFoodWorker2Camera();
        }
        else if (name == "WaterWorker")
        {
            SwitchToWaterWorkerCamera();
        }
    }
    public void SwitchToMainCamera()
    {
        _camerasAnimator.Play("MainCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }

    public void SwitchToOilPumpCamera()
    {
        _camerasAnimator.Play("OilPumpCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToPlasticCreatorCamera()
    {
        _camerasAnimator.Play("PlasticCreatorCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToFoodCreator()
    {
        _camerasAnimator.Play("FoodCreatorCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToBurCamera()
    {
        _camerasAnimator.Play("BurCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToTransporterCamera()
    {
        _camerasAnimator.Play("TransporterCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToMechanicSpwnerCamera()
    {
        _camerasAnimator.Play("MechanicSpwnerCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToWaterPumpCamera()
    {
        _camerasAnimator.Play("WaterPumpCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToTankerCamera()
    {
        _camerasAnimator.Play("TankerCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToShopCamera()
    {
        _camerasAnimator.Play("ShopCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToDronCamera()
    {
        _camerasAnimator.Play("DronCamera");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }

    public void SwitchToFoodWorker1Camera()
    {
        _camerasAnimator.Play("FoodWorker1");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToFoodWorker2Camera()
    {
        _camerasAnimator.Play("FoodWorker2");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }
    public void SwitchToWaterWorkerCamera()
    {
        _camerasAnimator.Play("WaterWorker");
        CameraMove = true;
        StopAllCoroutines();
        StartCoroutine(WaitForCanAdd());
    }

    private IEnumerator WaitForCanAdd()
    {
       
        CameraMove = true;
        TimerAds.Finished = false;
        float timer = 30f;
        if (TimerAds.Timer >= (TimerAds.TimerMax - timer))
        {
            TimerAds.Timer -= timer;
        }
        while (timer>0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        CameraMove = false;

    }
}

