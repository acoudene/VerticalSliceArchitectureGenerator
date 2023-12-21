namespace $safeprojectname$;

public interface ITransactionalUnitOfWork : IUnitOfWork
{
  void BeginTrans();
  void Commit();
  void RollBack();
}
