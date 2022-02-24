namespace CashDesk.Domain.Enums;

/// <summary>
///  Per paper [Fig 4 - conditions]
/// Describes certain thresholds that needs to be used to determine whether we should switch to express mode or not
/// </summary>
public abstract class ExpressModePolicy
{
    public const int ExpressItemsLimit = 8;
    public const int CheckPeriodSeconds = 60;
    public const double ExpressThreshold = 0.5;
    public const double SalesWindow = 1;
}