using Cart.API.Data.ValueObjects;
using Cart.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cart.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartRepository _repository;
        
        public CartController(ICartRepository cartRepository)
        {
            _repository = cartRepository;
        }

        [HttpGet("find-cart/{id}")]
        public async Task<ActionResult<CartVO>> FindCartByUserId(string userId)
        {
            var cart = await _repository.FindCartByUserId(userId);

            if(cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO cartVO)
        {
            var cart = await _repository.SaveOrUpdateCart(cartVO);

            if(cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVO)
        {
            var cart = await _repository.SaveOrUpdateCart(cartVO);

            if(cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(int cartId)
        {
            var status = await _repository.RemoveFromCart(cartId);

            if(!status)
                return BadRequest();

            return Ok(status);
        }

    }
}