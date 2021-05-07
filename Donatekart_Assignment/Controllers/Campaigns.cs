using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Donatekart_Assignment.Controllers
{
    public class Campaigns : Controller
    {
        //HttpClientHandler _clientHandler = new HttpClientHandler();
        string _apiURL = "https://testapi.donatekart.com/api/campaign";
        List<CampaignDataResponse> _dCampaignOutput = new List<CampaignDataResponse>();
        List<CampaignData> apiResponseData = new List<CampaignData>();

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// This Method to get all the campaigns data from  https://testapi.donatekart.com/api/campaign Url
        /// </summary>
        /// <returns>All Campaigns Data</returns>
        [HttpGet]
        [Route("campaigns")]
        public async Task<List<CampaignDataResponse>> GetAllCampaigns()
        {
            apiResponseData = new List<CampaignData>();
            _dCampaignOutput = new List<CampaignDataResponse>();
            apiResponseData = await GetAPIResponse();
            List<CampaignData> SortedList = apiResponseData.OrderByDescending(o => o.totalAmount).ToList();
            foreach (var item in SortedList)
            {
                CampaignDataResponse cResponse = new CampaignDataResponse();
                cResponse.title = item.title;
                cResponse.totalAmount = item.totalAmount;
                cResponse.backersCount = item.backersCount;
                cResponse.endDate = item.endDate;
                _dCampaignOutput.Add(cResponse);

            }
            return _dCampaignOutput;
        }

        /// <summary>
        /// This Method to fetches active camagins from  https://testapi.donatekart.com/api/campaign Url
        /// </summary>
        /// <returns> Title, Total Amount, Backers Count and End date of Active Campagins</returns>
        [HttpGet]
        [Route("activecampaigns")]
        public async Task<List<CampaignData>> GetActiveCampaigns()
        {
            apiResponseData = new List<CampaignData>();
            List<CampaignData> campaignOutput = new List<CampaignData>();
            apiResponseData = await GetAPIResponse();

            foreach (var item in apiResponseData)
            {
                CampaignDataResponse cResponse = new CampaignDataResponse();
                if (item.endDate.Date >= DateTime.Now.Date && (DateTime.Now.Date - item.created.Date).TotalDays <= 30)
                {
                    campaignOutput.Add(item);
                }
            }          
            return campaignOutput;
        }

        /// <summary>
        /// This Method to fetches closed campagins from  https://testapi.donatekart.com/api/campaign Url
        /// </summary>
        /// <returns> Title, Total Amount, Backers Count and End date of Closed Campagins</returns>

        [HttpGet]
        [Route("closedcampaigns")]
        public async Task<List<CampaignData>> GetClosedCampaigns()
        {
            apiResponseData = new List<CampaignData>();
            List<CampaignData> campaignOutput = new List<CampaignData>();
            apiResponseData = await GetAPIResponse();

            foreach (var item in apiResponseData)
            {
                CampaignDataResponse cResponse = new CampaignDataResponse();
                if (item.endDate.Date < DateTime.Now.Date || item.procuredAmount == item.totalAmount)
                {
                    campaignOutput.Add(item);
                }
            }   
            return campaignOutput;
        }

        private async Task<List<CampaignData>> GetAPIResponse()
        {
            List<CampaignData> apiResponse = new List<CampaignData>();
            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(_apiURL).Result)
                {
                    try
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var campaignJsonString = await response.Content.ReadAsStringAsync();
                            var responseData = JsonConvert.DeserializeObject<List<CampaignData>>(campaignJsonString);
                            apiResponse = responseData.OrderByDescending(o => o.totalAmount).ToList();
                        }
                        else
                        {
                            throw new Exception($"API Call was not successful - returned status code as : {response.StatusCode} - Response string {response} - URL : {_apiURL} - {response.ToString()}");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"API Call was not successful - Exception = {ex.Message} - Exception Trace = {ex.StackTrace} - URL : {_apiURL} - {response.ToString()}");
                    }
                }
            }
            return apiResponse;
        }
    }
}
