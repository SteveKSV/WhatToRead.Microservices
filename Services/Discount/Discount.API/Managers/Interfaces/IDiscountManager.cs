﻿using Discount.API.Entities;

namespace Discount.API.Managers.Interfaces
{
    public interface IDiscountManager
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
