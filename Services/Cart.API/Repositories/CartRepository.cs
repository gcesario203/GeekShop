using AutoMapper;
using Cart.API.Data.ValueObjects;
using Cart.API.Model.Context;
using Cart.API.Repositories.Interfaces;
using Cart.API.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace Cart.API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly MySqlContext _context;
        private IMapper _mapper;

        public CartRepository(MySqlContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CartVO> FindCartByUserId(string userId)
        {
            var header = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);

            return _mapper.Map<CartVO>(new Model.Entity.Cart { CartHeader = header, CartDetails = await _context.CartDetails.Where(x => x.CartHeaderId == header.Id).Include(x => x.Product).ToListAsync() });
        }

        public async Task<CartVO> SaveOrUpdateCart(CartVO cart)
        {
            var cartEntity = _mapper.Map<Model.Entity.Cart>(cart);

            SynchCartProducts(cart, cartEntity);

            // Pega o cabeçalho do carrinho
            var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);

            // Caso o cabeçalho não exista, insere ele no banco, junto com os detalhes
            if (cartHeader == null)
            {
                _context.CartHeaders.Add(cartHeader);

                await _context.SaveChangesAsync();

                SynchCartDetails(cart, cartEntity, cartHeader);

                return _mapper.Map<CartVO>(cartEntity);
            }

            UpdateCartDetails(cart, cartEntity, cartHeader);

            return _mapper.Map<CartVO>(cartEntity);
        }

        public async Task<bool> RemoveFromCart(long cartDetailsId)
        {
            try
            {
                var cartDetail = await _context.CartDetails.FirstOrDefaultAsync(x => x.Id == cartDetailsId);

                var total = await _context.CartDetails.Where(x => x.CartHeaderId == cartDetail.CartHeaderId).CountAsync();

                _context.CartDetails.Remove(cartDetail);

                if (total > 1)
                    return true;

                var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(x => x.Id == cartDetail.CartHeaderId);

                _context.CartHeaders.Remove(cartHeaderToRemove);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<bool> ClearCart(string userId)
        {
           var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);

           if(cartHeader == null)
            return false;

            var cartDetailsToRemove = await _context.CartDetails.Where(x => x.CartHeaderId == cartHeader.Id).ToListAsync();

            _context.CartDetails.RemoveRange(cartDetailsToRemove);

            _context.CartHeaders.RemoveRange(cartHeader);

            await _context.SaveChangesAsync();

            return true;
        }

        private async void SynchCartProducts(CartVO cart, Model.Entity.Cart cartEntity)
        {
            // Lista de produtos no carrinho
            var productsInTheCart = cart.CartDetails.Select(x => x.ProductId);

            // Lista de produtos sincronizados no carrinho
            var synchronizedProducts = await _context.Products.Where(x => productsInTheCart.Contains(x.Id)).ToListAsync();

            // Lista de produtos não sincronizados
            var notSynchedProducts = cartEntity.CartDetails.Select(x => x.Product).Where(x => !synchronizedProducts.Select(x => x.Id).Contains(x.Id));

            // Insere os produtos que estão no carrinho mas não estão sincronizados
            _context.Products.AddRange(notSynchedProducts);

            // Commita os novos produtos
            await _context.SaveChangesAsync();
        }

        private async void SynchCartDetails(CartVO cart, Model.Entity.Cart cartEntity, Model.DataModel.CartHeader cartHeader)
        {
            // Itera os cart details, verificando se o id do cabeçalho ja foi inserido
            // IMPORTANTE:: ISSO É FEITO PRA NÃO DAR CONFLITO COM O ENTITY
            // IMPORTANTE:: Este é um add """seco""" pois caso, o cliente não possua um cartHeader,
            // podemos subentender que o cliente não tem detalhes de pedido
            _context.CartDetails.AddRange(cartEntity.CartDetails.Where(x => x.CartHeaderId == 0).Select(x => { x.CartHeaderId = cartHeader.Id; x.Product = null; return x; }).ToList());

            await _context.SaveChangesAsync();
        }

        private async void UpdateCartDetails(CartVO cart, Model.Entity.Cart cartEntity, Model.DataModel.CartHeader cartHeader)
        {
            // Prepara os itens que estão na VO
            var existingItemsFromEntity = cartEntity.CartDetails.Where(x => x.CartHeaderId == cartHeader.Id && cartEntity.CartDetails.Select(x => x.ProductId).Contains(x.ProductId));

            // Pega os itens da VO que existem no banco
            var existingItems = await _context.CartDetails.AsNoTracking().Where(x => existingItemsFromEntity.Select(x => x.Id).Contains(x.Id)).ToListAsync();

            // Pega os itens da VO que não existem no banco
            var notExistingItems = cartEntity.CartDetails.Where(x => existingItems.Select(x => x.Id).Contains(x.Id));

            // Insere os caras que não existem
            _context.CartDetails.AddRange(notExistingItems);

            await _context.SaveChangesAsync();

            // Atualiza os caras existentes
            _context.CartDetails.UpdateRange(existingItems.Select(x => { x.Count += 1; return x; }));

            await _context.SaveChangesAsync();
        }

        public Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCoupon(string userId)
        {
            throw new NotImplementedException();
        }
    }
}