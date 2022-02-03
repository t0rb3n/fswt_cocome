﻿namespace CashDesk.Exceptions;

public class IllegalCashDeskStateException : Exception
{
    private CashDeskState _state;
    private IReadOnlySet<CashDeskState> _legalStates;

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
        this._state = currentState;
        this._legalStates = legalStates;
    }
}