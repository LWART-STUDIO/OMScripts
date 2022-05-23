public class FoodWorkerAI : AIMain
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
        FoodWork();
        CheckSpace();
    }
    
}
