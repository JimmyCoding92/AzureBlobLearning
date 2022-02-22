using AzureBlobLearning.Models;
using AzureBlobLearning.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AzureBlobLearning.Controllers
{
    public class CdnController : Controller
    {
        private readonly IAzureBlobService _azureBlobService;
        private readonly string _cdnEndpoint;

        public CdnController(IAzureBlobService azureBlobService, IConfiguration configuration)
        {
            _azureBlobService = azureBlobService;
            _cdnEndpoint = configuration.GetValue<string>("CdnEndpoint");
        }

        public async Task<IActionResult> Index()
        {
			try
			{
				var allBlobs = await _azureBlobService.ListAsync();
                List<Uri> cdnUris = new List<Uri>();

                foreach (var blob in allBlobs)
                {
                    var parts = blob.ToString().Split('/');
                    cdnUris.Add(new Uri(_cdnEndpoint + parts[parts.Length - 1]));
                }

				return View(cdnUris);
			}
			catch (Exception ex)
			{
				ViewData["message"] = ex.Message;
				ViewData["trace"] = ex.StackTrace;
				return View("Error");
			}
		}
    }
}
