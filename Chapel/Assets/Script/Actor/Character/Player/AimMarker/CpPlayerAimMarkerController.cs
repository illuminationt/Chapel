using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpPlayerAimMarkerController
{
    public CpPlayerAimMarkerController()
    {
        _markerNormalizedPosition = new Vector2(0.5f, 0.5f);
        _markerWorldPosition = CpUtil.GetWorldPositionFromNormalizedPosition(_markerNormalizedPosition);
        _param = CpPlayerSettings.Get().AimMarkerParam;

        _markerObject = MonoBehaviour.Instantiate(_param.MarkerPrefab, _markerWorldPosition, Quaternion.identity);
    }

    public void Update(ECpDirectionInputDevice device)
    {
        switch (device)
        {
            case ECpDirectionInputDevice.Mouse:
                UpdateOnMouse(ref _markerNormalizedPosition);
                break;

            case ECpDirectionInputDevice.RightStick:
                UpdateOnRightStick(ref _markerNormalizedPosition);
                break;

            default:
                throw new System.NotImplementedException();
        }

        _markerWorldPosition = CpUtil.GetWorldPositionFromNormalizedPosition(_markerNormalizedPosition);

        _markerObject.transform.position = _markerWorldPosition;
    }

    public Vector2 GetAimMarkerWorldPosition()
    {
        return _markerWorldPosition;
    }

    void UpdateOnRightStick(ref Vector2 markerNormalizedPosition)
    {
        var input = CpInputManager.Get();
        Vector2 currentDirection = input.GetDirectionInput();

        const float directionInputDeadZone = 0.8f;
        float currentDirInputSize = currentDirection.magnitude;
        if (currentDirInputSize < directionInputDeadZone)
        {
            // “ü—Í‚ª¬‚³‚¢‚È‚ç‰½‚à‚µ‚È‚¢
            return;
        }

        float moveSpeed = _param.MoveSpeedOnRightStickOnScreenSpace;
        Vector2 deltaMove = currentDirection * moveSpeed * CpTime.DeltaTime;
        markerNormalizedPosition += deltaMove;
    }

    void UpdateOnMouse(ref Vector2 markerNormalizedPosition)
    {
        var input = CpInputManager.Get();
        Vector2 mouseScreenPosition = input.GetMouseLocation();
        markerNormalizedPosition = CpUtil.GetNormalizedPositionFromScreenPosition(mouseScreenPosition);
    }

    Vector2 _markerNormalizedPosition;
    Vector2 _markerWorldPosition;
    CpPlayerAimMarkerParam _param;
    CpPlayerAimMarker _markerObject;
}
