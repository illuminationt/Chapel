using UnityEngine;

public partial class CpActorBase
{
    public bool IsPlayerInRadius(float radius)
    {
        CpPlayer player = CpGameManager.Instance.Player;
        Vector2 playerPos = player.transform.position;
        Vector2 selfPos = _transform.position;
        float distSq = selfPos.GetSquaredDistanceTo(playerPos);

        return distSq < radius * radius;
    }
}
