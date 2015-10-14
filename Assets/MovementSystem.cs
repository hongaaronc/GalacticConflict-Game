using UnityEngine;
using System.Collections;

public class MovementSystem : GenericSystem
{
    public ShipMovement2D shipMovement;
    public int coolDown;
    public AnimationCurve forceCurveX;
    public AnimationCurve forceCurveY;
    public AnimationCurve forceCurveZ;
    public AnimationCurve rotationForceCurveX;
    public AnimationCurve rotationForceCurveY;
    public AnimationCurve rotationForceCurveZ;

    public float topAngularSpeed;
    private float normalTopAngularSpeed;

    private bool myKeyDown = false;
    private int timer = 0;

    private float moveTimer;
    public float moveTime;

    private NetworkView myNetworkView;
    private NetworkManager myNetworkManager;
    // Use this for initialization
    void Start()
    {
        moveTimer = moveTime;
        timer = coolDown + 1;
        myNetworkView = GetComponent<NetworkView>();
        myNetworkManager = Camera.main.GetComponent<NetworkManager>();
        normalTopAngularSpeed = shipMovement.topAngularSpeed;
    }


    void FixedUpdate()
    {
        if (moveTimer <= moveTime)
        {
            shipMovement.GetComponent<Rigidbody>().maxAngularVelocity = topAngularSpeed;
            shipMovement.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(forceCurveX.Evaluate(moveTimer / moveTime), forceCurveY.Evaluate(moveTimer / moveTime), forceCurveZ.Evaluate(moveTimer / moveTime)));
            shipMovement.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(rotationForceCurveX.Evaluate(moveTimer / moveTime), rotationForceCurveY.Evaluate(moveTimer / moveTime), rotationForceCurveZ.Evaluate(moveTimer / moveTime)));
            if (moveTimer == moveTime)
            {
                shipMovement.GetComponent<Rigidbody>().maxAngularVelocity = normalTopAngularSpeed;
            }
            moveTimer++;
        }
        else
        {
            if (timer <= coolDown)
            {
                if (myKeyDown)
                {
                    moveTimer = 0f;
                    timer = 0;
                }
                timer++;
            }
        }
        myKeyDown = false;
    }

    public override void Activate()
    {
        myKeyDown = true;
        if (timer > coolDown)
            timer = 0;
    }
}
