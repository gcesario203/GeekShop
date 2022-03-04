using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers.Interfaces
{
    public interface IBaseController<T>
    {
        Task<ActionResult<IEnumerable<T>>> FindAll();

        Task<ActionResult<T>> FindById(long id);

        Task<ActionResult<T>> Create(T vo);
        Task<ActionResult<T>> Update(T vo);
        Task<ActionResult> Delete(long id);
    }
}