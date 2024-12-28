using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FCpShootControlParam
{
    public Vector2 origin;
    public Vector2 forward;
}

public class CpShootComponent : MonoBehaviour
{
    public GameObject testShotPrefab = null;
    public FCpMoveParamLinear MoveParamLinear;
    // 弾丸発射処理.
    // 外部からのパラメータはcontrolParamで受け取る.
    public void execute(in FCpShootControlParam controlParam)
    {
        CpInputManager input = CpInputManager.Get();
        if (input.WasPressed(ECpButton.Shoot) || input.IsPressHold(ECpButton.Shoot))
        {
            CpShotBase shot = CreateShot(testShotPrefab);
            shot.transform.position = transform.position;
            MoveParamLinear.Degree = SltMath.ToDegree(controlParam.forward);

            ICpMover mover = shot as ICpMover;
            mover.StartMove(MoveParamLinear);
        }
    }

    private void Update()
    {

    }

    CpShotBase CreateShot(GameObject prefab)
    {
        GameObject newObj = Instantiate(prefab);
        return newObj.GetComponent<CpShotBase>();
    }
}
