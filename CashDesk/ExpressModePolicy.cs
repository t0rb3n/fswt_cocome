namespace CashDesk;


//TODO maybe change to lower view privileges


// per paper [Fig 4. - conditions] 
// only 8 products
// has to pay by cash
public abstract class ExpressModePolicy
{
    public static int _expressItemsLimit = 8;
}