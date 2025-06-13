using BNPL.Api.Server.src.Application.Abstractions.Repositories;
using Core.Models;
using Dapper;
using System.Data;
using System.Reflection;

namespace BNPL.Api.Server.src.Infrastructure.Persistence.Repositories
{
    public abstract class GenericRepository<T>(IDbConnection connection) : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly IDbConnection Connection = connection;

        public virtual async Task<T?> GetByIdAsync(Guid code, IDbTransaction? transaction = null)
        {
            var sql = $"SELECT * FROM \"{GetTableName()}\" WHERE code = @Code";
            return await Connection.QuerySingleOrDefaultAsync<T>(sql, new { Code = code }, transaction);
        }

        public virtual async Task<int> InsertAsync(T entity, IDbTransaction? transaction = null)
        {
            var (columns, values) = GetColumnsAndValues(entity);
            var sql = $"INSERT INTO \"{GetTableName()}\" ({columns}) VALUES ({values}) RETURNING 1";
            return await Connection.ExecuteAsync(sql, entity, transaction);
        }

        public virtual async Task<bool> UpdateAsync(T entity, IDbTransaction? transaction = null)
        {
            var setClause = GetUpdateSetClause(entity);
            var sql = $"UPDATE \"{GetTableName()}\" SET {setClause} WHERE code = @Code";
            var affectedRows = await Connection.ExecuteAsync(sql, entity, transaction);
            return affectedRows > 0;
        }

        public virtual async Task<bool> InactivateAsync(Guid code, Guid updatedBy, IDbTransaction? transaction = null)
        {
            var sql = $"""
                UPDATE "{GetTableName()}"
                SET is_active = FALSE,
                    updated_by = @UpdatedBy,
                    updated_at = @UpdatedAt
                WHERE code = @Code
                """;
            var affectedRows = await Connection.ExecuteAsync(sql, new { Code = code, UpdatedBy = updatedBy, UpdatedAt = DateTime.UtcNow }, transaction);
            return affectedRows > 0;
        }

        #region Helpers
        private static string GetTableName()
        {
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>();
            if (tableAttr != null) return tableAttr.Name;
            return type.Name.ToLowerInvariant();
        }

        private static IEnumerable<PropertyInfo> GetMappableProperties()
        {
            return typeof(T).GetProperties().Where(p => p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>() == null);
        }

        private static (string columns, string values) GetColumnsAndValues(T entity)
        {
            var properties = GetMappableProperties();
            var columns = string.Join(", ", properties.Select(p => $"\"{p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>()?.Name ?? p.Name.ToLowerInvariant()}\""));
            var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return (columns, values);
        }

        private static string GetUpdateSetClause(T entity)
        {
            var properties = GetMappableProperties().Where(p => p.Name != "Code" && p.Name != "Id");
            var setClauses = properties.Select(p => $"\"{p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>()?.Name ?? p.Name.ToLowerInvariant()}\" = @{p.Name}");
            return string.Join(", ", setClauses);
        }
        #endregion
    }
}