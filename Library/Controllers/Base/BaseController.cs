using Microsoft.AspNetCore.Mvc;
using Library.Controllers.Interfaces;
using Library.Repositories.Base;
using Library.Models;
using Library.Contexts;

namespace Library.Controllers.Base
{
    public abstract class BaseController<T, V, G, S> : ControllerBase, IBaseController<V>
        where T : BaseRepository<V, G, S>
        where G : BaseEntity
        where S : BaseDBContext<S>
    {
        protected T _repository;

        public BaseController(T repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<V>>> FindAll()
        {
            var products = await _repository.FindAll();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<V>> FindById(long id)
        {
            var product = await _repository.FindById(id);

            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public virtual async Task<ActionResult<V>> Create(V vo)
        {
            if (vo == null) return BadRequest();

            var product = await _repository.Create(vo);

            return Ok(product);
        }

        [HttpPut]
        public virtual async Task<ActionResult<V>> Update(V vo)
        {
            if (vo == null) return BadRequest();

            var product = await _repository.Create(vo);

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(long id)
        {
            var status =  await _repository.Delete(id);

            if(!status) return BadRequest();

            return Ok(status);
        }
    }
}