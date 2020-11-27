using KakaoLion.dto.model;
using Prism.Mvvm;

namespace KakaoLion.model
{
    public class MenuModel : BindableBase
    {
        public int idx { get; set; }
        public int page { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public int price { get; set; }

        private int _discount;
        public int discount 
        {
            get { return _discount; }
            set
            {
                SetProperty(ref _discount, value);
            }
        }

        private bool _stock;
        public bool stock
        {
            get { return _stock; }
            set
            {
                SetProperty(ref _stock, value);
            }
        }

        public string imagePath { get; set; }
    }
}
