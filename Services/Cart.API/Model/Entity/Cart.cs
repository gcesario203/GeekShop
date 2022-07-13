using Cart.API.Model.DataModel;

namespace Cart.API.Model.Entity
{
    public class Cart
    {
        public CartHeader CartHeader { get; set; }

        public IEnumerable<CartDetail> CartDetails { get; set; } = new List<CartDetail>();
    }
}