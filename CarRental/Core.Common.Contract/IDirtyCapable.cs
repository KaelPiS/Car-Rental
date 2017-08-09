using System.Collections.Generic;

namespace Core.Common.Contract
{
    public interface IDirtyCapable
    {
        bool IsDirty { get; }
        bool isAnythingDirty();
        List<IDirtyCapable> GetDirtyObjects();
        void CleanAll();
    }
}
