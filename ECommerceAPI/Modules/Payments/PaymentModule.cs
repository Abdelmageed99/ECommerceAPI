using Braintree;
using ECommerceAPI.Modules.Payments.Repositories;
using ECommerceAPI.Modules.Payments.Services;

namespace ECommerceAPI.Modules.Payments
{
    public static class PaymentModule  
    {
        public static void AddPaymentModule(this IServiceCollection services)
        {
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            //services.AddScoped<IBraintreeGateway>();

        }
    }
}
