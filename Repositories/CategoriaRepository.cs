using System.Collections.Generic;
using System.Threading.Tasks;
using GUFOS_BackEnd.Domains;
using GUFOS_BackEnd.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GUFOS_BackEnd.Repositories
{
    public class CategoriaRepository : ICategoria
    {
        public async Task<Categoria> Alterar(Categoria categoria)
        {
            using(GufosContext _context = new GufosContext()){
                _context.Entry(categoria).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return categoria;
        }

        public async Task<Categoria> BuscarPorID(int id)
        {
            using(GufosContext _context = new GufosContext()){
                return await _context.Categoria.FindAsync(id);
            }
        }

        public async Task<Categoria> Excluir(Categoria categoria)
        {
            using(GufosContext _context = new GufosContext()){
                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();
                return categoria;                
            }
        }

        public async Task<List<Categoria>> Listar()
        {
            using(GufosContext _context = new GufosContext()){
                return await _context.Categoria.ToListAsync();
            }
        }

        public async Task<Categoria> Salvar(Categoria categoria)
        {
            using(GufosContext _context = new GufosContext()){
                await _context.AddAsync(categoria);
                await _context.SaveChangesAsync();
                return categoria;
            }
        }
    }
}