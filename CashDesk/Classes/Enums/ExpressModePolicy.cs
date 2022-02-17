namespace CashDesk.Classes.Enums;
// per paper [Fig 4. - conditions] 
// only 8 products
// has to pay by cash
public abstract class ExpressModePolicy
{
    public const int ExpressItemsLimit = 8;
    public const int CheckPeriodSeconds = 60;
    public const double ExpressThreshold = 0.5;
    public const double SalesWindow = 1;
}