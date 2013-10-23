// --------------------------------------------------------------------------
// <copyright file="MixParlayMonitorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.10.21">Created</change>
// </history>

namespace Fanex.ApolloParlayRMS.Tools.Controllers
{
    using System.Web.Mvc;
    using Fanex.ApolloParlayRMS.Core.Models;
    using Fanex.ApolloParlayRMS.Core.Web;
    using Fanex.ApolloParlayRMS.Core.Web.Controllers;
    using Fanex.ApolloParlayRMS.Tools.Services.Implements.MixParlayMonitor;
    using Fanex.ApolloParlayRMS.Tools.Services.Interfaces.MixParlayMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Mix parlay monitor controller.
    /// </summary>
    public class MixParlayMonitorController : BasedController
    {
        private readonly IMixParlayMonitorService _mixParlayMonitorService;

        public MixParlayMonitorController() :
            this(null)
        {
        }

        public MixParlayMonitorController(IMixParlayMonitorService mixParlayMonitorService)
        {
            this._mixParlayMonitorService = mixParlayMonitorService ?? new MixParlayMonitorService { ServiceContext = this.ServiceContext };
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = ApolloParlayRMSFunctionKeys.MixParlayMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.SportTypes = FilterForm.SportTypes.GetRange(1, 3);
            ViewBag.Combos = FilterForm.Combos;
            ViewBag.Levels = FilterForm.LevelList;
            ViewBag.SelectTop = FilterForm.SelectTops;
            ViewBag.RefreshTimes = FilterForm.RefreshTimes.GetRange(1, 3);

            return this.View();
        }
    }
}