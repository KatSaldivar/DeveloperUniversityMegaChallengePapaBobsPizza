using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PapaBobs.Domain
{
    public class OrderManager
    {
        public static void CreateOrder(DTO.OrderDTO orderDTO)
        {
            if (orderDTO.Name.Trim().Length == 0)
                throw new Exception("Name is required.");
            if (orderDTO.Address.Trim().Length == 0)
                throw new Exception("Address is required.");
            if (orderDTO.Zip.Trim().Length == 0)
                throw new Exception("Zip code is required.");
            if (orderDTO.Phone.Trim().Length == 0)
                throw new Exception("Phone is required.");

            orderDTO.OrderId = Guid.NewGuid();
            orderDTO.TotalCost = PizzaPriceManager.CalculateCost(orderDTO);
            Persistence.OrderRepository.CreateOrder(orderDTO);
        }

        public static void CompleteOrder(Guid orderId)
        {
            Persistence.OrderRepository.CompleteOrder(orderId);
        }

        public static object GetOrders()
        {
            return Persistence.OrderRepository.GetOrders();
        }
    }
}
