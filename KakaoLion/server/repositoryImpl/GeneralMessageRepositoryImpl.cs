using KakaoLion.model;
using KakaoLion.server.repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace KakaoLion.server.repositoryImpl
{
    class GeneralMessageRepositoryImpl : GeneralMessageRepository
    {
        private JObject json;

        public void sendGeneralMessage(string userId, string content, bool isGroup)
        {
            json = new JObject();
            json.Add("MSGType", (int)Message.GENERAL);
            json.Add("Id", userId);
            json.Add("Content", content);
            json.Add("ShopName", "");
            json.Add("OrderNumber", "");
            json.Add("Group", isGroup);
            json.Add("Menus", "");

            byte[] buffer = new byte[4096];
            string message = JsonConvert.SerializeObject(json);
            buffer = Encoding.UTF8.GetBytes(message);

            App.stream.Write(buffer, 0, buffer.Length);
        }
    }
}
