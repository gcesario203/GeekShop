using Library.Services.Interfaces;
using Library.Utils.Extensions;

namespace Library.Services.Base
{
    public class BaseService<T> : IService<T>
    {
        private readonly HttpClient _client;
        public string BasePath { get; protected set; }

        public BaseService(HttpClient client)
        {
            _client =client ?? throw new ArgumentNullException($"Something goes creating a HTTP service for: {BasePath}");
        }
        public virtual async Task<IEnumerable<T>> FindAll()
        {
            var response = await _client.GetAsync(BasePath);

            return await response.ReadContentAs<List<T>>();
        }

        public virtual async Task<T> FindById(long id)
        {
            var response = await _client.GetAsync($"{BasePath}/{id}");

            return await response.ReadContentAs<T>();
        }

        public virtual async Task<T> Create(T model)
        {
            var response = await _client.PostAsJson<T>(BasePath, model);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Something goes wrong creating on: {BasePath}");

            return await response.ReadContentAs<T>();
        }

        public virtual async Task<T> Update(T model)
        {
            var response = await _client.PutAsJson<T>(BasePath, model);

            if (!response.IsSuccessStatusCode)
                new Exception($"Something goes wrong updating on: {BasePath}");

            return await response.ReadContentAs<T>();
        }

        public virtual async Task<bool> Delete(long id)
        {
            var response = await _client.DeleteAsync($"{BasePath}/{id}");

            if(!response.IsSuccessStatusCode)
                new Exception($"Something goes wrong deleting on: {BasePath}");

            return await response.ReadContentAs<bool>();
        }
    }
}