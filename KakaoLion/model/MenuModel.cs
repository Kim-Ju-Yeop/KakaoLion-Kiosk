using System;

namespace KakaoLion.model
{
    public class MenuModel
    {
        public int idx { get; set; }
        public int page { get; set; }
        public Category category { get; set; }
        public String name{ get; set; }
        public int price { get; set; }
        public int discount { get; set; }
        public String imagePath { get; set; }
        public enum Category
        {
            Small,
            Medium,
            Big,
        }
    }
}
