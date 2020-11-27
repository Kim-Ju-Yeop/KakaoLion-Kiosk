using System;

namespace KakaoLion.database.repository
{
    interface ProgramRepository
    {
        DateTime getOperationTime();

        void updateOperationTime(string operationTime);
    }
}
