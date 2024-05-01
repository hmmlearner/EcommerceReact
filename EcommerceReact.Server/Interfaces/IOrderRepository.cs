using EcommerceReact.Server.DTO.Order;
using EcommerceReact.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceReact.Server.Interfaces
{
    public interface IOrderRepository
    {
        public Task<ServiceResponse<OrderRetrieveDto>> OrderConfirmation(int ordernumber, string sessionid);

        public Task<ServiceResponse<string>> SubmitPayment();
        public Task<ServiceResponse<OrderRetrieveDto>> RetrieveOrder(int ordernumber);
    }
}
