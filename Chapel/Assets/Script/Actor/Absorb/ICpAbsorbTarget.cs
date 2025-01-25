using UnityEngine;

public interface ICpAbsorbTarget
{
    public bool CanAbsorb(ICpAbsorbable absorbable);
    public void StartAbsorb(ICpAbsorbable absorbable);
}
