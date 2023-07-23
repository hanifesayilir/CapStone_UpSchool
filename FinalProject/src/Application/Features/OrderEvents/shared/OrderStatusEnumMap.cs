using Domain.Enums;

namespace Application.Features.OrderEvents.shared
{
    public class OrderStatusEnumMap
    {

        public static string MapToEnum(OrderStatus value)
        {

            string newValue;
            switch (value)
            {
                case OrderStatus.BotStarted: newValue = "Bot Started"; break;
                case OrderStatus.CrawlingStarted: newValue = "Crawling Started"; break;
                case OrderStatus.CrawlingCompleted: newValue = "Crawling Completed"; break;
                case OrderStatus.CrawlingFailed: newValue = "Crawling Failed"; break;
                case OrderStatus.OrderCompleted: newValue = "Order Completed"; break;
                default:
                    newValue = "None";
                    break;
            }

            return newValue;
        }

    }
}
