using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName{request.ProductName}"));
            }
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _repository.CreateDiscount(coupon);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;

        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _repository.UpdateDiscount(coupon);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);
            return new DeleteDiscountResponse { Success = deleted };
        }
    }
}
