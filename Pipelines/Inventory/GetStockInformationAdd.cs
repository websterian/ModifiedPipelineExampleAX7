// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStockInformation.cs" company="Sitecore Corporation">
//   Copyright (c) Sitecore Corporation 1999-2016
// </copyright>
// <summary>
//   Pipeline processor to get product stock information
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sitecore.Commerce.Connect.DynamicsRetail.Pipelines.Inventory
{
    using Sitecore.Commerce.Connect.CommerceServer.Inventory.Models;
    using Sitecore.Commerce.Pipelines;
    using Sitecore.Commerce.Services;
    using Sitecore.Commerce.Services.Inventory;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// PipelineProcessor for GetStockInformation
    /// </summary>
    public class GetStockInformationAdd : PipelineProcessor<ServicePipelineArgs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetStockInformationAdd"/> class.
        /// </summary>
        public GetStockInformationAdd()
        {
            this.InventoryCacheTimeout = 1; 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStockInformationAdd"/> class.
        /// </summary>       
        /// <param name="inventoryCacheTimeout">The inventory cache timeout.</param>
        public GetStockInformationAdd(string inventoryCacheTimeout)
        {
            double timeout = 1;
            if (!string.IsNullOrEmpty(inventoryCacheTimeout))
            {
                Double.TryParse(inventoryCacheTimeout, out timeout);
            }
            
            this.InventoryCacheTimeout = timeout;            
        }

        /// <summary>
        /// Gets or sets the inventory cache timeout.
        /// </summary>
        /// <value>
        /// The inventory cache timeout.
        /// </value>
        public double InventoryCacheTimeout { get; set; }

        /// <summary>
        /// Process the Pipeline event
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(ServicePipelineArgs args)
        {
            GetStockInformationRequest request;
            var result = args.Result as GetStockInformationResult;
            this.ValidateArguments(args, out request, out result);

            var stockInfos = new List<CommerceStockInformation>();
            foreach (var info in result.StockInformation)
            {
                var stockInfo = info as CommerceStockInformation;

                stockInfo.OnHandQuantity = stockInfo.OnHandQuantity + 1000000;
                stockInfo.Count = stockInfo.Count + 1000000;
                stockInfo.Status = Sitecore.Commerce.Entities.Inventory.StockStatus.InStock;

                //stockInfos.Add(stockInfo);
            }

            //result.StockInformation = stockInfos;
            result.Success = true;
        }

        private void ValidateArguments<TRequest, TResult>(ServicePipelineArgs args, out TRequest request, out TResult result)
            where TRequest : ServiceProviderRequest
            where TResult : ServiceProviderResult
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNull(args, "args");
            Sitecore.Diagnostics.Assert.ArgumentNotNull(args.Request, "args.Request");
            Sitecore.Diagnostics.Assert.ArgumentNotNull(args.Request.RequestContext, "args.Request.RequestContext");
            Sitecore.Diagnostics.Assert.ArgumentNotNull(args.Result, "args.Result");

            request = args.Request as TRequest;
            result = args.Result as TResult;
            Sitecore.Diagnostics.Assert.IsNotNull(request, "The parameter args.Request was not of the expected type.  Expected {0}.  Actual {1}.", typeof(TRequest).Name, args.Request.GetType().Name);
            Sitecore.Diagnostics.Assert.IsNotNull(result, "The parameter args.Result was not of the expected type.  Expected {0}.  Actual {1}.", typeof(TResult).Name, args.Result.GetType().Name);
        }
    }
}
