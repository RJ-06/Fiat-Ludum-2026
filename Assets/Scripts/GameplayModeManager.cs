using UnityEngine;

public class GameplayModeManager : MonoBehaviour
{
    public static GameplayModeManager Instance;
    public enum Mode
    {
        PlayerControl,
        ShipSteering,
        Cooking,
        Fishing,
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
            ChangeShipVisibility.Instance.ToggleVisibilityTop();
            PlayerMovement.instance.playerRend.enabled = !PlayerMovement.instance.playerRend.enabled;
        }

        currentMode = enabled ? Mode.ShipSteering : Mode.PlayerControl;

    }

    public void SetCookingMode(bool enabled)
    {
        if (!((currentMode == Mode.PlayerControl) ^ enabled))
        {
            PlayerMovement.instance.playerRend.enabled = !PlayerMovement.instance.playerRend.enabled;
        }

        currentMode = enabled ? Mode.Cooking : Mode.PlayerControl;
    }

    public void SetFishingMode(bool enabled)
    {
        if (!((currentMode == Mode.PlayerControl) ^ enabled))
        {
            PlayerMovement.instance.playerRend.enabled = !PlayerMovement.instance.playerRend.enabled;
        }

        currentMode = enabled ? Mode.Fishing : Mode.PlayerControl;
    }

    public void SetCannonShootingMode(bool enabled)
    {
        if (!((currentMode == Mode.PlayerControl) ^ enabled))
        {
            PlayerMovement.instance.playerRend.enabled = !PlayerMovement.instance.playerRend.enabled;
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

    public bool isFishingMode() 
    {
        return (currentMode == Mode.Fishing);
    }

    public bool isCookingMode()
    {
        return (currentMode == Mode.Cooking);
    }

    public bool isWalkingMode() 
    {
        return (currentMode == Mode.PlayerControl);
    }
}