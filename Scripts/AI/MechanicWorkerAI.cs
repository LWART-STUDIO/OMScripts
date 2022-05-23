public class MechanicWorkerAI : AIMain
{
    public bool WorkDone=false;
    public bool Repeir;
    private void Update()
    {
        CheckSpace();
        TimerCounter();
        DoSomeTask();
        if (WorkDone && Tasks.Count > 0)
        {
            if (Tasks[0].Name != "Rest")
            {
                Tasks.Clear();
            }

            WorkDone = false;
        }
        AnimationControl();
    }

    private void AnimationControl()
    {
        if (Mover.IsMoving && !Repeir)
        {
            AiAnimationControl.MechanicRun();
        }
        else if (!Mover.IsMoving && !Repeir)
        {
            AiAnimationControl.MechanicIdle();
        }
        else if (!Mover.IsMoving && Repeir)
        {
            if (Tasks.Count >0)
            {
                transform.rotation = Tasks[0].Rotation;
            }
            AiAnimationControl.MechanicWork();
        }
    }
}
