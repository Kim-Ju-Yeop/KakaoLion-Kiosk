using KakaoLion.model;
using System.Collections.Generic;

namespace KakaoLion.database.repository
{
    interface OrderRepository
    {
        int getLastOrderCount();

        void insertOrder(int lastOrderCount);

        List<OrderModel> getAllOrder();

        List<string> getAllPurchaseAt();

        List<OrderModel> getTodayAllOrder(string date);

        List<OrderModel> getAllTimeOrder(string hour);

        List<OrderModel> getUserOrder(string userId);
    }
}
