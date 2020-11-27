using KakaoLion.model;
using KakaoLion.server.repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace KakaoLion.server.repositoryImpl
{
    class LoginRepositoryImpl : LoginRepository
    {
        private JObject json;

        public LoginRepositoryImpl()
        {
            json = new JObject();
        }

        public void sendLoginMessage(string userId)
        {
            json.Add("MSGType", (int)Message.LOGIN);
            json.Add("Id", userId);
            json.Add("Content", "");
            json.Add("ShopName", "");
            json.Add("OrderNumber", "");
            json.Add("Group", false);
            json.Add("Menus", "");

            byte[] buffer = new byte[4096];
            string message = JsonConvert.SerializeObject(json);
            buffer = Encoding.UTF8.GetBytes(message);

            App.stream.Write(buffer, 0, buffer.Length);
        }
    }
}
