using KakaoLion.model;
using System.Collections.Generic;

namespace KakaoLion.database.repository
{
    interface UserRepository
    {
        List<UserModel> getAllUser();
    }
}
