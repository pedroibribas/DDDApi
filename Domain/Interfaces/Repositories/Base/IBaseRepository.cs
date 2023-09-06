namespace Domain.Interfaces.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Obtém uma entidade por uma chave <c>string</c>.
        /// </summary>
        /// <param name="id">Id da entidade na base de dados.</param>
        /// <returns>A entidade encontrada.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<T> GetById(int id);

        Task<List<T>> List();

        Task Add(T Object);
        
        Task Update(T Object);
        
        Task Delete(string id);
                
    }
}
