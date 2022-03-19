namespace Cart.API.Data.ValueObjects
{
    public class CartVO
    {
        public CartHeaderVO CartHeader { get; set; }

        public IEnumerable<CartDetailVO> cardDetails { get; set; } = new List<CartDetailVO>();
    }
}