﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetCoding.Core.Interfaces;

namespace DotnetCoding.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextClass _dbContext;
        public IProductRepository Products { get; }
        public IApprovalQueueRepository Approvals { get; }

        public UnitOfWork(DbContextClass dbContext,
                            IProductRepository productRepository,IApprovalQueueRepository approvals)
        {
            _dbContext = dbContext;
            Products = productRepository;
            Approvals = approvals;
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

    }
}
