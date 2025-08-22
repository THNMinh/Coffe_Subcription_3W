using AutoMapper;
using Core.DTOs.Response;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Service.Services;
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
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IMapper _mapper;

        private readonly IUserSubcriptionService _userSubscriptionService;


        public VnpayController(IVnpay vnPayservice, IConfiguration configuration, ISubscriptionPlanRepository subscriptionPlanRepository, IPaymentTransactionRepository paymentTransactionRepository,
            IPaymentTransactionService paymentTransactionService,
            IMapper mapper, IUserSubcriptionService userSubscriptionService)
        {
            _vnpay = vnPayservice;
            _configuration = configuration;

            _vnpay.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:CallbackUrl"]);

            _subscriptionPlanRepository = subscriptionPlanRepository;
            _paymentTransactionRepository = paymentTransactionRepository;
            _paymentTransactionService = paymentTransactionService;
            _mapper = mapper;
            _userSubscriptionService = userSubscriptionService;
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
                    var transaction = await _paymentTransactionService.GetByOrderIdAsync(paymentResult.PaymentId.ToString());
                    if (transaction == null)
                    {
                        return NotFound();
                    }
                    if (paymentResult.IsSuccess)
                    {
                        if (transaction != null)
                        {
                            transaction.TransactionNo = "success";
                            var success = await _paymentTransactionService.UpdateAsync(transaction);
                            var plan = await _subscriptionPlanRepository.GetByIdAsync(int.Parse(transaction.OrderId));
                            if (!success)
                            {
                                return NotFound(new ApiResponseDTO<object>
                                {
                                    Success = false,
                                    Message = "Update fail"
                                });
                            }
                            else {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                var userSub = new UserSubscription
                                {
                                    UserId = transaction.UserId,
                                    PlanId = int.Parse(transaction.OrderId),
                                     
                                    StartDate = DateTime.UtcNow,
                                    EndDate = DateTime.UtcNow.AddDays(30), // Assuming a plan with 30 days duration
                                    RemainingCups = plan.TotalCups, // Assuming a plan with 30 cups per month
                                    IsActive = true
                                };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                var createdSub = await _userSubscriptionService.CreateAsync(userSub);
                            }
                        }
                        return Ok(transaction);
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


         //[Authorize(Roles = "manager")]
        [HttpGet("getallpayment")]
         
        public async Task<IActionResult> GetCategories()
        {

            var categories = await _paymentTransactionService.GetAllPaymentTransactionPlansAsync();
             return Ok(categories);
        }
    }
}