using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class SubCategoriaDA : ISubCategoriaDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        public SubCategoriaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }


        public async Task<IEnumerable<SubCategoria>> ObtenerSubCategoria(Guid CategoriaId)
        {
            string query = @"ObtenerSubCategorias";
            var resultado = await _sqlConnection.QueryAsync<SubCategoria>(query, new
            {
                IdCategoria=CategoriaId
            });
            return resultado;
        }
    }
}
