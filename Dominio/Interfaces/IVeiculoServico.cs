using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPI.Dominio.Entidades;

namespace MinimalAPI.Dominio.Interfaces
{
    public interface IVeiculoServico
    {
        List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null);
        Veiculo? BuscarPorId(int id);

        void Adicionar(Veiculo veiculo);

        void Atualizar(Veiculo veiculo);

        void Deletar(Veiculo veiculo);
    }
}
