﻿using ListaDeFilmes.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ListaDeFilmes.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity         //IDisposable serve para liberar memória
    {
        Task Adicionar(TEntity entity);

        Task<TEntity> ObterPorId(Guid id);

        Task<TEntity> ObterPorId(Guid? id);

        Task<List<TEntity>> ObterTodos();

        Task Atualizar(TEntity entity);

        Task Remover(Guid id);

        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);  //busca por qualquer parametro atraves de expressões lambda

        Task<int> SaveChanges();  // int porque retorna o numero de linhas afetadas

        Task<bool> IdExiste(Guid id); //verifica se o id existe

    }
}
