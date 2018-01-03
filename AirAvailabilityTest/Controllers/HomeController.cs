using AirAvailabilityTest.AirService;
using AirAvailabilityTest.Models;
using AirAvailabilityTest.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AirAvailabilityTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public static string MY_APP_NAME = "UAPI";
        private string origin = "COK";
        private string destination = "SIN";

        #region Air Availability
        public JsonResult Availability(SearchModel searchModel)
        {
            origin = searchModel.Origin;
            destination = searchModel.Destination;

            AvailabilitySearchReq request = new AvailabilitySearchReq();
            AvailabilitySearchRsp rsp;

            request = SetupRequestForSearch(request, searchModel);

            AirAvailabilitySearchPortTypeClient client = new AirAvailabilitySearchPortTypeClient("AirAvailabilitySearchPort", WsdlService.AIR_ENDPOINT);
            client.ClientCredentials.UserName.UserName = Helper.RetrunUsername();
            client.ClientCredentials.UserName.Password = Helper.ReturnPassword();
            try
            {
                var httpHeaders = Helper.ReturnHttpHeader();
                client.Endpoint.EndpointBehaviors.Add(new HttpHeadersEndpointBehavior(httpHeaders));

                rsp = client.service(null, request);
                AirAvailabilityResponseMain response = ConverToModel(rsp);
                //Console.WriteLine(rsp.AirItinerarySolution.Count());
                //Console.WriteLine(rsp.AirSegmentList.Count());

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception se)
            {
                //Console.WriteLine("Error : " + se.Message);
                client.Abort();
                throw se;
            }

            //these checks are just sanity that we can make an availability request
            //assertThat(rsp.getAirItinerarySolution().size(), is(not(0)));
            //assertThat(rsp.getAirSegmentList().getAirSegment().size(), is(not(0)));
        }
        public AirAvailabilityResponseMain ConverToModel(AvailabilitySearchRsp rsp)
        {
            var bLoop = false;
            var iSegmentCount = 0;
            AirAvailabilityResponseMain respMain = new AirAvailabilityResponseMain();
            respMain.AirAvailabilityResponse = new System.Collections.Generic.List<AirAvailabilityResponse>();
            AirAvailabilityResponse response = new AirAvailabilityResponse();
            response.AirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
            AirAvailabilityResult result = new AirAvailabilityResult();
            System.Collections.Generic.List<AirAvailabilityResponse> liAirAvailabilityResponse = new System.Collections.Generic.List<AirAvailabilityResponse>();
            System.Collections.Generic.List<AirAvailabilityResult> liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
            if (rsp != null)
            {
                var j = 0;
                for (var i = 0; i < rsp.AirItinerarySolution[0].Connection.Length; i++)
                {
                    if (i == 0)
                    {
                        while (j < rsp.AirItinerarySolution[0].Connection[i].SegmentIndex)
                        {
                            liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                            liAirAvailabilityResponse = new System.Collections.Generic.List<AirAvailabilityResponse>();
                            response = new AirAvailabilityResponse();
                            response.AirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                            liAirAvailabilityResult.Add(new AirAvailabilityResult()
                            {
                                From = rsp.FlightDetailsList[j].Origin,
                                To = rsp.FlightDetailsList[j].Destination
                            });
                            response.AirAvailabilityResult.AddRange(liAirAvailabilityResult);
                            respMain.AirAvailabilityResponse.Add(response);
                            j++;
                        }
                        j = rsp.AirItinerarySolution[0].Connection[i].SegmentIndex;
                    }
                    else if (i < rsp.AirItinerarySolution[0].Connection.Length - 1)
                    {
                        response = new AirAvailabilityResponse();
                        response.AirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                        liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                        liAirAvailabilityResponse = new System.Collections.Generic.List<AirAvailabilityResponse>();
                        while (j < rsp.AirItinerarySolution[0].Connection[i].SegmentIndex)
                        {
                            liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                            liAirAvailabilityResult.Add(new AirAvailabilityResult()
                            {
                                From = rsp.FlightDetailsList[j].Origin,
                                To = rsp.FlightDetailsList[j].Destination
                            });
                            response.AirAvailabilityResult.AddRange(liAirAvailabilityResult);
                            iSegmentCount = rsp.AirItinerarySolution[0].Connection[i].SegmentIndex;
                            j++;
                        }
                        respMain.AirAvailabilityResponse.Add(response);
                        j = rsp.AirItinerarySolution[0].Connection[i].SegmentIndex;
                    }
                    else if (rsp.AirItinerarySolution[0].Connection.Length - 1 == i)
                    {
                        response = new AirAvailabilityResponse();
                        response.AirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                        liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                        liAirAvailabilityResponse = new System.Collections.Generic.List<AirAvailabilityResponse>();
                        j = rsp.AirItinerarySolution[0].Connection[i].SegmentIndex;
                        while (j < rsp.AirSegmentList.Length)
                        {
                            liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                            liAirAvailabilityResult.Add(new AirAvailabilityResult()
                            {
                                From = rsp.FlightDetailsList[j].Origin,
                                To = rsp.FlightDetailsList[j].Destination
                            });
                            response.AirAvailabilityResult.AddRange(liAirAvailabilityResult);
                            iSegmentCount = rsp.AirItinerarySolution[0].Connection[i].SegmentIndex;
                            j++;
                        }
                        respMain.AirAvailabilityResponse.Add(response);
                    }
                }
            }
            return respMain;

        }
        private AvailabilitySearchReq SetupRequestForSearch(AvailabilitySearchReq request, SearchModel searchModel)
        {
            // TODO Auto-generated method stub

            //add in the tport branch code
            request.TargetBranch = CommonUtility.GetConfigValue(ProjectConstants.G_TARGET_BRANCH);

            //set the GDS via a search modifier
            String[] gds = new String[] { "1G" };
            AirSearchModifiers modifiers = AirReq.CreateModifiersWithProviders(gds);

            AirReq.AddPointOfSale(request, MY_APP_NAME);

            //try to limit the size of the return... not supported by 1G!
            modifiers.MaxSolutions = string.Format("25");
            request.AirSearchModifiers = modifiers;

            //travel is for denver to san fransisco 2 months from now, one week trip
            SearchAirLeg outbound = AirReq.CreateSearchLeg(origin, destination);
            AirReq.AddSearchDepartureDate(outbound, searchModel.FromDate);
            AirReq.AddSearchEconomyPreferred(outbound);

            ////coming back
            //SearchAirLeg ret = AirReq.CreateSearchLeg(destination, origin);
            //AirReq.AddSearchDepartureDate(ret, Helper.daysInFuture(70));
            ////put traveller in econ
            //AirReq.AddSearchEconomyPreferred(ret);


            request.Items = new SearchAirLeg[2];
            request.Items.SetValue(outbound, 0);
            //request.Items.SetValue(ret, 1);

            return request;
        }
        #endregion Air Availability

        #region LowFare Search
        public JsonResult LowFareShop(SearchModel searchModel)
        {
            bool solutionResult = true;
            LowFareSearchReq lowFareSearchReq = new LowFareSearchReq();
            LowFareSearchRsp lowFareSearchRsp;
            lowFareSearchReq = SetUpLFSSearch(lowFareSearchReq, solutionResult, searchModel);
            AirLowFareSearchPortTypeClient client = new AirLowFareSearchPortTypeClient("AirLowFareSearchPort", WsdlService.AIR_ENDPOINT);
            client.ClientCredentials.UserName.UserName = Helper.RetrunUsername();
            client.ClientCredentials.UserName.Password = Helper.ReturnPassword();
            try
            {
                var httpHeaders = Helper.ReturnHttpHeader();
                client.Endpoint.EndpointBehaviors.Add(new HttpHeadersEndpointBehavior(httpHeaders));
                lowFareSearchRsp = client.service(null, lowFareSearchReq);
                return Json(ConverToModelForLFS(lowFareSearchRsp), JsonRequestBehavior.AllowGet);
            }
            catch (Exception se)
            {
                Console.WriteLine("Error : " + se.Message);
                client.Abort();
                return null;
            }
        }

        private LowFareSearchReq SetUpLFSSearch(LowFareSearchReq lowFareSearchReq, bool solutionResult, SearchModel searchModel)
        {
            lowFareSearchReq.TargetBranch = CommonUtility.GetConfigValue(ProjectConstants.G_TARGET_BRANCH);
            lowFareSearchReq.SolutionResult = solutionResult;  //Change it to true if you want AirPricingSolution, by default it is false
                                                               //and will send AirPricePoint in the result

            //set the GDS via a search modifier
            string[] gds = new string[] { "1G" };
            AirSearchModifiers modifiers = AirReq.CreateModifiersWithProviders(gds);

            AirReq.AddPointOfSale(lowFareSearchReq, MY_APP_NAME);

            //try to limit the size of the return... not supported by 1G!
            modifiers.MaxSolutions = string.Format("25");
            lowFareSearchReq.AirSearchModifiers = modifiers;


            //travel is for denver to san fransisco 2 months from now, one week trip
            SearchAirLeg outbound = AirReq.CreateSearchLeg(searchModel.Origin, searchModel.Destination);
            AirReq.AddSearchDepartureDate(outbound, searchModel.FromDate);
            AirReq.AddSearchEconomyPreferred(outbound);

            //coming back
            /*SearchAirLeg ret = AirReq.CreateSearchLeg(destination, origin);
            AirReq.AddSearchDepartureDate(ret, Helper.daysInFuture(65));
            //put traveller in econ
            AirReq.AddSearchEconomyPreferred(ret);*/

            lowFareSearchReq.Items = new SearchAirLeg[1];
            lowFareSearchReq.Items.SetValue(outbound, 0);
            //lowFareSearchReq.Items.SetValue(ret, 1);

            //AirPricingModifiers priceModifiers = AirReq.AddAirPriceModifiers(typeAdjustmentType.Amount, +40);

            //lowFareSearchReq.AirPricingModifiers = priceModifiers;

            lowFareSearchReq.SearchPassenger = AirReq.AddSearchPassenger();

            return lowFareSearchReq;
        }

        public AirAvailabilityResponseMain ConverToModelForLFS(LowFareSearchRsp rsp)
        {
            AirAvailabilityResponseMain respMain = new AirAvailabilityResponseMain();
            respMain.AirAvailabilityResponse = new System.Collections.Generic.List<AirAvailabilityResponse>();
            AirAvailabilityResponse response = new AirAvailabilityResponse();
            response.AirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
            AirAvailabilityResult result = new AirAvailabilityResult();
            System.Collections.Generic.List<AirAvailabilityResponse> liAirAvailabilityResponse = new System.Collections.Generic.List<AirAvailabilityResponse>();
            System.Collections.Generic.List<AirAvailabilityResult> liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
            if (rsp != null)
            {
                for (var i = 0; i < rsp.Items.Length; i++)
                {
                    response = new AirAvailabilityResponse();
                    response.AirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                    liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                    liAirAvailabilityResponse = new System.Collections.Generic.List<AirAvailabilityResponse>();


                    for (var j = 0; j < ((AirPricingSolution)rsp.Items[i]).Journey[0].AirSegmentRef.Length; j++)
                    {

                        var data = (from a in rsp.AirSegmentList
                                    where a.Key == ((AirPricingSolution)rsp.Items[i]).Journey[0].AirSegmentRef[j].Key
                                    select new
                                    {
                                        a.Destination,
                                        a.ArrivalTime,
                                        a.Origin,
                                        a.DepartureTime,
                                        a.ProviderCode,
                                        ((AirPricingSolution)rsp.Items[i]).TotalPrice,
                                        ((AirPricingSolution)rsp.Items[i]).Taxes,
                                        a.Carrier
                                    }).FirstOrDefault();
                        liAirAvailabilityResult = new System.Collections.Generic.List<AirAvailabilityResult>();
                        liAirAvailabilityResult.Add(new AirAvailabilityResult()
                        {
                            From = data.Origin,
                            DepartureTime = Convert.ToDateTime(data.DepartureTime).ToString("dd-MMM-yyyy hh:mm tt"),
                            To = data.Destination,
                            ArrivalTime = Convert.ToDateTime(data.ArrivalTime).ToString("dd-MMM-yyyy hh:mm tt"),
                            Carrier = data.Carrier
                        });

                        response.AirAvailabilityResult.AddRange(liAirAvailabilityResult);
                        response.Amount = ((AirPricingSolution)rsp.Items[i]).TotalPrice;
                        response.TaxAmount = ((AirPricingSolution)rsp.Items[i]).Taxes;
                    }
                    respMain.AirAvailabilityResponse.Add(response);
                }

            }
            return respMain;

        }
        #endregion

        #region Air Price
        public JsonResult GetAirPrice(List<typeBaseAirSegment> pricingSegments)
        {
            AirPriceReq priceReq = new AirPriceReq();
            AirPriceRsp priceRsp;

            AirReq.AddPointOfSale(priceReq, "UAPI");

            AirItinerary itinerary = new AirItinerary();

            List<typeBaseAirSegment> itinerarySegments = new List<typeBaseAirSegment>();

            IEnumerator airSegments = pricingSegments.GetEnumerator();
            while (airSegments.MoveNext())
            {
                typeBaseAirSegment seg = (typeBaseAirSegment)airSegments.Current;
                seg.ProviderCode = "1G";
                seg.FlightDetailsRef = null;
                seg.ClassOfService = "Y";
                itinerarySegments.Add(seg);
            }

            itinerary.AirSegment = itinerarySegments.ToArray();

            priceReq.AirItinerary = itinerary;

            priceReq.SearchPassenger = AirReq.AddSearchPassenger();

            priceReq.AirPricingModifiers = new AirPricingModifiers()
            {
                PlatingCarrier = priceReq.AirItinerary.AirSegment[0].Carrier
            };

            List<AirPricingCommand> pricingCommands = new List<AirPricingCommand>();

            AirPricingCommand command = new AirPricingCommand()
            {
                CabinClass = "Economy"//You can use Economy,PremiumEconomy,Business etc.
            };

            pricingCommands.Add(command);

            priceReq.AirPricingCommand = pricingCommands.ToArray();


            priceReq.TargetBranch = CommonUtility.GetConfigValue(ProjectConstants.G_TARGET_BRANCH);

            AirPricePortTypeClient client = new AirPricePortTypeClient("AirPricePort", WsdlService.AIR_ENDPOINT);

            client.ClientCredentials.UserName.UserName = Helper.RetrunUsername();
            client.ClientCredentials.UserName.Password = Helper.ReturnPassword();
            try
            {
                var httpHeaders = Helper.ReturnHttpHeader();
                client.Endpoint.EndpointBehaviors.Add(new HttpHeadersEndpointBehavior(httpHeaders));

                priceRsp = client.service(null, priceReq);

                return Json(priceRsp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception se)
            {
                Console.WriteLine("Error : " + se.Message);
                client.Abort();
                return null;
            }

        }

        #endregion

        
    }
}