using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using Library.Repositories.Interfaces;
using Library.Contexts;

namespace Library.Repositories.Base
{
    public abstract class BaseRepository<T, G, V> : IBaseRepository<T>
    where G : BaseEntity
    where V : BaseDBContext<V>
    {
        protected readonly V _context;
        protected IMapper _mapper;

        public BaseRepository(V context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<T>> FindAll()
        {
            var products = await _context.GetDbSetByType<G>().ToListAsync();
            return _mapper.Map<List<T>>(products);
        }

        public virtual async Task<T> FindById(long id)
        {
            return _mapper.Map<T>(await _context.GetDbSetByType<G>().Where(x => x.Id == id).FirstOrDefaultAsync());
        }
        public virtual async Task<T> Create(T vo)
        {
            var product = _mapper.Map<G>(vo);

            _context.GetDbSetByType<G>().Add(product);

            await _context.SaveChangesAsync();

            return vo;
        }

        public virtual async Task<T> Update(T vo)
        {
            var product = _mapper.Map<G>(vo);

            _context.GetDbSetByType<G>().Update(product);

            await _context.SaveChangesAsync();

            return vo;
        }

        public virtual async Task<bool> Delete(long id)
        {
            try
            {
                var product = await _context.GetDbSetByType<G>().Where(x => x.Id == id).FirstOrDefaultAsync();
                

                if(product == null) return false;

                _context.GetDbSetByType<G>().Remove(product);

                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}