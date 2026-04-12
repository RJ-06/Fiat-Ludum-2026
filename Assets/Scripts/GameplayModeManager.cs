using UnityEngine;

public class GameplayModeManager : MonoBehaviour
{
    public static GameplayModeManager Instance;
    public ChangeShipVisibility changeShipVisibility;
    public enum Mode
    {
        PlayerControl,
        ShipSteering
    }

    public Mode currentMode = Mode.PlayerControl;

    void Awake()
    {
        Instance = this;
    }

    public void SetShipSteeringMode(bool enabled)
    {
        if (!((currentMode == Mode.PlayerControl) ^ enabled))
            changeShipVisibility.ToggleVisibility();

        currentMode = enabled ? Mode.ShipSteering : Mode.PlayerControl;

    }

    public bool IsSteeringMode()
    {
        return currentMode == Mode.ShipSteering;
    }
}