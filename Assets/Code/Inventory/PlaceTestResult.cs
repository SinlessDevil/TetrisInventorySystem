using System.Collections.Generic;

namespace Code.Inventory
{
    public struct PlaceTestResult
    {
        public bool IsSuccess { get; }
        public IReadOnlyList<int> Passed { get; }
        public IReadOnlyList<int> Blocked { get; }
        
        public PlaceTestResult(bool isSuccess, IReadOnlyList<int> passed, IReadOnlyList<int> blocked)
        {
            IsSuccess = isSuccess;
            Passed = passed;
            Blocked = blocked;
        }

        public static implicit operator bool(PlaceTestResult placeTestResult)
        {
            return placeTestResult.IsSuccess;
        }
    }
}