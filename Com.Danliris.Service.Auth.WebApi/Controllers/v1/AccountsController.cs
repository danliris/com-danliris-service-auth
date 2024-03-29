﻿using AutoMapper;
using Com.Danliris.Service.Auth.Lib.BusinessLogic.Interfaces;
using Com.Danliris.Service.Auth.Lib.Models;
using Com.Danliris.Service.Auth.Lib.Services.IdentityService;
using Com.Danliris.Service.Auth.Lib.Services.ValidateService;
using Com.Danliris.Service.Auth.Lib.ViewModels;
using Com.Danliris.Service.Auth.WebApi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Auth.WebApi.Controllers.v1
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/accounts")]
    [Authorize]
    public class AccountsController : BaseController<Account, AccountViewModel, IAccountService>
    {
        public static readonly string ApiVersion = "1.0.0";
        public readonly IAccountService _accountService;

        public AccountsController(IIdentityService identityService, IValidateService validateService, IAccountService service, IMapper mapper, IAccountService _accountService) : base(identityService, validateService, service, mapper, "1.0.0")
        {
            this._accountService = _accountService;
        }

        [HttpPut("changePass")]
        public async Task<IActionResult> UpdatePass([FromBody] AccoutChangePassViewModel data)
        {
            try
            {
                VerifyUser();

                await _accountService.UpdatePass(data.username, data.password);

                return NoContent();
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
