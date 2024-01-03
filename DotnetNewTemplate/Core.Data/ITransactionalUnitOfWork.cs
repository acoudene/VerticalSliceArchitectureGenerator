﻿// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

namespace Core.Data;

public interface ITransactionalUnitOfWork : IUnitOfWork
{
  void BeginTrans();
  void Commit();
  void RollBack();
}
