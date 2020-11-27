using KakaoLion.model;
using System.Collections.Generic;

namespace KakaoLion.database.repository
{
    interface StoreRepository
    {
        List<StoreModel> getAllStore();

        void updateStoreLastOrder(int shopIdx);

        void updateStorePossible(int shopIdx);
    }
}
