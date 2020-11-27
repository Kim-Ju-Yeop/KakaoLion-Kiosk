using KakaoLion.dto.model;

namespace KakaoLion.model
{
    public class StatsModel
    {
        public Category category { get; set; }
        public int quantity { get; set; }
        public int totalPrice { get; set; }
        public int salePrice { get; set; }
    }
}
