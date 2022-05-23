using System;

public class WaterWorkerAI : AIMain
{
    private AIMover _aiMover;

    private void Start()
    {
        _aiMover = GetComponent<AIMover>();
        _aiMover.SetSpeed(1.5f);
    }

    private void Update()
    {
        TimerCounter();
        FoodAnimationConrol();
        DoSomeTask();
        WaterWork();
        CheckSpace();
    }
}
