using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KakaoLion.model
{
    class OrderModel
    {

        public int idx { get; set; } //주문 목록 인덱스
        public int menuindx { get; set; } //메뉴 인덱스 참조
        public int quantity { get; set; } // 수량
        public int totalPrice { get; set; } //총가격
        public String userId { get; set; } //userId
        public String purchaseAt { get; set; } //구매 시간
        public Boolean paymentPlace { get; set; } // 구매 장소
        public Boolean paymentMethod { get; set; } //구매 방법
        public int shopIdx { get; set; } // 지점 선택시 

    }
}
