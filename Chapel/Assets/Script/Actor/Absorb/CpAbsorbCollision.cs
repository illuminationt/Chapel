using Unity.VisualScripting;
using UnityEngine;

public class CpAbsorbCollision : MonoBehaviour, ICpAbsorbable
{

    [SerializeField] Transform _transform = null;

    // I
    public Transform GetAbsorbRootTransform() { return _transform; }


    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        ICpAbsorbTarget absorbTarget = collider.GetComponent<ICpAbsorbTarget>();
        if (absorbTarget != null)
        {
            if (absorbTarget.CanAbsorb(this))
            {
                absorbTarget.StartAbsorb(this);
            }
        }
    }
}
