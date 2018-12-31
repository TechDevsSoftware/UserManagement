﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechDevs.Clients;
using TechDevs.Shared.Models;
using TechDevs.MarketingPreferences;
using TechDevs.NotificationPreferences;

namespace TechDevs.Accounts.WebService.Controllers
{
    [Route("api/v1/customer/account")]
    public class CustomerDataController : Controller
    {
        private readonly IClientService _clientService;
        private readonly INotificationPreferencesService _notificationPreferences;
        private readonly IMarketingPreferencesService _marketingService;

        public CustomerDataController(IClientService clientService, IMarketingPreferencesService marketingService, INotificationPreferencesService notificationPreferences)
        {
            _clientService = clientService;
            _notificationPreferences = notificationPreferences;
            _marketingService = marketingService;
        }

        [HttpPost("preferences/marketing")]
        public async Task<IActionResult> UpdateMarketingPreferences([FromBody] MarketingNotificationPreferences marketingPreferences, [FromHeader(Name = "TechDevs-ClientKey")] string clientKey)
        {
            try
            {
                var client = await _clientService.GetClientByShortKey(clientKey);
                var userId = this.UserId();
                if (userId == null) return new UnauthorizedResult();

                var result = await _marketingService.UpdateMarketingPreferences(marketingPreferences, userId, client.Id);
                return new OkObjectResult(result);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Marketing preferences could not be updated");
            }
        }

        [HttpPost("preferences/notifications")]
        public async Task<IActionResult> UpdateNotificationPreferences([FromBody] CustomerNotificationPreferences notificationPreferences, [FromHeader(Name = "TechDevs-ClientKey")] string clientKey)
        {
            try
            {
                var client = await _clientService.GetClientByShortKey(clientKey);
                var userId = this.UserId();
                if (userId == null) return new UnauthorizedResult();

                var result = await _notificationPreferences.UpdateNotificationPreferences(notificationPreferences, userId, client.Id);
                return new OkObjectResult(result);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Notification preferences could not be updated");
            }
        }
    }
}