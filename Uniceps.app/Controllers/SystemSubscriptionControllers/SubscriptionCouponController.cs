using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Controllers.SystemSubscriptionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SubscriptionCouponController : ControllerBase
    {
        private readonly ICouponDataService _couponDataService;

        public SubscriptionCouponController(ICouponDataService couponDataService)
        {
            _couponDataService = couponDataService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {

            IEnumerable<SubscriptionCoupon> coupons = await _couponDataService.GetAll();
            return Ok(coupons.Select(x => new CouponResponse(x)).ToList());
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            SubscriptionCoupon coupon = await _couponDataService.Get(id);
            return Ok(new CouponResponse(coupon));
        }
        [HttpPost]
        public async Task<IActionResult> Create(CouponRequest couponRequest)
        {
            if (couponRequest == null)
                return BadRequest("coupon data is missing.");

            SubscriptionCoupon coupon = couponRequest.ToModel();
            SubscriptionCoupon result = await _couponDataService.Create(coupon);
            return Ok(new CouponResponse(result));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CouponRequest couponRequest)
        {
            SubscriptionCoupon coupon = couponRequest.ToModel();
            coupon.Id = id;
            var result = await _couponDataService.Update(coupon);
            return Ok("Updated successfully");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _couponDataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
