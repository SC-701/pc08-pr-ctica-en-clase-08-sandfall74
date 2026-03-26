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
    public class ProductoDA : IProductoDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        
        public ProductoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }



        public async Task<Guid> Agregar(ProductoRequest producto)
        {
            string query = @"AgregarProducto";
            var result = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id = Guid.NewGuid(),
                producto.Nombre,
                producto.Descripcion,
                producto.Precio,
                producto.IdSubCategoria,
                producto.Stock,
                producto.CodigoBarras
            });
            return result;
        }

        public async Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            string query = @"ActualizarProducto";
            var result = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id,
                producto.Nombre,
                producto.Descripcion,
                producto.Precio,
                producto.IdSubCategoria,

                producto.Stock,
                producto.CodigoBarras
            });
            return result;
        }

        public async Task<Guid> Eliminar(Guid Id)
        {
            string query = @"EliminarProducto";
            var result = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                Id
            }, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
           string query = @"ObtenerProductos";
            var result = await _sqlConnection.QueryAsync<ProductoResponse>(query);
            return result;

        }

        public Task<ProductoResponse> Obtener(Guid Id)
        {
            string query = @"ObtenerProducto";
            var result =  _sqlConnection.QueryFirstOrDefaultAsync<ProductoResponse>(query, new
            {
                Id
            });
            return result;
        }
    }
}
