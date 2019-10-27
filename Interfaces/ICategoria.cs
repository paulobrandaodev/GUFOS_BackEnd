using System.Collections.Generic;
using System.Threading.Tasks;
using GUFOS_BackEnd.Domains;
using Microsoft.AspNetCore.Mvc;

namespace GUFOS_BackEnd.Interfaces
{
    public interface ICategoria
    {
        Task<List<Categoria>> Listar();

        Task<Categoria> BuscarPorID(int id);

        Task<Categoria> Salvar(Categoria categoria);

        Task<Categoria> Alterar(Categoria categoria);

        Task<Categoria> Excluir(Categoria categoria);

    }
}