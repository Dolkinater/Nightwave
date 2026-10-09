using UnityEngine;

public class RogStasisCrouch : MonoBehaviour
{
    // statsis temp fix helps hold player animator in place
    // however, the sliding state seems to be animated at a lower elevation than the rest of the animations
    // so I manually modify the position of the animator during only the sliding state

    [SerializeField] RogStasisTempFix tempFix;

    Vector3 storedPos;
    bool storing = false;

    [SerializeField] PlayerStateMachine sm;

    private void Update()
    {
        if (sm.GetCurrentState() == sm.SlideState)
        {
            if (!storing)
            {
                storing = true;
                storedPos = tempFix.GetLocalPos();

                tempFix.SetLocalPos(new Vector3(0, -.6f, 0));
            }

            return;
        }

        if (storing)
        {
            storing = false;
            tempFix.SetLocalPos(storedPos);
        }
    }
}
