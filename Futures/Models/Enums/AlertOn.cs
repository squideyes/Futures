// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures.Models;

public enum AlertOn
{
    LoginComplete,
    LoginFailure,
    Dropped,
    Reconnected,
    InputNeeded,
    LowMargin,
    SortieResult,
    JunketSummary,
    PlanModified,
    MarginCall,
    ManualGoFlat,
    Statements,
    VendorGoFlat,
    VendorModify,
    EndOfSession,
    ErrorShutdown
}