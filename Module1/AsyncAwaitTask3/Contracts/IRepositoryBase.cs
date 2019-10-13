﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);

        void Create(T entity);

        void CreateRange(IEnumerable<T> entity);

        void Update(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entity);

        Task SaveAsync();
    }
}
