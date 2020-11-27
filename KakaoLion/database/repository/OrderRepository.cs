using KakaoLion.model;
using System.Collections.Generic;

namespace KakaoLion.database.repository
{
    interface OrderRepository
    {
        int getLastOrderCount();

        void insertOrder(int lastOrderCount);
    }
}
