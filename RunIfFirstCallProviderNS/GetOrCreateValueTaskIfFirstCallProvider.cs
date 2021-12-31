using SpinLockUtilNS;

namespace RunIfFirstCallProviderNS;

public class GetOrCreateValueTaskIfFirstCallProvider
{
    private readonly Lazy<ValueTask> _lazyValueTask;
    private int _wasCalled = SpinLockUtil.False;

    public bool WasCalled => SpinLockUtil.IsLockedOnce(isLocked: ref _wasCalled);
    public bool WasNotCalled => SpinLockUtil.IsUnlocked(isLocked: ref _wasCalled);

    public GetOrCreateValueTaskIfFirstCallProvider(Func<ValueTask> getValueTaskFunc)
    {
        _lazyValueTask = new Lazy<ValueTask>(valueFactory: getValueTaskFunc);
    }

    public ValueTask GetValueTaskIfFirstCall()
    {
        // Notice that, even if `Increment` implementation can theoretically be cheaper than `CAS`,
        // it is still correct to use `CAS` in this case to prevent theoretically possible int overflow
        SpinLockUtil.TryLock(isLocked: ref _wasCalled);
        return _lazyValueTask.Value;
    }
}