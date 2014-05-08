using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ForecastIO;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace WeatherAPI.Controllers
{
	public class ValuesController : ApiController
	{

		public class Request {
			public float latitude { get; set; }
			public float longitude { get; set; }
		}

		public class Response {
			public double Temperature { get; set; }
			public List<String> Services { get; set; }
			public string Message { get; set; }
		}

		// GET api/values/5
		public String Get([FromUri] Request req)
		{
			if(req.latitude>0 && req.longitude>0)
			{
				return getWeatherForecast(req.latitude, req.longitude);				
			}
			else
			{
				return "Invalid Request";
			}
		}

		public string getWeatherForecast(float latitude, float longitude)
		{
			var request = new ForecastIORequest("abc4fd99d05a34abc8583612b44de521", latitude, longitude, Unit.si);
			var response = request.Get();
			var temperature = response != null ? response.currently.apparentTemperature : 0.0f;
			var message = getMessageForTemp(temperature);
			Response resp = new Response()
			{
				Message = message,
				Temperature = temperature
			};
			string json = JsonConvert.SerializeObject(resp);		
			return json;
		}

		public string getMessageForTemp(double temperature)
		{
			using (XmlReader reader = XmlReader.Create(HttpContext.Current.Server.MapPath("~/App_Data/TemperatureMap.xml")))
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == "WeatherData")
					{
						XmlReader forecastReader = reader.ReadSubtree();

						while (forecastReader.Read())
						{
							if (forecastReader.NodeType == XmlNodeType.Element && forecastReader.Name == "Forecast")
							{
								XmlReader tempReader = forecastReader.ReadSubtree();

								while (tempReader.Read())
								{
									if (tempReader.NodeType == XmlNodeType.Element && tempReader.Name == "Temperature")
									{
										int min = Convert.ToInt32(tempReader.GetAttribute("Min").ToString());
										if (temperature > min)
										{
											int max = Convert.ToInt32(tempReader.GetAttribute("Max").ToString());
											if (temperature < max)
											{
												return tempReader.GetAttribute("Message").ToString();
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return "not found";
		}

		
		// POST api/values
		public void Post([FromBody]string value)
		{
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}
	}
}