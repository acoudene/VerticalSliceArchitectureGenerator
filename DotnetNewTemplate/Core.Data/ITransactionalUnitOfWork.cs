namespace Core.Data;

public interface ITransactionalUnitOfWork : IUnitOfWork
{
  void BeginTrans();
  void Commit();
  void RollBack();
}
