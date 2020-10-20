using System;

namespace KakaoLion.model
{
    public class OrderModel
    {

        public int? idx { get; set; } 
        public int menuindx { get; set; } 
        public int quantity { get; set; }
        public int totalPrice { get; set; } 
        public String userId { get; set; } 
        public String purchaseAt { get; set; } 
        public Boolean paymentPlace { get; set; } 
        public Boolean paymentMethod { get; set; } 
        public int shopIdx { get; set; } 

    }
}
