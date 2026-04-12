using UnityEngine;

public class GameplayModeManager : MonoBehaviour
{
    public static GameplayModeManager Instance;
    public ChangeShipVisibility changeShipVisibility;
    public GameObject player;
    public enum Mode
    {
        PlayerControl,
        ShipSteering,
        CannonShooting
    }

    public Mode currentMode = Mode.PlayerControl;

    void Awake()
    {
        Instance = this;
    }

    public void SetShipSteeringMode(bool enabled)
    {
        if (!((currentMode == Mode.PlayerControl) ^ enabled))
        {
            changeShipVisibility.ToggleVisibility();
            player.GetComponent<Renderer>().enabled = !player.GetComponent<Renderer>().enabled;
        }

        currentMode = enabled ? Mode.ShipSteering : Mode.PlayerControl;

    }

    public void SetCannonShootingMode(bool enabled)
    {
        if (!((currentMode == Mode.PlayerControl) ^ enabled))
        {
            //changeShipVisibility.ToggleVisibility();
            player.GetComponent<Renderer>().enabled = !player.GetComponent<Renderer>().enabled;
        }

        currentMode = enabled ? Mode.CannonShooting : Mode.PlayerControl;

    }

    public bool IsSteeringMode()
    {
        return currentMode == Mode.ShipSteering;
    }

    public bool isCannonShootingMode()
    {
        return currentMode == Mode.CannonShooting;
    }
}