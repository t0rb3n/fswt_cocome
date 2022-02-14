namespace CashDesk;
// per paper [Fig 4. - conditions] 
// only 8 products
// has to pay by cash
public abstract class ExpressModePolicy
{
    public static int _expressItemsLimit = 8;
    public static int _checkPeriodSeconds = 60;
    public static double _expressThreshold = 0.5;
    public static double _salesWindow = 1;
    
}