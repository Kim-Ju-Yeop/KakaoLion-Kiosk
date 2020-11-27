namespace KakaoLion.server.repository
{
    interface OrderMessageRepository
    {
        void sendOrderMessage(int lastOrderCount);
    }
}
