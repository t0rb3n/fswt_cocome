using CashDesk.Classes.Enums;

namespace CashDesk.Exceptions;
/// <summary>
/// This exception is thrown when the CashDesk is in a state that it shouldn't be in when we try to do something
/// </summary>
public class IllegalCashDeskStateException : Exception
{
    private CashDeskState? _state;
    private IReadOnlySet<CashDeskState>? _legalStates;

    public IllegalCashDeskStateException()
    {
    }

    public IllegalCashDeskStateException(string message) : base(message)
    {
    }

    public IllegalCashDeskStateException(string message, Exception inner) : base(message, inner)
    {
    }

    public IllegalCashDeskStateException(CashDeskState currentState, IReadOnlySet<CashDeskState> legalStates)
    {
        _state = currentState;
        _legalStates = legalStates;
    }
}