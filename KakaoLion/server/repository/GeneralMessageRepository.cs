using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KakaoLion.server.repository
{
    interface GeneralMessageRepository
    {
        void sendGeneralMessage(string userId, string content, bool isGroup);
    }
}
