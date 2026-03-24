using UnityEngine;

public class TestIK : MonoBehaviour
{
    private Animator animator;  
    [SerializeField]
    private float weightIK;

    //Right Hand

    [SerializeField]
    private Transform handRTarget;
    [SerializeField]
    private Transform elbowR;

    //Left Hand

    [SerializeField]
    private Transform handLTarget;
    [SerializeField]
    private Transform elbowL;

    //Right Foot

    [SerializeField]
    private Transform footRTarget;
    [SerializeField]
    private Transform kneeR;

    //Left Foot

    [SerializeField]
    private Transform footLTarget;
    [SerializeField]
    private Transform kneeL;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //Right Hand
        if (handRTarget != null)
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, handRTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, handRTarget.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weightIK);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weightIK);

            animator.SetIKHintPosition(AvatarIKHint.RightElbow, elbowR.position);
            animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, weightIK);
        }

        //Leftt Hand
        if (handLTarget != null)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, handLTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, handLTarget.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weightIK);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weightIK);

            animator.SetIKHintPosition(AvatarIKHint.LeftElbow, elbowL.position);
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, weightIK);
        }

        //Right Foot
        if (footRTarget != null)
        {
            animator.SetIKPosition(AvatarIKGoal.RightFoot, footRTarget.position);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, footRTarget.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weightIK);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weightIK);

            animator.SetIKHintPosition(AvatarIKHint.RightKnee, kneeR.position);
            animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, weightIK);
        }

        //Left Foot
        if (footLTarget != null)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, footLTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, footLTarget.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weightIK);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weightIK);

            animator.SetIKHintPosition(AvatarIKHint.LeftKnee, kneeL.position);
            animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, weightIK);
        }
    }
}
