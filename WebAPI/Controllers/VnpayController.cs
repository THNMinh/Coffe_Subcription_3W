using Core.Interfaces.Repositories;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using VNPAY;
using VNPAY.Enums;
using VNPAY.Models;
using VNPAY.Utilities;

namespace Backend_API_Testing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _configuration;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IPaymentTransactionRepository _paymentTransactionRepository;

        public VnpayController(IVnpay vnPayservice, IConfiguration configuration, ISubscriptionPlanRepository subscriptionPlanRepository, IPaymentTransactionRepository paymentTransactionRepository)
        {
            _vnpay = vnPayservice;
            _configuration = configuration;

            _vnpay.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:CallbackUrl"]);

            _subscriptionPlanRepository = subscriptionPlanRepository;
            _paymentTransactionRepository = paymentTransactionRepository;
        }

        /// <summary>
        /// Tạo url thanh toán
        /// </summary>
        /// <param name="money">Số tiền phải thanh toán</param>
        /// <param name="description">Mô tả giao dịch</param>
        /// <returns></returns>
        [HttpGet("CreatePaymentUrl")]
        public async Task<ActionResult<string>> CreatePaymentUrlAsync(int planId, int userId) // Change userId type to int
        {
            try
            {
                var subPlan = await _subscriptionPlanRepository.GetByIdAsync(planId); // Kiểm tra xem gói đăng ký có tồn tại hay không
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch

                var request = new PaymentRequest
                {
                    PaymentId = DateTime.UtcNow.Ticks,
                    Money = (double)subPlan.Price,
                    Description = "Purchase plan",
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                    CreatedDate = DateTime.UtcNow, // Tùy chọn. Mặc định là thời điểm hiện tại
                    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                    Language = DisplayLanguage.Vietnamese // Tùy chọn. Mặc định là tiếng Việt
                };

                await _paymentTransactionRepository.CreateAsync(new PaymentTransaction
                {
                    UserId = userId, // Change userId type to int to match PaymentTransaction.UserId
                    Amount = subPlan.Price,
                    Currency = Currency.VND.ToString(),
                    OrderId = planId.ToString(),
                    TransactionNo = request.PaymentId.ToString(),
                    PaymentTime = DateTime.UtcNow,
                    TransactionStatus = null,
                    BankCode = BankCode.ANY.ToString(),
                    CardType = "VNPAY",
                    CreatedAt = DateTime.UtcNow,
                    IpAddress = ipAddress,
                });

                var paymentUrl = _vnpay.GetPaymentUrl(request);

                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Thực hiện hành động sau khi thanh toán. URL này cần được khai báo với VNPAY để API này hoạt đồng (ví dụ: http://localhost:1234/api/Vnpay/IpnAction)
        /// </summary>
        /// <returns></returns>
        [HttpGet("IpnAction")]
        public async Task<IActionResult> IpnAction()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = await _vnpay.GetPaymentResultAsync(Request.Query); // Await the Task to get the PaymentResult object
                    if (paymentResult.IsSuccess)
                    {
                        return Ok();
                    }

                    // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.
                    return BadRequest("Thanh toán thất bại");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }

        /// <summary>
        /// Trả kết quả thanh toán về cho người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet("Callback")]
        public async Task<ActionResult<PaymentResult>> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = await _vnpay.GetPaymentResultAsync(Request.Query); // Await the Task to get the PaymentResult object

                    if (paymentResult.IsSuccess)
                    {
                        return Ok(paymentResult);
                    }

                    return BadRequest(paymentResult);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}