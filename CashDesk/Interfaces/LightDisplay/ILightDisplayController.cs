namespace CashDesk.LightDisplay;

public interface ILightDisplayController
{
    void TurnExpressLightOn();
    void TurnExpressLightOff();
    bool isOn();
}