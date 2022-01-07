﻿using GeekShopping.Email.Messages;

namespace GeekShopping.Email.Repository
{
    public interface IEmailRepository
    {
        Task SendAndLogEmail(UpdatePaymentResultMessage message);
        
    }
}
