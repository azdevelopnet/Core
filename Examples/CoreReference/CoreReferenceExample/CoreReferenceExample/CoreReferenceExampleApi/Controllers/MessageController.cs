//using System;
//using System.Net;
//using CoreReferenceExampleApi.Data;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace CoreReferenceExampleApi.Controllers
//{
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//    public class MessageController: Controller
//    {
//        private readonly IMessageService _messageService;

//        public MessageController(IMessageService messageService)
//        {
//            _messageService = messageService;
//        }
//    }
//}
