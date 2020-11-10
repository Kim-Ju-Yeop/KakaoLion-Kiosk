using KakaoLion.dto.model;

namespace KakaoLion.model
{
    public class MenuModel
    {
        public int idx { get; set; }
        public int page { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public int price { get; set; }
        public int discount { get; set; }
        public string imagePath { get; set; }
    }
}
