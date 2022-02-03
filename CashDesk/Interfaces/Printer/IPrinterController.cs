namespace CashDesk.Printer;

public interface IPrinterController
{
    void TearOffPrintout();
    void PrintText();
    string GetCurrentPrintout();
}