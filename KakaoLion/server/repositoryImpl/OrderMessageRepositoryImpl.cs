using KakaoLion.model;
using KakaoLion.pages;
using KakaoLion.server.repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

namespace KakaoLion.server.repositoryImpl
{
    class OrderMessageRepositoryImpl : OrderMessageRepository
    {
        private JArray jarray;

        public OrderMessageRepositoryImpl()
        {
            jarray = new JArray();
        }

        public void sendOrderMessage(int lastOrderCount)
        {
            for (int i = 0; i < OrderPage.orderList.Count; i++)
            {
                JObject jsonData = new JObject();
                string menuName = MainWindow.menuList.Where(menu => menu.idx == OrderPage.orderList[i].menuIdx).ToList()[0].name;

                jsonData.Add("Name", menuName);
                jsonData.Add("Count", OrderPage.orderList[i].quantity);
                jsonData.Add("Price", OrderPage.orderList[i].totalPrice);

                jarray.Add(jsonData);
            }

            string orderCount;
            if (lastOrderCount + 1 < 10) orderCount = "00" + (lastOrderCount + 1);
            else if (lastOrderCount + 1 < 100) orderCount = "0" + (lastOrderCount + 1);
            else orderCount = "" + (lastOrderCount + 1);

            JObject json = new JObject();
            json.Add("MSGType", (int)Message.ORDER);
            json.Add("Id", OrderPage.orderList[0].userId);
            json.Add("Content", "");
            json.Add("ShopName", "카카오프렌즈");
            json.Add("OrderNumber", orderCount);
            json.Add("Group", true);
            json.Add("Menus", jarray);

            byte[] buffer = new byte[4096];
            string message = JsonConvert.SerializeObject(json);
            buffer = Encoding.UTF8.GetBytes(message);

            App.stream.Write(buffer, 0, buffer.Length);
        }
    }
}
