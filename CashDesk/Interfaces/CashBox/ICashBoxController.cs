using CashDesk.Datatypes;

namespace CashDesk.CashBox;

public interface ICashBoxController
{
    void Open();
    void Close();
    void IsOpen();
    void PressControlKey(ControlKeyStroke keyStroke);
    void PressNumpadKey(NumpadKeyStroke key);
}