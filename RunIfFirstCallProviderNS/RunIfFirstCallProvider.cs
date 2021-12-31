using SpinLockUtilNS;

namespace RunIfFirstCallProviderNS;

public class RunIfFirstCallProvider
{
    private readonly Action _action;
    private int _wasCalled = SpinLockUtil.False;

    public bool WasCalled => SpinLockUtil.IsLockedOnce(isLocked: ref _wasCalled);
    public bool WasNotCalled => SpinLockUtil.IsUnlocked(isLocked: ref _wasCalled);

    public RunIfFirstCallProvider(Action action)
    {
        _action = action;
    }

    public void RunIfFirstCall()
    {
        if (SpinLockUtil.TryLock(isLocked: ref _wasCalled)) _action();
    }
}