using System.Text.RegularExpressions;
using GeekShop.web.Constants;

namespace GeekShop.web.Models
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }


        public string SubstringData(string data, int size)
        {
            return data?.Length < size ? data : data?.Substring(0, size - 3) + "...";
        }

        public string GetImage()
        {
            var pattern = new Regex(RegexPatternConstants.Http);

            return !string.IsNullOrEmpty(ImageUrl) && pattern.Match(ImageUrl).Success ?  ImageUrl : ImageConstants.NoDataFound ;
        }
    }
}