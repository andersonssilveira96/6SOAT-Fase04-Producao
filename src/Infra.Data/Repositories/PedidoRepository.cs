﻿using Domain.Repositories;
using Domain.Entities;
using Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly TechChallengeContext _context;
        public PedidoRepository(TechChallengeContext context)
        {
            _context = context;
        }

        public async Task<Pedido> Inserir(Pedido pedido)
        {
            if (pedido is null)
            {
                throw new ArgumentNullException(nameof(pedido));
            }

            _context.Pedido.Add(pedido);

            await _context.SaveChangesAsync();

            return pedido;
        }
        public virtual async Task<Pedido> Atualizar(Pedido pedido)
        {
            var entry = _context.Entry(pedido);

            _context.Pedido.Update(entry.Entity);

            await _context.SaveChangesAsync();

            return pedido;
        }

        public async Task<List<Pedido>> ListarPedidos() => await _context.Pedido.ToListAsync();
        public async Task<Pedido> ObterPorId(long id) => await _context.Pedido.FirstOrDefaultAsync(x => x.Id == id);

    }
}
