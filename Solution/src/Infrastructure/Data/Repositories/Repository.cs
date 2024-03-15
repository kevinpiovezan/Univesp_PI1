using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Interfaces.Repository;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Repositories
{
    public abstract class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        protected readonly AppDbContext _Context;

        protected Repository(AppDbContext context)
        {
            _Context = context;
        }
      
        public void Add(T objeto)
        {
            _Context.Set<T>().Add(objeto);
        }
        public void Update(T objeto)
        {
            _Context.Set<T>().Update(objeto);
        }
        public void Remove(T objeto)
        {
            _Context.Set<T>().Remove(objeto);
        }

        public void BeginTransaction()
        {
            _Context.Database.BeginTransaction();
        }
        public Task<int> ExecuteStrategy(Task<int> dbFunction)
        {
            var strategy = _Context.Database.CreateExecutionStrategy();
            return strategy.Execute(() => dbFunction);
        }
        public void CommitTransaction()
        {
            _Context.Database.CommitTransaction();
        }

        public async Task Adicionar(T objeto)
        {
            await _Context.Set<T>().AddAsync(objeto);
            await _Context.SaveChangesAsync();
        }

        public async Task Remover(T objeto)
        {
            _Context.Set<T>().Remove(objeto);
            await _Context.SaveChangesAsync();
        }

        public async Task<T> ObterPorId(int id)
        {
            return await _Context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ObterTodos()
        {
            return await _Context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> ObterTodosTracked()
        {
            return await _Context.Set<T>().ToListAsync();
        }

        public async Task Atualizar(T objeto)
        {
            _Context.Set<T>().Update(objeto);
            await _Context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _Context?.Dispose();
        }
    }
}
