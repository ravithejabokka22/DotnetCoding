

namespace DotnetCoding.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IApprovalQueueRepository Approvals { get; }

        int Save();
    }
}
