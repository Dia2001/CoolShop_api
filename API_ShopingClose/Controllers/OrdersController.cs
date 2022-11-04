﻿using API_ShopingClose.Common;
using API_ShopingClose.Entities;
using API_ShopingClose.Models;
using API_ShopingClose.Service;
using Microsoft.AspNetCore.Mvc;

namespace API_ShopingClose.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class OrdersController : AuthController
    {
        private readonly IConfiguration _config;
        OrderDeptService _orderService;
        OrderDetailDeptService _orderDetailService;
        ProductDeptService _productservice;

        public OrdersController(IConfiguration config, ILogger<UsersController> logger,
            OrderDeptService orderService, OrderDetailDeptService orderDetailService, ProductDeptService productservice) : base(logger)
        {
            _config = config;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _productservice = productservice;
        }

        [HttpGet]
        [Route("orders")]
        public async Task<IActionResult> getAllOrder()
        {
            try
            {

                List<OrderModel> orderModes = new List<OrderModel>();
                List<Order> orders = (await _orderService.getAllOrder()).ToList();
                foreach (var ordertemp in orders)
                {
                    OrderModel orderdata = ConvertMethod.convertOrderToOrderModel(ordertemp);
                    List<OrderDetails> listOrdetail = (await _orderDetailService.getAllOrderDetailByOrderId(Guid.Parse(orderdata.orderId.ToString()))).ToList();
                    OrderDetailsModel[] orderDetailArr = new OrderDetailsModel[listOrdetail.Count()];
                    int count = 0;
                    foreach (var orderDetailTmp in listOrdetail)
                    {
                        OrderDetailsModel orderDetailModel = ConvertMethod.convertOrderDetailsToOrderDetailModel(orderDetailTmp);
                        orderDetailArr[count] = orderDetailModel;
                        count++;
                    }
                    orderdata.orderDetail = orderDetailArr;
                    orderModes.Add(orderdata);
                }
                return StatusCode(StatusCodes.Status200OK, orderModes);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dynamic response = new
                {
                    status = 500,
                    message = "Call servser faile!",
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("orders/user")]
        public async Task<IActionResult> getAllOrderToUser()
        {
            try
            {
                List<OrderModel> orderModes = new List<OrderModel>();
                List<Order> orders = (await _orderService.getAllOrderToUser(Guid.Parse(GetUserId().ToString()))).ToList();
                foreach (var ordertemp in orders)
                {
                    OrderModel orderdata = ConvertMethod.convertOrderToOrderModel(ordertemp);
                    List<OrderDetails> listOrdetail = (await _orderDetailService.getAllOrderDetailByOrderId(Guid.Parse(orderdata.orderId.ToString()))).ToList();
                    OrderDetailsModel[] orderDetailArr = new OrderDetailsModel[listOrdetail.Count()];
                    int count = 0;
                    foreach (var orderDetailTmp in listOrdetail)
                    {
                        OrderDetailsModel orderDetailModel = ConvertMethod.convertOrderDetailsToOrderDetailModel(orderDetailTmp);
                        orderDetailArr[count] = orderDetailModel;
                        count++;
                    }
                    orderdata.orderDetail = orderDetailArr;
                    orderModes.Add(orderdata);
                }
                return StatusCode(StatusCodes.Status200OK, orderModes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dynamic response = new
                {
                    status = 500,
                    message = "Call servser faile!",
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        [Route("orders/{orderId}")]
        public async Task<IActionResult> getAllOrderByIdUser([FromRoute] Guid orderId)
        {
            try
            {
                Order order = await _orderService.getOneOrderByUserIdAndOrderId(Guid.Parse(GetUserId().ToString()), orderId);
                OrderModel orderdata = ConvertMethod.convertOrderToOrderModel(order);
                Console.WriteLine(Guid.Parse(orderdata.orderId.ToString()));
                List<OrderDetails> listOrdetail = (await _orderDetailService.getAllOrderDetailByOrderId(Guid.Parse(orderdata.orderId.ToString()))).ToList();
                OrderDetailsModel[] orderDetailArr = new OrderDetailsModel[listOrdetail.Count()];
                int count = 0;
                foreach (var orderDetailTmp in listOrdetail)
                {
                    OrderDetailsModel orderDetailModel = ConvertMethod.convertOrderDetailsToOrderDetailModel(orderDetailTmp);
                    orderDetailArr[count] = orderDetailModel;
                    count++;
                }
                orderdata.orderDetail = orderDetailArr;
                return StatusCode(StatusCodes.Status200OK, orderdata);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                dynamic response = new
                {
                    status = 500,
                    message = "Call servser faile!",
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("orders")]
        public async Task<IActionResult> createOrder([FromBody] OrderModel orderModel)
        {
            dynamic response = new
            {
                status = 500,
                message = "Call servser faile!",
            };

            try
            {
                orderModel.orderStatusId = OrderContants.CREATED;
                Order order = ConvertMethod.convertOrderModelToOrder(orderModel);

                order.CreateDate = DateTime.Now;
                order.UpdateDate = DateTime.Now;

                order.UserID = Guid.Parse(GetUserId().ToString());

                var orderId = await _orderService.InsertOrder(order);
                if (orderId != null)
                {
                    foreach (var orderDetailTmp in orderModel.orderDetail)
                    {
                        OrderDetails orderDetail = new OrderDetails();
                        orderDetail.ProductID = orderDetailTmp.productId;
                        orderDetail.SizeID = orderDetailTmp.sizeId;
                        orderDetail.ColorID = orderDetailTmp.colorId;
                        orderDetail.Qunatity = orderDetailTmp.quantity;
                        orderDetail.Promotion = orderDetailTmp.promotion;
                        orderDetail.Price = orderDetailTmp.price;
                        orderDetail.OrderID = Guid.Parse(orderId.ToString());
                        await _orderDetailService.InsertOrderDetail(orderDetail);
                    }
                }
                response = new
                {
                    status = 201,
                    message = "Tạo sản phẩm thành công!",
                    data = orderId
                };
                return StatusCode(StatusCodes.Status201Created, response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, response);
        }
    }
}
